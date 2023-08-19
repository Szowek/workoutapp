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
    [Route("api/{userId}/calendars/{calendarId}/calendardays/{calendarDayId}/meals")]
    [ApiController]
    [Authorize]
    public class MealController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MealController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateMeal([FromRoute] int userId, [FromRoute] int calendarId, [FromRoute] int calendarDayId, [FromBody] CreateMealDto dto)
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

            var calendar = await _context
            .Calendars
            .FirstOrDefaultAsync(c => c.CalendarId == calendarId);

            if (calendar == null)
            {
                return NotFound();
            }

            if (calendar.User.UserId != userId)
            {
                return Forbid();
            }

            var calendarDay = await _context.CalendarDays
               .Include(c => c.Calendar)
               .FirstOrDefaultAsync(c => c.CalendarDayId == calendarDayId);

            if (calendarDay == null)
            {
                return NotFound();
            }


            if (calendarDay.Calendar.CalendarId != calendarId)
            {
                return Forbid();
            }


            var newMeal= _mapper.Map<Meal>(dto);


            newMeal.CalendarDayId = calendarDayId;

            _context.Meals.Add(newMeal);
            _context.SaveChanges();

            var mealId = newMeal.MealId;

            return Created($"/api/{userId}/calendars/{calendarId}/calendardays/{calendarDayId}/meals/{mealId}", null);

        }

        [HttpDelete("{mealId}/delete")]
        public async Task<IActionResult> DeleteMeal([FromRoute] int userId, [FromRoute] int calendarId, [FromRoute] int calendarDayId, [FromRoute] int mealId)
        {
            int loggeduserID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));

            var user = _context
                .Users
                .Include(u => u.Calendars)
                .FirstOrDefault(u => u.UserId == loggeduserID);

            if (user == null)
            {
                return NotFound();
            }

            if (loggeduserID != user.UserId)
            {
                return Forbid();
            }

            var calendar = _context
                .Calendars
                .Include(c => c.CalendarDays)
                .FirstOrDefault(c => c.CalendarId == calendarId);

            if (calendar == null)
            {
                return NotFound();
            }

            if (calendar.UserId != userId)
            {
                return Forbid();
            }

            var calendarDay = calendar.CalendarDays.FirstOrDefault(c => c.CalendarDayId == calendarDayId);
            if (calendarDay == null)
            {
                return NotFound();
            }

            if (calendarDay.CalendarId != calendarId)
            {
                return Forbid();
            }


            var meal = _context.Meals
                .Include(m => m.CalendarDay)
                .Include(m=>m.Products)
                .FirstOrDefault(m => m.MealId == mealId);

            if (meal == null)
            {
                return NotFound();
            }

            if (meal.CalendarDayId != calendarDayId)
            {
                return Forbid();
            }

            _context.Meals.Remove(meal);
            _context.SaveChanges();

            return Ok("Usunales Meal");
        }


        [HttpGet]
        public async Task<IActionResult> GetAllMealsCD([FromRoute] int userId, [FromRoute] int calendarId, [FromRoute] int calendarDayId)
        {

            int loggeduserID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));

            var user = _context
                .Users
                .Include(u => u.Calendars)
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            if (user.UserId != loggeduserID)
            {

                return Forbid();
            }

            var calendar = await _context.Calendars
            .Include(c => c.User)
            .Include(c => c.CalendarDays)
            .FirstOrDefaultAsync(c => c.CalendarId == calendarId);

            if (calendar == null)
            {
                return NotFound();
            }

            if (calendar.User.UserId != userId)
            {
                return Forbid();
            }

            var calendarDay = await _context.CalendarDays
               .Include(c => c.Meals)
               .FirstOrDefaultAsync(c => c.CalendarDayId == calendarDayId);

            if (calendarDay == null)
            {
                return NotFound();
            }

            if (calendarDay.CalendarId != calendarId)
            {
                return Forbid();
            }

            var mealsDtos = _mapper.Map<List<MealDto>>(calendarDay.Meals);

            return Ok(mealsDtos);

        }

        [HttpGet("{mealId}")]
        public async Task<IActionResult> GetMealById([FromRoute] int userId, [FromRoute] int calendarId, [FromRoute] int calendarDayId, [FromRoute] int mealId)
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

            var calendar = _context
                .Calendars
                .Include(c => c.CalendarDays)
                .FirstOrDefault(c => c.CalendarId == calendarId);

            if (calendar == null)
            {
                return NotFound();
            }

            if (calendar.UserId != userId)
            {
                return Forbid();
            }

            var calendarDay = _context
                .CalendarDays
                .Include(c => c.Meals)
                .Include(c => c.WorkoutDay)
                .FirstOrDefault(c => c.CalendarDayId == calendarDayId);

            if (calendarDay == null)
            {
                return NotFound();
            }

            if (calendarDay.CalendarId != calendarId)
            {
                return Forbid();
            }

            var meal = _context
                .Meals
                .Include(c => c.CalendarDay)
                .Include(m=>m.Products)
                .FirstOrDefault(c => c.CalendarDayId == calendarDayId);

            if (meal == null)
            {
                return NotFound();
            }

            if (meal.CalendarDayId != calendarDayId)
            {
                return Forbid();
            }

            var mealDto = _mapper.Map<MealDto>(meal);

            return Ok(mealDto);

        }



        [HttpPut("{mealId}/edit")]
        public async Task<IActionResult> GetMealById([FromRoute] int userId, [FromRoute] int calendarId, [FromRoute] int calendarDayId, [FromRoute] int mealId,
            [FromBody] UpdateMealNameDto dto)
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

            var calendar = _context
                .Calendars
                .Include(c => c.CalendarDays)
                .FirstOrDefault(c => c.CalendarId == calendarId);

            if (calendar == null)
            {
                return NotFound();
            }

            if (calendar.UserId != userId)
            {
                return Forbid();
            }

            var calendarDay = _context
                .CalendarDays
                .Include(c => c.Meals)
                .Include(c => c.WorkoutDay)
                .FirstOrDefault(c => c.CalendarDayId == calendarDayId);

            if (calendarDay == null)
            {
                return NotFound();
            }

            if (calendarDay.CalendarId != calendarId)
            {
                return Forbid();
            }

            var meal = _context
                .Meals
                .Include(c => c.CalendarDay)
                .Include(m => m.Products)
                .FirstOrDefault(c => c.CalendarDayId == calendarDayId);

            if (meal == null)
            {
                return NotFound();
            }

            if (meal.CalendarDayId != calendarDayId)
            {
                return Forbid();
            }

            meal.MealName = dto.MealName;

            _context.SaveChanges();

            return Ok();
        }




        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllMeals()
        {
            var meals = _context.Meals
                .Include(m => m.CalendarDay)
                .ToList();

            return Ok(meals);
        }

    }
}
