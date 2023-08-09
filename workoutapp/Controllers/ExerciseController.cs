using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using workoutapp.DAL;

namespace workoutapp.Controllers
{
    public class ExerciseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ExerciseController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
