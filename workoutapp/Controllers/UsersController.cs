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
using workoutapp.Models;
using workoutapp.Tools;

namespace workoutapp.Controllers
{
    [EnableCors("FrontEnd")]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

                var existingUser = _context.Users.Where(u => u.Username == user.Username && u.Password == password).Select(u => new
                {
                    u.UserId,
                    u.Username
                }).FirstOrDefault();

                if (existingUser == null)
                {
                    return BadRequest("Nieprawidlowa nazwa uzytkownika lub haslo");
                }

                List<Claim> autClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, existingUser.Username),
                    new Claim("userID", existingUser.UserId.ToString()),
                    new Claim("Username", existingUser.Username)
                };

                var token = this.getToken(autClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            //var users = _context.Users
              //.Include(wp => wp.WorkoutPlans)
              //.ToList();

            return Ok(_context.Users.ToList());
        }


        [HttpPut("change/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] User userDto)
        {
            var user = _context
                .Users
                .FirstOrDefault(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();  
            }

            user.Email = userDto.Email;
            user.Password = userDto.Password;

            var existingEmail = _context.Users.FirstOrDefault(u => u.Email == user.Email);

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

            user.Password = Password.hashPassword(user.Password);

            _context.SaveChanges();

            return Ok();

        }

        private JwtSecurityToken getToken(List<Claim> authClaim)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken
            (
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(24),
                claims: authClaim,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

          return token;
           
        }

        [Authorize]
        [HttpGet("logged")]
        public async Task<IActionResult> getloggedInUser()
        {
            int id = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));
            string username = HttpContext.User.FindFirstValue("Username");
            return Ok(new { UserId = id, Username = username });
        }




    }
}
