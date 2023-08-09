using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;
using workoutapp.DAL;
using workoutapp.Dtos;
using workoutapp.Models;

namespace workoutapp.Controllers
{

    [Route("api/{userId}/workoutplans")]
    [ApiController]
    [Authorize]
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
        public async Task<IActionResult> CreateWorkoutPlan([FromRoute] int userId, [FromBody] CreateWorkoutPlanDto dto)
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

            
            var newWorkoutPlan = _mapper.Map<WorkoutPlan>(dto);

            newWorkoutPlan.UserId = userId;

            _context.WorkoutPlans.Add(newWorkoutPlan);
            _context.SaveChanges();
          
            //return Ok("Stworzyles WorkoutPlan");

            var workoutplanId = newWorkoutPlan.WorkoutPlanId;
            return Created($"/api{userId}/workoutplans/{workoutplanId}", null);

        }

        // Metoda usuwania WorkoutPlanu danego użytkownika
        [HttpDelete("{workoutPlanId}")]
        public async Task<IActionResult> DeleteWorkoutPlan([FromRoute] int userId, [FromRoute] int workoutPlanId)
        {
            int loggeduserID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));

            var user = _context
                .Users
                .Include(u => u.WorkoutPlans)
                .FirstOrDefault(u => u.UserId == loggeduserID);


            if (user == null)
            {
                return NotFound();
            }

            if (loggeduserID != user.UserId)
            {
                return Forbid();
            }

            var workoutPlan = user.WorkoutPlans.FirstOrDefault(wp => wp.WorkoutPlanId == workoutPlanId);

            if(workoutPlan == null) 
            {
                return NotFound();
            }

            _context.WorkoutPlans.Remove(workoutPlan);
            _context.SaveChanges();

            return Ok("Usunales WorkoutPlan");

        }

        
        //Metoda zwracajaca wszystkie WorkoutPlany danego uzytkownika
        [HttpGet("getAllUserWorkoutPlans")]
        public async Task<IActionResult> GetAllUserWorkoutPlans([FromRoute] int userId)
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

            if (user.UserId != loggeduserID )
            {
                return Forbid();
            }

            var workoutplansDtos = _mapper.Map<List< WorkoutPlanDto>>(user.WorkoutPlans);

            return Ok(workoutplansDtos);
           
        }
        


        // Metoda zwracająca wszystkie WorkoutPlany wszystkich uzytkownikow 
        [HttpGet("getAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllWorkoutPlans()
        {
            var workoutPlans = _context.WorkoutPlans
                .Include(u => u.User)
                .Include(wd=>wd.WorkoutDays)
                .ToList();

            var workoutplansDtos = _mapper.Map<List<WorkoutPlanDto>>(workoutPlans);

            return Ok(workoutplansDtos);
        }


        // Metoda zwracająca WorkoutPlan użytkownika na podstawie id
        [HttpGet("{workoutPlanId}")]
        public async Task<IActionResult> GetWorkoutPlanById([FromRoute] int userId, [FromRoute] int workoutPlanId)
        {
            int loggeduserID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));

            var user = _context
                .Users
                .Include(u => u.WorkoutPlans)
                .FirstOrDefault(u => u.UserId == userId);


            if(user == null)
            {
                return NotFound();
            } 
            
            if (user.UserId != loggeduserID ) 
            { 
                return Forbid(); 
            }

            var workoutPlan = user.WorkoutPlans.FirstOrDefault(wp => wp.WorkoutPlanId == workoutPlanId);

            if(workoutPlan == null) 
            {
                return NotFound();
            }

            var workoutPlanDto = _mapper.Map<WorkoutPlanDto>(workoutPlan);

            //return Ok(new { UserId = userId, WorkoutPlanId = workoutPlanId });


            return Ok(workoutPlanDto);

        }

    }

}
