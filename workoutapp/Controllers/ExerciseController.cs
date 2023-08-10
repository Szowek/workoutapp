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
    [Route("api/{userId}/workoutplans/{workoutPlanId}/workoutdays/{workoutDayId}/exercises")]
    [ApiController]
    [Authorize]
    public class ExerciseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ExerciseController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateExercise([FromRoute] int userId, [FromRoute] int workoutPlanId, [FromRoute] int workoutDayId, [FromBody] CreateExerciseDto dto)
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

            if (workoutPlan.User.UserId != userId)
            {
                return Forbid();  
            }

            var workoutDay = await _context.WorkoutDays
                .Include(wd => wd.WorkoutPlan)
                .FirstOrDefaultAsync(wd => wd.WorkoutDayId == workoutDayId);

            if(workoutDay == null) 
            {
                return NotFound();
            }

            
            if(workoutDay.WorkoutPlan.WorkoutPlanId != workoutPlanId)
            {
                return Forbid();
            }      
          
            var newExercise = _mapper.Map<UserExercise>(dto);
            newExercise.WorkoutDayId = workoutDayId;

            _context.UserExercises.Add(newExercise);
            _context.SaveChanges();

            var exerciseId = newExercise.UserExerciseId;
 
            return Created($"api/{userId}/workoutplans/{workoutPlanId}/workoutdays/{workoutDayId}/exercises/{exerciseId}", null);

        }
        
        [HttpPost("delete/{userExerciseId}")]
        public async Task<IActionResult> DeleteExercise([FromRoute] int userId, [FromRoute] int workoutPlanId, [FromRoute] int workoutDayId, [FromRoute] int userExerciseId )
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

            var workoutDay = await _context.WorkoutDays
                .Include(wd => wd.WorkoutPlan)
                .Include(wd=>wd.UserExercises)
                .FirstOrDefaultAsync(wd => wd.WorkoutDayId == workoutDayId);

            if (workoutDay == null)
            {
                return NotFound();
            }

            if(workoutDay.WorkoutPlanId != workoutPlanId) 
            {
                return Forbid();
            }

            var exercise =  _context.UserExercises
                .Include(e => e.WorkoutDay)
                .FirstOrDefault(e => e.UserExerciseId == userExerciseId);

            if (exercise == null)
            {
                return NotFound();
            }

            if (exercise.UserExerciseId != userExerciseId)
            {
                return Forbid();
            }

            _context.UserExercises.Remove(exercise);
            _context.SaveChanges();

            return Ok("Usunales Exercise");

        }

        [HttpGet("GetAllExercises")]
        public async Task<IActionResult> GetAllExercises([FromRoute] int userId, [FromRoute] int workoutPlanId, [FromRoute] int workoutDayId)
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

            var workoutDay = await _context.WorkoutDays
                .Include(wd => wd.WorkoutPlan)
                .Include(wd => wd.UserExercises)
                .FirstOrDefaultAsync(wd => wd.WorkoutDayId == workoutDayId);

            if (workoutDay == null)
            {
                return NotFound();
            }

            if (workoutDay.WorkoutPlanId != workoutPlanId)
            {
                return Forbid();
            }

            var exercisesDtos = _mapper.Map<List<UserExerciseDto>>(workoutDay.UserExercises);

            return Ok(exercisesDtos);

        }

        [HttpPut("{userExerciseId}/editNumbers")]
        public async Task<IActionResult> EditNumbers([FromRoute] int userId, [FromRoute] int workoutPlanId, [FromRoute] int workoutDayId, [FromRoute] int userExerciseId, [FromBody] UpdateExerciseNumbersDto dto)
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

            var workoutDay = await _context.WorkoutDays
                .Include(wd => wd.WorkoutPlan)
                .Include(wd => wd.UserExercises)
                .FirstOrDefaultAsync(wd => wd.WorkoutDayId == workoutDayId);

            if (workoutDay == null)
            {
                return NotFound();
            }

            if (workoutDay.WorkoutPlanId != workoutPlanId)
            {
                return Forbid();
            }


            var exercise = _context.UserExercises
              .Include(e => e.WorkoutDay)
              .FirstOrDefault(e => e.UserExerciseId == userExerciseId);

            if (exercise == null)
            {
                return NotFound();
            }

            if (exercise.UserExerciseId != userExerciseId)
            {
                return Forbid();
            }


            exercise.NumberOfSeries = dto.NumberOfSeries; 
            if (exercise.NumberOfSeries < 1)
            {
                return BadRequest("Licza serii musi wynosic przynajmniej 1");
            }

           exercise.NumberOfRepeats = dto.NumberOfRepeats; 
            if (exercise.NumberOfRepeats < 1)
            {
                return BadRequest("Licza powtorzen musi wynosic przynajmniej 1");
            }

            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("{userExerciseId}")]
        public async Task<IActionResult> GetExerciseById([FromRoute] int userId, [FromRoute] int workoutPlanId, [FromRoute] int workoutDayId, [FromRoute] int userExerciseId)
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

            var workoutDay = await _context.WorkoutDays
                .Include(wd => wd.WorkoutPlan)
                .Include(wd => wd.UserExercises)
                .FirstOrDefaultAsync(wd => wd.WorkoutDayId == workoutDayId);

            if (workoutDay == null)
            {
                return NotFound();
            }

            if (workoutDay.WorkoutPlanId != workoutPlanId)
            {
                return Forbid();
            }


            var exercise = _context.UserExercises
              .Include(e => e.WorkoutDay)
              .FirstOrDefault(e => e.UserExerciseId == userExerciseId);

            if (exercise == null)
            {
                return NotFound();
            }

            if (exercise.UserExerciseId != userExerciseId)
            {
                return Forbid();
            }

            var exerciseDto = _mapper.Map<UserExerciseDto>(exercise);

            return Ok(exerciseDto);
        }



        [HttpGet("getAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllExercise()
        {
            var exercises = _context.UserExercises
            .Include(e=> e.WorkoutDay)
            .ToList();

            return Ok(exercises);
        }
    }
}
