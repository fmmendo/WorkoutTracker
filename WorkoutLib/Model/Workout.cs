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
    public class Workout
    {
        public int Id { get; set; }

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
        public List<Exercise> ExerciseList { get; set; }
    }
}
