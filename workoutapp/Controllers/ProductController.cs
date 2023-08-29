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

        [HttpPost("create/template")]
        public async Task<IActionResult> CreateTemplateProduct([FromRoute] int userId, [FromRoute] int calendarId, [FromRoute] int calendarDayId, [FromRoute] int mealId,
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

            var templateMealExample = _context
                .Products
                .Where(p => p.ProductWeight == 0)
                .FirstOrDefault();

            var templateMealId = templateMealExample.MealId;

            newProduct.ProductCategoryId = productCategory.ProductCategoryId;
            newProduct.MealId = templateMealId;

            _context.Products.Add(newProduct);
            _context.SaveChanges();

            return Ok(newProduct);
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

            meal.TotalKcal += (int) (newProduct.ProductWeight * (newProduct.ProductKcal/100.0));
            meal.TotalFat += newProduct.ProductWeight * (newProduct.ProductFat / 100);
            meal.TotalCarbs += newProduct.ProductWeight * (newProduct.ProductCarbs / 100);
            meal.TotalProtein += newProduct.ProductWeight * (newProduct.ProductProtein / 100);


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

            if (product.MealId != mealId)
            {
                return Forbid();
            }


            meal.TotalKcal -= (int)(product.ProductWeight * (product.ProductKcal / 100.0));
            meal.TotalFat -= product.ProductWeight * (product.ProductFat / 100);
            meal.TotalCarbs -= product.ProductWeight * (product.ProductCarbs / 100);
            meal.TotalProtein -= product.ProductWeight * (product.ProductProtein / 100);
            _context.Products.Remove(product);
            _context.SaveChanges();

            return Ok("Usunales Product");
        }

        [HttpPut("{productId}/edit")]
        public async Task<IActionResult> EditProduct([FromRoute] int userId, [FromRoute] int calendarId, [FromRoute] int calendarDayId, [FromRoute] int mealId,
    [FromRoute] int productId, [FromBody] CreateProductDto dto)
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

            if (product.MealId != mealId)
            {
                return Forbid();
            }
            meal.TotalKcal -= (int)(product.ProductWeight * (product.ProductKcal / 100.0));
            meal.TotalFat -= product.ProductWeight * (product.ProductFat / 100);
            meal.TotalCarbs -= product.ProductWeight * (product.ProductCarbs / 100);
            meal.TotalProtein -= product.ProductWeight * (product.ProductProtein / 100);
            product.ProductWeight = dto.ProductWeight;
            meal.TotalKcal += (int)(product.ProductWeight * (product.ProductKcal / 100.0));
            meal.TotalFat += product.ProductWeight * (product.ProductFat / 100);
            meal.TotalCarbs += product.ProductWeight * (product.ProductCarbs / 100);
            meal.TotalProtein += product.ProductWeight * (product.ProductProtein / 100);

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


            var product = _context.Products
               .Include(p => p.Meal)
               .FirstOrDefault(m => m.ProductId == productId);

            if (product == null)
            {
                return NotFound();
            }


            var productDto = _mapper.Map<ProductDto>(product);

            return Ok(productDto);

        }

        [HttpGet("getAllByCategory/{categoryId}")]
        public async Task<IActionResult> getAllByCategory([FromRoute] int userId, [FromRoute] int calendarId, [FromRoute] int calendarDayId, 
            [FromRoute] int mealId, [FromRoute] int categoryId)
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

            var products = _context
                .Products
                .Where(p => p.ProductCategoryId == categoryId && p.ProductWeight == 0)
                .ToList();

            products = products.DistinctBy(p => p.ProductName).ToList();

            if (products.Count > 0)
            {
                return Ok(products);
            }
            else
            {
                return NotFound();
            }
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
