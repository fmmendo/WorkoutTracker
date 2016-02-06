using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutLib.Model.Storage
{
    public class CompletedExercise
    {
        /// <summary>
        /// Name of completed exercise
        /// </summary>
        public string ExerciseName { get; set; }

        /// <summary>
        /// Number of sets for this exercise
        /// </summary>
        public List<Set> Sets { get; set; }

        /// <summary>
        /// Duration for this exercise (cardio)
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Settings (speed:x, inclination:y, resistance:z, etc...)
        /// </summary>
        public Dictionary<string, int> Settings { get; set; }

        /// <summary>
        /// Whether user has completed exercise successfully 
        /// or missed reps/sets/time/etc.
        /// </summary>
        public bool Successful { get; set; }
    }
}
