using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Newtonsoft.Json.Schema;
using System.Security.Claims;
using workoutapp.DAL;
using workoutapp.Dtos;
using workoutapp.Models;

namespace workoutapp.Controllers
{
    [Route("api/{userId}/workoutplans/{workoutPlanId}/workoutdays")]
    //[Route("api/workoutplans/{workoutPlanId}/workoutdays")] ????????????????????????????????
    [ApiController]
    [Authorize]
    public class WorkoutDaysController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public WorkoutDaysController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("getBaseDays")]
        public async Task<IActionResult> GetBaseDays([FromRoute] int userId, [FromRoute] int workoutPlanId)
        {
            int loggeduserID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));

            var user = _context
                .Users
                .Include(u => u.WorkoutPlans)
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            if (loggeduserID != user.UserId)
            {
                return Forbid();
            }


            var workoutPlan = await _context.WorkoutPlans
            .Include(wp => wp.User)
            .Include(wp => wp.WorkoutDays)
            .FirstOrDefaultAsync(wp => wp.WorkoutPlanId == workoutPlanId);

            if (workoutPlan == null)
            {
                return NotFound();
            }

            if (workoutPlan.User.UserId != userId)
            {
                return Forbid();
            }

            var baseWorkoutDays = _context
                .WorkoutDays
                .Include(wp => wp.UserExercises)
                .Where(wp => wp.CalendarDate == "base" && wp.WorkoutPlanId == workoutPlan.WorkoutPlanId)
                .ToList();
            if(baseWorkoutDays.Count == 0 || baseWorkoutDays == null)
            {
                return BadRequest();
            }

            return Ok(baseWorkoutDays);
        }


        [HttpGet("checkBaseDays")]
        public async Task<IActionResult> CheckForCompletedBaseDays([FromRoute] int userId, [FromRoute] int workoutPlanId)
        {
            int loggeduserID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));

            var user = _context
                .Users
                .Include(u => u.WorkoutPlans)
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            if (loggeduserID != user.UserId)
            {
                return Forbid();
            }


            var workoutPlan = await _context.WorkoutPlans
            .Include(wp => wp.User)
            .Include(wp => wp.WorkoutDays)
            .FirstOrDefaultAsync(wp => wp.WorkoutPlanId == workoutPlanId);

            if (workoutPlan == null)
            {
                return NotFound();
            }

            if (workoutPlan.User.UserId != userId)
            {
                return Forbid();
            }

            var baseWorkoutDays = _context
            .WorkoutDays
            .Where(bwd => bwd.WorkoutPlanId == workoutPlanId && bwd.CalendarDate == "base")
            .Include(bwd => bwd.UserExercises)
            .ToList();

            if (baseWorkoutDays == null || baseWorkoutDays.Count == 0)
                return BadRequest(false);

            for (int i = 0; i < baseWorkoutDays.Count; i++)
            {
                if (baseWorkoutDays[i].UserExercises.Count == 0)
                    return BadRequest(false);
            }

            return Ok(true);
        }

        //tworzenie workoutdaya na podstawie bazowego dnia 
        [HttpPost("create/nextDay/{baseDayId}")]
        public async Task<IActionResult> CreateNextWorkoutDay([FromRoute] int userId, [FromRoute] int workoutPlanId, [FromRoute] int baseDayId, [FromBody] CreateWorkoutDayDto dto)
        {
            int loggeduserID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));

            var user = _context
                .Users
                .Include(u => u.WorkoutPlans)
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            if (loggeduserID != user.UserId)
            {
                return Forbid();
            }


            var workoutPlan = await _context.WorkoutPlans
            .Include(wp => wp.User)
            .Include(wp => wp.WorkoutDays)
            .FirstOrDefaultAsync(wp => wp.WorkoutPlanId == workoutPlanId);

            if (workoutPlan == null)
            {
                return NotFound();
            }

            if (workoutPlan.User.UserId != userId)
            {
                return Forbid();
            }

            var newWorkoutDay = _mapper.Map<WorkoutDay>(dto);
            newWorkoutDay.WorkoutPlanId = workoutPlanId;
            var date = newWorkoutDay.CalendarDate;
            var existingWorkoutDay = await _context.WorkoutDays
                .Where(wd => wd.WorkoutPlan.UserId == userId && wd.CalendarDate == date )
                .FirstOrDefaultAsync();

            if (existingWorkoutDay != null)
            {
                return BadRequest("Dzien o tej dacie juz istnieje w twoich WorkoutPlanach");
            }

            if (_context.CalendarDays.Count(cd => cd.Calendar.UserId == userId && cd.CalendarDate == date) > 0)
            {
                var existingCalendarDay = await _context.CalendarDays
                    .Where(cd => cd.Calendar.UserId == userId && cd.CalendarDayId == newWorkoutDay.CalendarDayId)
                    .FirstOrDefaultAsync();
                if (existingCalendarDay == null)
                {
                    return BadRequest("Nie ma takiego utworzonego dnia");
                }
                newWorkoutDay.CalendarDayId = existingCalendarDay.CalendarDayId;
            }
            else
            {
                var existingCalendarDay = await _context.CalendarDays
                  .Where(cd => cd.Calendar.UserId == userId && cd.CalendarDate == date)
                  .FirstOrDefaultAsync();
                if (existingCalendarDay == null)
                {
                    return BadRequest("Nie ma takiego utworzonego dnia");
                }
                newWorkoutDay.CalendarDayId = existingCalendarDay.CalendarDayId;
            }

            //finding wanted base day
            var baseWorkoutDay = _context
                .WorkoutDays
                .FirstOrDefault(bwd => bwd.WorkoutDayId == baseDayId && bwd.CalendarDate == "base");
            if (baseWorkoutDay == null)
            {
                return BadRequest("Nie ma takiego dnia bazowego");
            }
          
            //using base workout day as a template to create new workout day
            newWorkoutDay.UserExercises = baseWorkoutDay.UserExercises;
            _context.WorkoutDays.Add(newWorkoutDay);
            _context.SaveChanges();

            var workoutdayId = newWorkoutDay.WorkoutDayId;



            //return Ok("Stworzyles WorkoutDay");
            return new ObjectResult(workoutdayId);
        }
        // Metoda tworzenia WorkoutDaya dla danego WorkoutPlanu
        [HttpPost("create")]
        public async Task<IActionResult> CreateWorkoutDay([FromRoute] int userId, [FromRoute] int workoutPlanId, [FromBody] CreateWorkoutDayDto dto)
        {

            int loggeduserID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));

            var user = _context
                .Users
                .Include(u => u.WorkoutPlans)
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            if (loggeduserID != user.UserId)
            {
                return Forbid();
            }


            var workoutPlan = await _context.WorkoutPlans
            .Include(wp => wp.User)
            .Include(wp=>wp.WorkoutDays)
            .FirstOrDefaultAsync(wp => wp.WorkoutPlanId == workoutPlanId);

            if (workoutPlan == null)
            {
                return NotFound(); 
            }

            if(workoutPlan.User.UserId != userId)
            {
                return Forbid(); 
            }

            var newWorkoutDay = _mapper.Map<WorkoutDay>(dto);
            newWorkoutDay.WorkoutPlanId = workoutPlanId;

            var date = newWorkoutDay.CalendarDate;

            //var existingWorkoutDay = workoutPlan.WorkoutDays.FirstOrDefault(wd => wd.CalendarDate == date);

            var existingWorkoutDay = await _context.WorkoutDays
                .Where(wd => wd.WorkoutPlan.UserId == userId && wd.CalendarDate == date && wd.CalendarDate != "base")
                .FirstOrDefaultAsync();

            if(existingWorkoutDay != null) 
            {
                return BadRequest("Dzien o tej dacie juz istnieje w twoich WorkoutPlanach");
            }

            if (_context.CalendarDays.Count(cd => cd.Calendar.UserId == userId && cd.CalendarDate == date) > 0)
            {
                var existingCalendarDay = await _context.CalendarDays
                    .Where(cd => cd.Calendar.UserId == userId && cd.CalendarDayId == newWorkoutDay.CalendarDayId)
                    .FirstOrDefaultAsync();
                if (existingCalendarDay == null)
                {
                    return BadRequest("Nie ma takiego utworzonego dnia");
                }
                newWorkoutDay.CalendarDayId = existingCalendarDay.CalendarDayId;
            }
            else
            {
                var existingCalendarDay = await _context.CalendarDays
                  .Where(cd => cd.Calendar.UserId == userId && cd.CalendarDate == date)
                  .FirstOrDefaultAsync();
                if (existingCalendarDay == null)
                {
                    return BadRequest("Nie ma takiego utworzonego dnia");
                }
                newWorkoutDay.CalendarDayId = existingCalendarDay.CalendarDayId;
            }

            _context.WorkoutDays.Add(newWorkoutDay);
            _context.SaveChanges();

            var workoutdayId = newWorkoutDay.WorkoutDayId;

         

            //return Ok("Stworzyles WorkoutDay");
            return new ObjectResult(workoutdayId);

        }


        // Metoda usuwania WorkoutDaya dla danego WorkoutPlanu
        [HttpDelete("{workoutDayId}/delete")]
        public async Task<IActionResult> DeleteWorkoutDay([FromRoute] int userId, [FromRoute] int workoutPlanId, [FromRoute] int workoutDayId)
        {
            int loggeduserID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));
            //var user = _context.Users.Include(u => u.WorkoutPlans).FirstOrDefault(u => u.UserId == userID);

            var user = _context
               .Users
               .Include(u => u.WorkoutPlans)
               .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            if (loggeduserID != user.UserId)
            {
                return Forbid();
            }


                var workoutPlan = await _context.WorkoutPlans
               .Include(wp => wp.User)
               .Include(wp => wp.WorkoutDays)
               .FirstOrDefaultAsync(wp => wp.WorkoutPlanId == workoutPlanId);


                if (workoutPlan == null)
                {
                    return NotFound();
                }

                if (workoutPlan.User.UserId != userId)
                {
                    return Forbid();
                }

                var workoutDay = workoutPlan.WorkoutDays.FirstOrDefault(wd => wd.WorkoutDayId == workoutDayId);
                if (workoutDay == null)
                {
                    return NotFound();
                }

                if(workoutDay.WorkoutPlanId != workoutPlanId)            
                {
                   return Forbid();
                }


                _context.WorkoutDays.Remove(workoutDay);
                _context.SaveChanges();

                return Ok("Usunales WorkoutDay");
            
        }

        //metoda zwracajaca wszystkie WorkoutDay danego WorkoutPlan
        [HttpGet]
        public async Task<IActionResult> GetAllWorkoutDaysWP([FromRoute] int userId, [FromRoute] int workoutPlanId)
        {
            int loggeduserID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));


            var user = _context
                .Users
                .Include(u => u.WorkoutPlans)
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            if (user.UserId != loggeduserID)
            {
                return Forbid();
            }

            var workoutPlan = await _context.WorkoutPlans
       .Include(wp => wp.User)
       .Include(wp => wp.WorkoutDays)
       .FirstOrDefaultAsync(wp => wp.WorkoutPlanId == workoutPlanId);

            if (workoutPlan == null)
            {
                return NotFound();
            }

            if (workoutPlan.User.UserId != loggeduserID)
            {
                return Forbid();
            }

            var workoutdaysDtos = _mapper.Map<List<WorkoutDayDto>>(workoutPlan.WorkoutDays);

            return Ok(workoutdaysDtos);

        }


        // Metoda zwracająca WorkoutDay na podstawie id dla WorkoutPlanu
        [HttpGet("{workoutDayId}")]
        public async Task<IActionResult> GetWorkoutDayById([FromRoute] int userId, [FromRoute] int workoutPlanId, [FromRoute] int workoutDayId)
        {
            int loggeduserID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));

            //var workoutPlan = await _context.WorkoutPlans.Include(wp => wp.WorkoutDays).FirstOrDefaultAsync(wp => wp.WorkoutPlanId == workoutPlanId);

            var user = _context
               .Users
               .Include(u => u.WorkoutPlans)
               .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            if (loggeduserID != user.UserId)
            {
                return Forbid();
            }

            var workoutPlan = await _context.WorkoutPlans
           .Include(wp => wp.User)
           .Include(wp => wp.WorkoutDays)
           .FirstOrDefaultAsync(wp => wp.WorkoutPlanId == workoutPlanId);

           
            if (workoutPlan == null)
            {
                return NotFound();
            }

            if (workoutPlan.User.UserId != userId)
            {
                return Forbid();
            }

            var workoutDay = _context
                .WorkoutDays
                .Include(wd=>wd.UserExercises)
                .FirstOrDefault(wd => wd.WorkoutDayId == workoutDayId);

            if(workoutDay == null) 
            {
                return NotFound();
            }

            if (workoutDay.WorkoutPlanId != workoutPlanId)
            {
                return Forbid();
            }

            var workoutDayDto = _mapper.Map<WorkoutDayDto>(workoutDay);

            return Ok(workoutDayDto);
        }


        // Metoda zwracająca wszystkie WorkoutDay wszystkich WorkoutPlan
        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllWorkoutDays()
        {
            var workoutDays = _context.WorkoutDays
                .Include(wp => wp.WorkoutPlan)
                .ToList();

            return Ok(workoutDays);

        }
    }
}
