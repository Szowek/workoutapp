using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using workoutapp.DAL;
using workoutapp.Models;
using workoutapp.Tools;

namespace workoutapp.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            try
            {
                var existingUsername = _context.Users.FirstOrDefault(u => u.Username == user.Username);
                var existingEmail = _context.Users.FirstOrDefault(u => u.Email == user.Email);

                user.Password = Password.hashPassword(user.Password);

                //sprawdzenie czy uzytkownik o podanej nazwie juz istnieje
                if (existingUsername != null)
                {
                    return BadRequest("Uzytkownik o podanej nazwie juz istnieje.");
                }

                //sprawdzenie czy uzytkownik o podanym emailu juz istnieje
                if (existingEmail != null)
                {
                    return BadRequest("Uzytkownik o podanym adresie email juz istnieje.");
                }

                //walidacja hasla
                if (string.IsNullOrWhiteSpace(user.Password) || user.Password.Length < 3)
                {
                    return BadRequest("Haslo musi zawierac co najmniej 3 znaki.");
                }

                //dodanie uzytkownika do bazy danych
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok("Zarejestrowales sie");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            try
            {
                string password = Password.hashPassword(user.Password);

                var existingUser = _context.Users.Where(u => u.Username == user.Username && u.Password == password).FirstOrDefault();

                if (existingUser == null)
                {
                    return BadRequest("Nieprawidlowa nazwa uzytkownika lub haslo");
                }

                return Ok(existingUser);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(_context.Users.ToList());
        }
    }
}
