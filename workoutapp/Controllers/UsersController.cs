using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using workoutapp.DAL;
using workoutapp.Dtos;
using workoutapp.Models;
using workoutapp.Tools;

namespace workoutapp.Controllers
{
    [EnableCors("FrontEnd")]
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper; 

        public UsersController(ApplicationDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }
    

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            int loggeduserID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));

            var user = _context
              .Users
              .Include(u => u.WorkoutPlans)
              .Include(u => u.Calendars)
              .FirstOrDefault(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            if (loggeduserID != user.UserId)
            {
                return Forbid();
            }
           
            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);

        }


        [HttpPut("{id}/change")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserDto dto)
        {
            int loggeduserID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));

            var user = _context
                .Users
                .FirstOrDefault(u => u.UserId == id);

            if(user == null)
            {
                return NotFound();
            }

            if (loggeduserID != user.UserId)
            {
                return Forbid();
            }


            user.Email = dto.Email;
            var existingEmail = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            //sprawdzenie czy uzytkownik o podanym emailu juz istnieje
            if (existingEmail != null)
            {
              return BadRequest("Uzytkownik o podanym adresie email juz istnieje.");
            }

            user.Password = dto.Password;
            if (user.Password.Length < 3)
            {
              return BadRequest("Haslo musi zawierac co najmniej 3 znaki.");
            }

            //walidacja hasla
            user.Password = Password.hashPassword(user.Password);
            

            _context.SaveChanges();

            return Ok();

        }

        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            int loggeduserID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));
 

            var user = _context
                .Users
                .FirstOrDefault(u => u.UserId == id);

            if(user  == null)
            {
                return NotFound();
            }

            if (loggeduserID != user.UserId)
            {
                return Forbid();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok("Usunales uzytkownika");
        }


        [HttpGet]
        public async Task<IActionResult> getloggedInUser()
        {
            int loggeduserID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));

            var user = _context
              .Users
              .Include(u=> u.WorkoutPlans)
              .Include(u=>u.Calendars)
              .FirstOrDefault(u => u.UserId == loggeduserID);

            if (user == null)
            {
                return NotFound();
            }


            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }

        [HttpGet("{id}/preferred")]
        public async Task<IActionResult> GetPreferredWorkoutPlans([FromRoute] int id, [FromRoute] WorkoutPlanDto dto)
        {
            int loggeduserID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));

            var user = _context
                .Users
                .Include(u=>u.WorkoutPlans)
                .FirstOrDefault(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            if (loggeduserID != user.UserId)
            {
                return Forbid();
            }

            var workoutPlans = user
               .WorkoutPlans
               .Where(wp => wp.isPreferred == true);

            
            var workoutplansDtos = _mapper.Map<List<WorkoutPlanDto>>(workoutPlans);

            return Ok(workoutplansDtos);
           
        }

        [HttpGet("getAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _context.Users
            .Include(wp => wp.WorkoutPlans)
            .Include(u => u.Calendars)
            .ToList();

            return Ok(users);
        }

    }
}
