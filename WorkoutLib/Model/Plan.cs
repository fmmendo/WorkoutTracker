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
    public class Plan
    {
        /// <summary>
        /// Identifier for this workout
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Small description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Excercises to Perform
        /// </summary>
        public IEnumerable<Workout> Workouts { get; set; }

        /// <summary>
        /// ID for the current workout in the plan
        /// </summary>
        public int CurrentWorkout { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RootObject
    {
        /// <summary>
        /// 
        /// </summary>
        public Plan Plan { get; set; }
    }
}
