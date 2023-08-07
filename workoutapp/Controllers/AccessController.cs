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
    [Route("api/access")]
    [ApiController]
    public class AccessController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
       

        public AccessController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var existingUsername = _context.Users.FirstOrDefault(u => u.Username == dto.Username);
                var existingEmail = _context.Users.FirstOrDefault(u => u.Email == dto.Email);

                dto.Password = Password.hashPassword(dto.Password);

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
                if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 3)
                {
                    return BadRequest("Haslo musi zawierac co najmniej 3 znaki.");
                }


                var newUser = new User()
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    Password = dto.Password
                };

                //dodanie uzytkownika do bazy danych
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return Ok("Zarejestrowales sie");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                string password = Password.hashPassword(dto.Password);

                var existingUser = _context.Users.Where(u => u.Username == dto.Username && u.Password == password).Select(u => new
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


    }
}
