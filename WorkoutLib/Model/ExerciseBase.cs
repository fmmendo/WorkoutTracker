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
    public class ExerciseBase
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
    }
}
