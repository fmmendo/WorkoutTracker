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
    public class Set
    {
        /// <summary>
        /// Number of Reps in this set
        /// </summary>
        public int NumberOfReps { get; set; }

        /// <summary>
        /// Weight to use for each rep
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// Unit in which the current weight is presented
        /// </summary>
        public WorkoutLib.Utilities.Unit Unit { get; set; }

        /// <summary>
        /// Type of set
        /// </summary>
        public WorkoutLib.Utilities.SetType SetType { get; set; }
    }
}
