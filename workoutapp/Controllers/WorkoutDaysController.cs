using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using workoutapp.DAL;
using workoutapp.Dtos;
using workoutapp.Models;

namespace workoutapp.Controllers
{
    [Route("api/workoutdays")]
    //[Route("api/workoutplans/{workoutPlanId}/workoutdays")] ????????????????????????????????
    [ApiController]
    public class WorkoutDaysController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkoutDaysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Metoda tworzenia WorkoutDaya dla danego WorkoutPlanu
        [HttpPost("{workoutPlanId}/create")]
        [Authorize]
        public async Task<IActionResult> CreateWorkoutDay([FromRoute] int workoutPlanId, [FromBody] WorkoutDay workoutDay)
        {

            int loggeduserID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));

            var workoutPlan = await _context.WorkoutPlans
            .Include(wp => wp.User)
            .FirstOrDefaultAsync(wp => wp.WorkoutPlanId == workoutPlanId);

            if (loggeduserID == 0)
            {
                return BadRequest();
            }

            if (workoutPlan == null || workoutPlan.User.UserId != loggeduserID)
            {
                return Forbid(); // Użytkownik nie ma uprawnień do tworzenia WorkoutDay dla WorkoutPlan innego użytkownika
            }

            var newWorkoutDay = new WorkoutDay
            {
                WorkoutPlanId = workoutPlanId
            };

            _context.WorkoutDays.Add(newWorkoutDay);
            _context.SaveChanges();

            return Ok("Stworzyles WorkoutDay");

        }


        // Metoda usuwania WorkoutDaya dla danego WorkoutPlanu
        [HttpDelete("{workoutPlanId}/{workoutDayId}")]
        [Authorize]
        public async Task<IActionResult> DeleteWorkoutDay([FromRoute] int workoutPlanId, [FromRoute] int workoutDayId)
        {
            int loggeduserID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));
            //var user = _context.Users.Include(u => u.WorkoutPlans).FirstOrDefault(u => u.UserId == userID);

            var workoutPlan = await _context.WorkoutPlans
             .Include(wp => wp.User)
             .Include(wp => wp.WorkoutDays)
             .FirstOrDefaultAsync(wp => wp.WorkoutPlanId == workoutPlanId);

            if (loggeduserID == 0)
            {
                return BadRequest();
            }

            if (workoutPlan == null || workoutPlan.User.UserId != loggeduserID)
            {
                return Forbid();
            }

            var workoutDay = workoutPlan.WorkoutDays.FirstOrDefault(wd => wd.WorkoutDayId == workoutDayId);
            if (workoutDay != null)
            {
                _context.WorkoutDays.Remove(workoutDay);
                _context.SaveChanges();
                return Ok("Usunales WorkoutDay");
            }
            else
            {
                return NotFound();
            }

        }

        //metoda zwracajaca wszystkie WorkoutDay danego WorkoutPlan
        [HttpGet("{workoutPlanId}/getAllWorkoutDays")]
        public async Task<IActionResult> GetAllWorkoutDaysWP(int workoutPlanId)
        {
            int loggeduserID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));

            var workoutPlan = await _context.WorkoutPlans
           .Include(wp => wp.User)
           .Include(wp => wp.WorkoutDays)
           .FirstOrDefaultAsync(wp => wp.WorkoutPlanId == workoutPlanId);

            if (loggeduserID == 0)
            {
                return BadRequest();
            }

            if (workoutPlan == null || workoutPlan.User.UserId != loggeduserID)
            {
                return Forbid();
            }

            //var wpID =  _context.WorkoutPlans.FirstOrDefaultAsync(wp => wp.WorkoutPlanId== workoutPlanId);
            //var workoutplan = _context.WorkoutPlans.Include(wp=>wp.WorkoutDays).FirstOrDefault(wp=>wp.WorkoutPlanId);

            var WorkoutDayList = _context.WorkoutDays.Select(wd => new
            {
                wd.WorkoutDayId,
                wd.WorkoutPlanId

            }).Where(wd => wd.WorkoutPlanId == workoutPlanId).ToList();


            return Ok(WorkoutDayList);

        }


        // Metoda zwracająca wszystkie WorkoutDay wszystkich WorkoutPlan
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllWorkoutDays(int workoutPlanId)
        {
            var workoutDays = _context.WorkoutDays.ToList();

            return Ok(workoutDays);
        }

        // Metoda zwracająca WorkoutDay na podstawie id dla WorkoutPlanu
        [HttpGet("{workoutPlanId}/{workoutDayId}")]
        [Authorize]
        public async Task<IActionResult> GetWorkoutDayById([FromRoute] int workoutPlanId, [FromRoute] int workoutDayId)
        {
            int loggeduserID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));

            //var workoutPlan = await _context.WorkoutPlans.Include(wp => wp.WorkoutDays).FirstOrDefaultAsync(wp => wp.WorkoutPlanId == workoutPlanId);

            var workoutPlan = await _context.WorkoutPlans
           .Include(wp => wp.User)
           .Include(wp => wp.WorkoutDays)
           .FirstOrDefaultAsync(wp => wp.WorkoutPlanId == workoutPlanId);

            if (loggeduserID == 0)
            {
                return BadRequest();
            }

            if (workoutPlan == null || workoutPlan.User.UserId != loggeduserID)
            {
                return Forbid();
            }

            var workoutDay = workoutPlan.WorkoutDays.FirstOrDefault(wd => wd.WorkoutDayId == workoutDayId);

            if (workoutDay != null)
            {
                return Ok(new { UserId = loggeduserID, WorkoutPlanId = workoutPlanId, WorkoutDayId = workoutDayId });
            }
            else
            {
                return NotFound();
            }
        }
    }
}
