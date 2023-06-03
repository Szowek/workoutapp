﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using workoutapp.DAL;
using workoutapp.Models;

namespace workoutapp.Controllers
{

    [Route("api/workoutplans")]
    [ApiController]
    public class WorkoutPlansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkoutPlansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Metoda tworzenia WorkoutPlanu dla danego użytkownika
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateWorkoutPlan([FromBody]WorkoutPlanDto workoutPlan)
        {
            int userID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));
            var user = _context.Users.FirstOrDefault(u => u.UserId == userID);

            if (userID == 0) 
            {
                return BadRequest();
            }

            var newWorkoutPlan = new WorkoutPlan
            {
                UserId = userID
            };

            _context.WorkoutPlans.Add(newWorkoutPlan);
            user.WorkoutPlans.Add(newWorkoutPlan);
            _context.SaveChanges();
            /*
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {

                workoutPlan.UserId = user.UserId; // Przypisanie klucza obcego
                user.WorkoutPlans.Add(workoutPlan);
                _context.SaveChanges();

                var id = workoutPlan.WorkoutPlanId;

                return Created($"/api/{userId}/workoutplans/{id}", null);
                
                
            }
            else
            {
                return NotFound();
            }
            */
            return Ok("Stworzyles WorkoutPlan");
           

        }

        // Metoda usuwania WorkoutPlanu dla danego użytkownika
        [HttpDelete("{workoutPlanId}")]
        public async Task<IActionResult> DeleteWorkoutPlan(int userId, [FromRoute] int workoutPlanId)
        {
            var user = _context.Users.Include(u => u.WorkoutPlans).FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {
                var workoutPlan = user.WorkoutPlans.FirstOrDefault(wp => wp.WorkoutPlanId == workoutPlanId);
                if (workoutPlan != null)
                {
                    _context.WorkoutPlans.Remove(workoutPlan);
                    _context.SaveChanges();
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }


        // Metoda zwracająca wszystkie WorkoutPlany użytkownika z listy
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllWorkoutPlans()
        {
            int userID = Convert.ToInt32(HttpContext.User.FindFirstValue("UserId"));

            if (userID == 0)
            {
                return BadRequest();
            }

            var workoutPlans = _context.WorkoutPlans.ToList();

            return Ok(workoutPlans);
            /*
            var user = _context.Users.Include(u => u.WorkoutPlans).FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {
                var workoutPlans = user.WorkoutPlans.ToList();
                return Ok(workoutPlans);
            }
            else
            {
                return NotFound();
            }
            */
        }

        // Metoda zwracająca WorkoutPlan użytkownika na podstawie id
        [HttpGet("{workoutPlanId}")]
        public async Task<IActionResult> GetWorkoutPlanById(int userId, [FromRoute] int workoutPlanId)
        {
            var user = _context.Users.Include(u => u.WorkoutPlans).FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {
                var workoutPlan = user.WorkoutPlans.FirstOrDefault(wp => wp.WorkoutPlanId == workoutPlanId);
                if (workoutPlan != null)
                {
                    return Ok(workoutPlan);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

    }

}