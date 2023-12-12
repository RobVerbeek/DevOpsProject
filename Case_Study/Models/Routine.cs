using Contentful.Core.Models;

namespace Case_Study.Models
{
    public class Routine
    {
        public SystemProperties Sys {  get; set; }
        public required string RoutineName { get; set; }

        public List<Exercise>? Exercises { get; set; }
        public Asset? RoutineImage { get; set; } 


    }
}
