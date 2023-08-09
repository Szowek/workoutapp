using Microsoft.EntityFrameworkCore;
using workoutapp.DAL;
using workoutapp.Migrations;

namespace workoutapp.Seed
{
    public class ExerciseSeeder
    {
        private readonly ApplicationDbContext _context;

        public ExerciseSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            //sprawdzenie polaczenia do bazy danych:
            if (_context.Database.CanConnect())
            {


            }
        }


    }

}
