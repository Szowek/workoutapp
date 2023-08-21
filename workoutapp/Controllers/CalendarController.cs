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
    [Route("api/{userId}/calendars")]
    [ApiController]
    [Authorize]
    public class CalendarController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CalendarController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCalendar([FromRoute] int userId, [FromBody] CreateCalendarDto dto)
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


            var newCalendar = _mapper.Map<Calendar>(dto);

            newCalendar.UserId = userId;

            _context.Calendars.Add(newCalendar);
            _context.SaveChanges();

            var calendarId = newCalendar.CalendarId;
            return new ObjectResult(calendarId);

        }

        
        [HttpDelete("{calendarId}/delete")]
        public async Task<IActionResult> DeleteCalendar([FromRoute] int userId, [FromRoute] int calendarId)
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

            _context.Calendars.Remove(calendar);
            _context.SaveChanges();

            return Ok("Usunales kalendarz");

        }

        [HttpGet("getAllUserCalendars")]
        public async Task<IActionResult> GetAllUserCalendars([FromRoute] int userId)
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

            var calendarsDtos = _mapper.Map<List<CalendarDto>>(user.Calendars);

            return Ok(calendarsDtos);

        }

        //getting calendar count
        [HttpGet("count")]
        public async Task<IActionResult> GetUserCalendarsCount([FromRoute] int userId)
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

            var count = _context.Calendars.Count(c => c.UserId == userId);
            return Ok(count);
        }

        [HttpGet("{calendarId}")]
        public async Task<IActionResult> GetCalendarById([FromRoute] int userId, [FromRoute] int calendarId)
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

            var calendarDto = _mapper.Map<CalendarDto>(calendar);

            return Ok(calendarDto);

        }

        [HttpGet("getAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCalendars()
        {
            var calendars = _context.Calendars
                .Include(u => u.User)
                .ToList();

            return Ok(calendars);
        }

    }
}
