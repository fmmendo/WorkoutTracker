﻿namespace WorkoutLib.Model
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
        public double Weight { get; set; }

        /// <summary>
        /// Unit in which the current weight is presented
        /// </summary>
        public Utilities.Unit Unit { get; set; }

        /// <summary>
        /// Type of set
        /// </summary>
        public Utilities.SetType SetType { get; set; }
    }
}
