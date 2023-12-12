using Case_Study.Models;
using Contentful.Core;
using Contentful.Core.Configuration;
using Contentful.Core.Errors;
using Contentful.Core.Extensions;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


// System.Diagnostics.Debug.WriteLine use for debugging
namespace Case_Study.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContentfulClient _client;
        private readonly IContentfulManagementClient _managementClient;

        public HomeController(ILogger<HomeController> logger, IContentfulClient client, IContentfulManagementClient managementClient)
        {
            _client = client;
            _logger = logger;
            _managementClient = managementClient;
        }


        // List of all routines and corresponding exercises
        public async Task<IActionResult> Index()
        {
            var query = QueryBuilder<Routine>.New.Include(1);
            var routines = await _client.GetEntriesByType("exerciseRoutine", query);

            return View(routines);
        }

        // List of all existing exercises
        public async Task<IActionResult> ExerciseList() 
        {
            var Exercises = await _client.GetEntriesByType<Exercise>("exercise");

            return View(Exercises);        
        }

        // controller for editing an existing exercise

        public IActionResult EditExercise()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditExercise(Exercise exercise)
        {
       
            var EditingExercise = await _client.GetEntry<Exercise>(exercise.Sys.Id);
            System.Diagnostics.Debug.WriteLine(exercise.Sys.Version, "version or something like that");
            var shit = new UpdateExercise();
            shit.ExerciseName = EditingExercise.ExerciseName;
            shit.Weight = EditingExercise.Weight;
            shit.exerciseId = EditingExercise.Sys.Id;
            shit.Sets = EditingExercise.Sets;
            shit.Repetitions = EditingExercise.Repetitions;
            return View(shit);
        }

        [HttpPost]
        public async Task<IActionResult> SaveExerciseChanges(UpdateExercise exercise)
        {
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }

            var thing = await _managementClient.GetEntry(exercise.exerciseId);
            System.Diagnostics.Debug.WriteLine(thing.ConvertObjectToJsonString());
            System.Diagnostics.Debug.WriteLine(thing.SystemProperties.Version, "version");
            thing.Fields.weight["en-US"] = exercise.Weight;
            thing.Fields.sets["en-US"] = exercise.Sets;
            thing.Fields.repetitions["en-US"] = exercise.Repetitions;
            System.Diagnostics.Debug.WriteLine(thing.ConvertObjectToJsonString());
            var version = thing.SystemProperties.Version + 1;

            var ChangedExercise = await _managementClient.CreateOrUpdateEntry(thing, "xw17w70tomkg", "exercise", thing.SystemProperties.Version);
            await _managementClient.PublishEntry(ChangedExercise.SystemProperties.Id,(int)version);
 
            var Exercises = await _client.GetEntriesByType<Exercise>("exercise");
            return View("ExerciseList", Exercises);
        }


        // Controller for creating and uploading new exercises
        [HttpGet]
        public IActionResult NewExercise()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NewExercise([Bind("ExerciseName, Repetitions, Weight, Sets")] ViewExercise exercise) {
            if (ModelState.IsValid)
            {
                var entry = new Entry<dynamic>
                {
                    Fields = new
                    {
                        exerciseName = new Dictionary<string, string> { { "en-US", exercise.ExerciseName } },
                        repetitions = new Dictionary<string, int> { { "en-US", exercise.Repetitions } },
                        weight = new Dictionary<string, int> { { "en-US", exercise.Weight } },
                        sets = new Dictionary<string, int> { { "en-US", exercise.Sets } },
                    }
                };

                var NewExercise = await _managementClient.CreateEntry(entry, "exercise");
                await _managementClient.PublishEntry(NewExercise.SystemProperties.Id,1);

                return RedirectToAction("ExerciseList");              
            }
            return View("NewExercise", exercise);
        }


        // Controller for creating and uploading new routines.

        public async Task<IActionResult> NewRoutine()
        {            
            return View(); 
        }
      


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

