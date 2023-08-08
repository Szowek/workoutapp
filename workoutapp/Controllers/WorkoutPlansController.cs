using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using workoutapp.DAL;
using workoutapp.Dtos;
using workoutapp.Models;

namespace workoutapp.Controllers
{

    [Route("api/workoutplans")]
    [ApiController]
    public class WorkoutPlansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public WorkoutPlansController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Metoda tworzenia WorkoutPlanu dla danego użytkownika
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateWorkoutPlan([FromBody] CreateWorkoutPlanDto dto)
        {
            int userID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));
            var user = _context.Users.Include(u => u.WorkoutPlans).FirstOrDefault(u => u.UserId == userID);

            if (userID == 0) 
            {
                return BadRequest();
            }

            var newWorkoutPlan = _mapper.Map<WorkoutPlan>(dto);

            newWorkoutPlan.UserId = userID;

            _context.WorkoutPlans.Add(newWorkoutPlan);
            _context.SaveChanges();
          
            return Ok("Stworzyles WorkoutPlan");

            //var id = workoutPlan.WorkoutPlanId;
            //return Created($"/api//workoutplans/{id}", null);


        }

        // Metoda usuwania WorkoutPlanu danego użytkownika
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

        
        //Metoda zwracajaca wszystkie WorkoutPlany danego uzytkownika
        [HttpGet("getAllUserWorkoutPlans")]
        public async Task<IActionResult> GetAllUserWorkoutPlans()
        {
            int userID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));
            var user = _context.Users.Include(u => u.WorkoutPlans).FirstOrDefault(u => u.UserId == userID);
            
            if (userID == 0)
            {
                return BadRequest();
            }

            var workoutplansDtos = _mapper.Map<List< WorkoutPlanDto>>(user.WorkoutPlans);

            return Ok(workoutplansDtos);
           
        }
        


        // Metoda zwracająca wszystkie WorkoutPlany wszystkich uzytkownikow 
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllWorkoutPlans()
        {
            var workoutPlans = _context.WorkoutPlans
                .Include(u => u.User)
                .ToList();

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
