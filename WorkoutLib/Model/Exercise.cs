using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutLib.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Exercise
    {
        /// <summary>
        /// Name of the exercise
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Exercise order in Workout
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Wether this is a cardio or strength exercise
        /// </summary>
        public Utilities.ExerciseType ExerciseType { get; set; }

        #region Strength Exercise
        /// <summary>
        /// Sets
        /// </summary>
        public List<Set> Sets { get; set; }

        /// <summary>
        /// Rest time in seconds between sets
        /// </summary>
        public int RestTime { get; set; }
        #endregion

        #region Cardio Exercise
        /// <summary>
        /// Duration for the exercise
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Settings (speed:x, inclination:y, resistance:z, etc...)
        /// </summary>
        public Dictionary<string, int> Settings { get; set; }
        #endregion
    }
}
