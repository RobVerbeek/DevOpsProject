using Contentful.Core.Models;

namespace Case_Study.Models
{
    public class Routine
    {
        public required SystemProperties Sys {  get; set; }
        public required string RoutineName { get; set; }

        public required List<Exercise> Exercises { get; set; }
        public required Asset RoutineImage { get; set; } 


    }
}
