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
    [Route("api/{userId}/workoutplans/{workoutPlanId}/notes")]
    [ApiController]
    [Authorize]
    public class NoteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public NoteController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNote([FromRoute] int userId, [FromRoute] int workoutPlanId, [FromBody] CreateNoteDto dto)
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


            var workoutPlan = await _context.WorkoutPlans
            .Include(wp => wp.User)
            .FirstOrDefaultAsync(wp => wp.WorkoutPlanId == workoutPlanId);

            if (workoutPlan == null)
            {
                return NotFound();
            }

            if (workoutPlan.User.UserId != userId)
            {
                return Forbid();  // Użytkownik nie ma uprawnień do tworzenia WorkoutDay dla WorkoutPlan innego użytkownika
            }

            var newNote = _mapper.Map<Note>(dto);
            newNote.WorkoutPlanId = workoutPlanId;

            _context.Notes.Add(newNote);
            _context.SaveChanges();

            var noteId = newNote.NoteId;
            //return Ok("Stworzyles WorkoutDay");
            return Created($"api/{userId}/workoutplans/{workoutPlanId}/notes/{noteId}", null);

        }
        
        [HttpDelete("{noteId}/delete")]
        public async Task<IActionResult> DeleteNote([FromRoute] int userId, [FromRoute] int workoutPlanId, [FromRoute] int noteId)
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


            var workoutPlan = await _context.WorkoutPlans
           .Include(wp => wp.User)
           .Include(wp => wp.Notes)
           .FirstOrDefaultAsync(wp => wp.WorkoutPlanId == workoutPlanId);


            if (workoutPlan == null)
            {
                return NotFound();
            }

            if (workoutPlan.User.UserId != userId)
            {
                return Forbid();
            }

            var note = workoutPlan.Notes.FirstOrDefault(n => n.NoteId == noteId);
            if (note == null)
            {
                return NotFound();
            }

            if (note.WorkoutPlanId != workoutPlanId)
            {
                return Forbid();
            }


            _context.Notes.Remove(note);
            _context.SaveChanges();

            return Ok("Usunales notatke");

        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotesWP([FromRoute] int userId, [FromRoute] int workoutPlanId)
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

            var workoutPlan = await _context.WorkoutPlans
            .Include(wp => wp.User)
            .Include(wp => wp.Notes)
            .FirstOrDefaultAsync(wp => wp.WorkoutPlanId == workoutPlanId);

            if (workoutPlan == null)
            {
                return NotFound();
            }

            if (workoutPlan.User.UserId != userId)
            {
                return Forbid();
            }


            var notesDtos = _mapper.Map<List<NoteDto>>(workoutPlan.Notes);

            return Ok(notesDtos);

        }


        [HttpGet("{noteId}")]
        public async Task<IActionResult> GetNoteById([FromRoute] int userId, [FromRoute] int workoutPlanId, [FromRoute] int noteId)
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

            var workoutPlan = await _context.WorkoutPlans
           .Include(wp => wp.User)
           .Include(wp => wp.Notes)
           .FirstOrDefaultAsync(wp => wp.WorkoutPlanId == workoutPlanId);


            if (workoutPlan == null)
            {
                return NotFound();
            }

            if (workoutPlan.User.UserId != userId)
            {
                return Forbid();
            }


            var note = workoutPlan.Notes.FirstOrDefault(n => n.NoteId == noteId);

            if (note == null)
            {
                return NotFound();
            }

            if (note.WorkoutPlanId != workoutPlanId)
            {
                return Forbid();
            }

            var noteDto = _mapper.Map<NoteDto>(note);

            return Ok(noteDto);
        }


        [HttpPut("{noteId}/edit")]
        public async Task<IActionResult> EditNote([FromRoute] int userId, [FromRoute] int workoutPlanId, [FromRoute] int noteId, [FromBody] UpdateNoteDto dto)
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

            var workoutPlan = await _context.WorkoutPlans
            .Include(wp => wp.User)
            .Include(wp => wp.Notes)
            .FirstOrDefaultAsync(wp => wp.WorkoutPlanId == workoutPlanId);

            if (workoutPlan == null)
            {
                return NotFound();
            }

            if (workoutPlan.UserId != userId)
            {
                return Forbid();
            }

            var note = workoutPlan.Notes
                .FirstOrDefault(n => n.NoteId == noteId);

            if (note == null)
            {
                return NotFound();
            }

            if (note.WorkoutPlanId != workoutPlanId)
            {
                return Forbid();
            }


            note.NoteName = dto.NoteName;
         
            note.Text = dto.Text;
         
            _context.SaveChanges();
            return Ok();
        }


        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllNotes()
        {
            var notes = _context.Notes
                .Include(wp => wp.WorkoutPlan)
                .ToList();

            return Ok(notes);

        }

    }
}
