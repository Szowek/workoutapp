using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using workoutapp.DAL;
using workoutapp.Models;

namespace workoutapp.Controllers
{

    [Route("api/workoutplans")]
    [ApiController]
    public class WorkoutPlansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkoutPlansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Metoda tworzenia WorkoutPlanu dla danego użytkownika
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateWorkoutPlan([FromBody]WorkoutPlanDto workoutPlan)
        {
            int userID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));
            var user = _context.Users.Include(u => u.WorkoutPlans).FirstOrDefault(u => u.UserId == userID);

            if (userID == 0) 
            {
                return BadRequest();
            }

            var newWorkoutPlan = new WorkoutPlan
            {
                UserId = userID
            };

            _context.WorkoutPlans.Add(newWorkoutPlan);
            _context.SaveChanges();
          
            return Ok("Stworzyles WorkoutPlan");

            //var id = workoutPlan.WorkoutPlanId;
            //return Created($"/api//workoutplans/{id}", null);


        }

        // Metoda usuwania WorkoutPlanu dla danego użytkownika
        [HttpDelete("{workoutPlanId}")]
        public async Task<IActionResult> DeleteWorkoutPlan([FromRoute] int workoutPlanId)
        {
            int userID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));
            var user = _context.Users.Include(u => u.WorkoutPlans).FirstOrDefault(u => u.UserId == userID);
            

            if (user != null)
            {
                var workoutPlan = user.WorkoutPlans.FirstOrDefault(wp => wp.WorkoutPlanId == workoutPlanId);
                if (workoutPlan != null)
                {
                    _context.WorkoutPlans.Remove(workoutPlan);
                    _context.SaveChanges();
                    return Ok("Usunales WorkoutPlan");
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        
        
        [HttpGet("getAllUserWorkoutPlans")]
        public async Task<IActionResult> GetAllUserWorkoutPlans()
        {
            int userID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));
            var user = _context.Users.Include(u => u.WorkoutPlans).FirstOrDefault(u => u.UserId == userID);
            
            if (userID == 0)
            {
                return BadRequest();
            }
            return Ok();

            //var users = _context.Users
            //.Include(wp => wp.WorkoutPlans)
            //.ToList();
            //workoutPlan.UserId = user.UserId; // Przypisanie klucza obcego

            //var workoutPlanId = workoutPlan.WorkoutPlanId;
            /*
            var workoutplans = _context.WorkoutPlans
            .Include(u => u.UserId)
            .ToList();
            */

            //var workoutPlans = user.WorkoutPlans.FirstOrDefault(wp => wp.WorkoutPlanId == workoutPlanId);
            //var workoutPlans = _context.WorkoutPlans.ToList();
            //var workoutPlans = _context.WorkoutPlans.Include(u => u.UserId).ToList();
             //var wp = HttpContext.User.FindFirstValue("WorkoutPlans");

            //return Ok(workoutplans);
            /*
            var user = _context.Users.Include(u => u.WorkoutPlans).FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {
                var workoutPlans = user.WorkoutPlans.ToList();
                return Ok(workoutPlans);
            }
            else
            {
                return NotFound();
            }
            */
        }
        


        // Metoda zwracająca wszystkie WorkoutPlany
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllWorkoutPlans()
        {
            var workoutPlans = _context.WorkoutPlans.ToList();

            return Ok(workoutPlans);
        }

        // Metoda zwracająca WorkoutPlan użytkownika na podstawie id
        [HttpGet("{workoutPlanId}")]
        [Authorize]
        public async Task<IActionResult> GetWorkoutPlanById([FromRoute] int workoutPlanId)
        {
            int userID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));
            var user = _context.Users.Include(u => u.WorkoutPlans).FirstOrDefault(u => u.UserId == userID);

            if (user != null)
            {
                var workoutPlan = user.WorkoutPlans.FirstOrDefault(wp => wp.WorkoutPlanId == workoutPlanId);
                if (workoutPlan != null)
                {
                    return Ok(new { UserId = userID, WorkoutPlanId = workoutPlanId });
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

    }

}
