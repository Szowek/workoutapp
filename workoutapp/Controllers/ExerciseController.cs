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


        [HttpPost("{userExerciseId}/delete")]
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

        [HttpGet]
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
                .FirstOrDefaultAsync(c => c.WorkoutDayId == workoutDayId);

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

        [HttpGet("getsamples")]
        public async Task<IActionResult> GetReadyExercises()
        {
            var exercises = _context
            .Exercises
            .ToList();

            return Ok(exercises);
        }


        [HttpPost("getsamples/{exerciseId}")]
        public async Task<IActionResult> AddToUserExercises([FromRoute] int userId, [FromRoute] int workoutPlanId, [FromRoute] int workoutDayId, [FromRoute] int exerciseId)
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
            /*
            var exercise = _context.Exercises
              .FirstOrDefault(e => e.ExerciseId == exerciseId);

            if (exercise == null)
            {
                return NotFound();
            }

            var addedExercise = _mapper.Map<UserExercise>(exercise);

            _context.UserExercises.Add(addedExercise);
            _context.SaveChanges();

            return Ok("Dodales cwiczenie");
            */
            var exerciseToAdd = await _context.Exercises.FirstOrDefaultAsync(e => e.ExerciseId == exerciseId);

            if (exerciseToAdd == null)
            {
                return NotFound("Exercise not found");
            }

            var userExercise = new UserExercise
            {
                ExerciseName = exerciseToAdd.ExerciseName,
                Description = exerciseToAdd.Description,
                BodyPart = exerciseToAdd.BodyPart,
                NumberOfSeries = exerciseToAdd.NumberOfSeries,
                NumberOfRepeats = exerciseToAdd.NumberOfRepeats,
                WorkoutDayId = workoutDayId
            };

            _context.UserExercises.Add(userExercise);
            _context.SaveChanges();

            return Ok("Dodales cwiczenie");

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
        public async Task<IActionResult> GetAllUserExercise()
        {
            var exercises = _context.UserExercises
            .Include(e=> e.WorkoutDay)
            .ToList();

            return Ok(exercises);
        }
    }
}
