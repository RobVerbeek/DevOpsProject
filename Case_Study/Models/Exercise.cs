using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Models.Management;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Case_Study.Models
{

    public class Exercise
    {
        public required SystemProperties Sys { get; set; }
        public string? ExerciseName{ get; set; }
        public int Repetitions { get; set; }
        public int Weight { get; set; }
        public int Sets { get; set; }

    }
    public class ViewExercise
    {
        public string? ExerciseName { get; set; }
        public int Repetitions { get; set; }
        public int Weight { get; set; }
        public int Sets { get; set; }
    }
    public class UpdateExercise
    {
        public string? exerciseId { get; set; }
        public string? ExerciseName { get; set; }
        public int Repetitions { get; set; }
        public int Weight { get; set; }
        public int Sets { get; set; }
    }
}
