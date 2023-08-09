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
            .FirstOrDefaultAsync(wp => wp.WorkoutPlanId == workoutPlanId);

            if (workoutPlan == null)
            {
                return NotFound(); 
            }

            if(workoutPlan.User.UserId != loggeduserID)
            {
                return Forbid();  // Użytkownik nie ma uprawnień do tworzenia WorkoutDay dla WorkoutPlan innego użytkownika
            }

            var newWorkoutDay = _mapper.Map<WorkoutDay>(dto);
            newWorkoutDay.WorkoutPlanId = workoutPlanId;

            _context.WorkoutDays.Add(newWorkoutDay);
            _context.SaveChanges();

            var workoutdayId = newWorkoutDay.WorkoutDayId;
            //return Ok("Stworzyles WorkoutDay");
            return Created($"api/{userId}/workoutplans/{workoutPlanId}/workoutdays/{workoutdayId}", null);

        }


        // Metoda usuwania WorkoutDaya dla danego WorkoutPlanu
        [HttpDelete("delete/{workoutDayId}")]
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

                if (workoutPlan.User.UserId != loggeduserID)
                {
                    return Forbid();
                }

                var workoutDay = workoutPlan.WorkoutDays.FirstOrDefault(wd => wd.WorkoutDayId == workoutDayId);
                if (workoutDay == null)
                {
                    return NotFound();
                }

                _context.WorkoutDays.Remove(workoutDay);
                _context.SaveChanges();

                return Ok("Usunales WorkoutDay");
            
        }

        //metoda zwracajaca wszystkie WorkoutDay danego WorkoutPlan
        [HttpGet("getAllWorkoutDays")]
        public async Task<IActionResult> GetAllWorkoutDaysWP([FromRoute] int userId, [FromRoute] int workoutPlanId)
        {
            int loggeduserID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));

            var workoutPlan = await _context.WorkoutPlans
           .Include(wp => wp.User)
           .Include(wp => wp.WorkoutDays)
           .FirstOrDefaultAsync(wp => wp.WorkoutPlanId == workoutPlanId);

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

            if (workoutPlan == null)
            {
                return NotFound();
            }

            if (workoutPlan.User.UserId != loggeduserID)
            {
                return Forbid();
            }

            //var wpID =  _context.WorkoutPlans.FirstOrDefaultAsync(wp => wp.WorkoutPlanId== workoutPlanId);
            //var workoutplan = _context.WorkoutPlans.Include(wp=>wp.WorkoutDays).FirstOrDefault(wp=>wp.WorkoutPlanId);
            /*
            var WorkoutDayList = _context.WorkoutDays.Select(wd => new
            {
                wd.WorkoutDayId,
                wd.WorkoutPlanId

            }).Where(wd => wd.WorkoutPlanId == workoutPlanId).ToList();
            */

            var workoutdaysDtos = _mapper.Map<List<WorkoutDayDto>>(workoutPlan.WorkoutDays);

            return Ok(workoutdaysDtos);

        }


        // Metoda zwracająca wszystkie WorkoutDay wszystkich WorkoutPlan
        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllWorkoutDays()
        {
            var workoutDays = _context.WorkoutDays
                .Include(wp => wp.WorkoutPlan)
                .ToList();

            var workoutdaysDtos = _mapper.Map<List<WorkoutDayDto>>(workoutDays);

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

            if (workoutPlan.User.UserId != loggeduserID)
            {
                return Forbid();
            }


            var workoutDay = workoutPlan.WorkoutDays.FirstOrDefault(wd => wd.WorkoutDayId == workoutDayId);

            if(workoutDay == null) 
            {
                return NotFound();
            }

            var workoutDayDto = _mapper.Map<WorkoutDayDto>(workoutDay);

            return Ok(workoutDayDto);
        }
    }
}
