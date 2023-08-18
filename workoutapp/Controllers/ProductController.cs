using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using System.Security.Claims;
using workoutapp.DAL;
using workoutapp.Dtos;
using workoutapp.Models;

namespace workoutapp.Controllers
{

    [Route("api/{userId}/calendars/{calendarId}/calendardays/{calendarDayId}/meals/{mealId}/products")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetCategories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = _context.ProductCategories
                .ToList();

            return Ok(categories);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromRoute] int userId, [FromRoute] int calendarId, [FromRoute] int calendarDayId, [FromRoute] int mealId,
            [FromBody] CreateProductDto dto)
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

            var meal = await _context.Meals
            .Include(m => m.CalendarDay)
            .FirstOrDefaultAsync(m => m.MealId == mealId);

            if (meal == null)
            {
                return NotFound();
            }

            if (meal.CalendarDay.CalendarDayId != calendarDayId)
            {
                return Forbid();
            }

            var productCategory = await _context.ProductCategories
                 .FirstOrDefaultAsync(pc => pc.ProductCategoryName == dto.ProductCategoryName);

            if (productCategory == null)
            {
                return BadRequest("Podales zla kategorie");
            }

            var newProduct = _mapper.Map<Product>(dto);

            newProduct.MealId = mealId;
            newProduct.ProductCategoryId = productCategory.ProductCategoryId;

            meal.TotalKcal += newProduct.ProductKcal;


            _context.Products.Add(newProduct);
            _context.SaveChanges();

            var productId = newProduct.ProductId;

            return Created($"/api/{userId}/calendars/{calendarId}/calendardays/{calendarDayId}/meals/{mealId}/products/{productId}", null);

        }

        [HttpDelete("{productId}/delete")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int userId, [FromRoute] int calendarId, [FromRoute] int calendarDayId, [FromRoute] int mealId,
            [FromRoute] int productId)
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
                .Include(m => m.Products)
                .FirstOrDefault(m => m.MealId == mealId);

            if (meal == null)
            {
                return NotFound();
            }

            if (meal.CalendarDayId != calendarDayId)
            {
                return Forbid();
            }

            var product = _context.Products
               .Include(p => p.Meal)
               .FirstOrDefault(m => m.ProductId == productId);

            if (product == null)
            {
                return NotFound();
            }

            if (product.ProductId != productId)
            {
                return Forbid();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return Ok("Usunales Product");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductsMeal([FromRoute] int userId, [FromRoute] int calendarId, [FromRoute] int calendarDayId, [FromRoute] int mealId)
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

            var meal = _context.Meals
                .Include(m => m.CalendarDay)
                .Include(m => m.Products)
                .FirstOrDefault(m => m.MealId == mealId);

            if (meal == null)
            {
                return NotFound();
            }

            if (meal.CalendarDayId != calendarDayId)
            {
                return Forbid();
            }

            var productsDtos = _mapper.Map<List<ProductDto>>(meal.Products);

            return Ok(productsDtos);

        }


        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById([FromRoute] int userId, [FromRoute] int calendarId, [FromRoute] int calendarDayId, [FromRoute] int mealId,
            [FromRoute] int productId)
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

            var product = _context.Products
               .Include(p => p.Meal)
               .FirstOrDefault(m => m.ProductId == productId);

            if (product == null)
            {
                return NotFound();
            }

            if (product.ProductId != productId)
            {
                return Forbid();
            }

            var productDto = _mapper.Map<ProductDto>(product);

            return Ok(productDto);

        }



        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = _context.Products
                .Include(p => p.Meal)
                .ToList();

            return Ok(products);
        }

    }
}
