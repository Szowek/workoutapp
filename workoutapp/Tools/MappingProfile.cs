using AutoMapper;
using workoutapp.Dtos;
using workoutapp.Models;

namespace workoutapp.Tools
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<WorkoutPlan, WorkoutPlanDto>();

            CreateMap<WorkoutPlanDto, WorkoutPlan>();

            CreateMap<CreateWorkoutPlanDto, WorkoutPlan>();

            CreateMap<WorkoutDay, WorkoutDayDto>();

            CreateMap<CreateWorkoutDayDto, WorkoutDay>();

            CreateMap<UserExercise, UserExerciseDto>();

            CreateMap<CreateExerciseDto, UserExercise>();

            CreateMap<Note, NoteDto>();

            CreateMap<CreateNoteDto, Note>();

            CreateMap<Calendar, CalendarDto>();

            CreateMap<CreateCalendarDto, Calendar>();
        }
    }
}
