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
    [Route("api/{userId}/calendars/{calendarId}/calendardays")]
    [ApiController]
    [Authorize]
    public class CalendarDayController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CalendarDayController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("getByDate/{date}")]
        public async Task<IActionResult> GetCalendarDayId([FromRoute] int userId, [FromRoute] int calendarId, [FromRoute] string date)
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

            var calendarDay = await _context
                .CalendarDays
                .Where(cd => cd.CalendarDate == date)
                .FirstOrDefaultAsync();

            if(calendarDay == null)
            {
                return NotFound();
            }

            return Ok(calendarDay.CalendarDayId);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCalendarDay([FromRoute] int userId, [FromRoute] int calendarId, [FromBody] CreateCalendarDayDto dto)
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
            .Include(c=>c.User)
            .Include(c=>c.CalendarDays)
            .FirstOrDefaultAsync(c => c.CalendarId == calendarId);

            if (calendar == null)
            {
                return NotFound();
            }

            if (calendar.User.UserId != userId)
            {
                return Forbid();
            }

            var newCalendarDay = _mapper.Map<CalendarDay>(dto);

            newCalendarDay.CalendarId = calendarId;

            var date = newCalendarDay.CalendarDate;

            var existingCalendarDay = await _context.CalendarDays
              .Where(cd => cd.Calendar.UserId == userId && cd.CalendarDate == date && cd.CalendarDate != "base")
              .FirstOrDefaultAsync();

           // var existingCalendarDay =  calendar.CalendarDays.FirstOrDefault(c => c.CalendarDate == date);
            if (existingCalendarDay != null)
            {
                return BadRequest("Ta data juz istnieje w twoich kalendarzach");
            }

            _context.CalendarDays.Add(newCalendarDay);
            _context.SaveChanges();

            var calendarDayId = newCalendarDay.CalendarDayId;

            return new ObjectResult(calendarDayId);

        }


        [HttpDelete("{calendarDayId}/delete")]
        public async Task<IActionResult> DeleteCalendarDay([FromRoute] int userId, [FromRoute] int calendarId, [FromRoute] int calendarDayId)
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


            _context.CalendarDays.Remove(calendarDay);
            _context.SaveChanges();

            return Ok("Usunales Calendar Day");

        }

        [HttpGet]
        public async Task<IActionResult> GetAllCalendarDaysC([FromRoute] int userId, [FromRoute] int calendarId)
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
            .Include(c=>c.User)
            .Include(c => c.CalendarDays)
            .FirstOrDefaultAsync(c => c.CalendarId == calendarId);

            if (calendar == null)
            {
                return NotFound();
            }

            if (calendar.User.UserId != loggeduserID)
            {
                return Forbid();
            }

            var calendardaysDtos = _mapper.Map<List<CalendarDayDto>>(calendar.CalendarDays);

            return Ok(calendardaysDtos);

        }

        [HttpGet("{calendarDayId}")]
        public async Task<IActionResult> GetCalendarDayById([FromRoute] int userId, [FromRoute] int calendarId, [FromRoute] int calendarDayId)
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
                .Include(c=>c.WorkoutDay)
                .FirstOrDefault(c => c.CalendarDayId == calendarDayId);

            if (calendarDay == null)
            {
                return NotFound();
            }

            if (calendarDay.CalendarId != calendarId)
            {
                return Forbid();
            }

            var calendarDto = _mapper.Map<CalendarDayDto>(calendarDay);

            return Ok(calendarDto);

        }



        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCalendarDays()
        {
            var calendarDays = _context.CalendarDays
                .Include(c => c.Calendar)
                .ToList();

            return Ok(calendarDays);

        }
    }
}
