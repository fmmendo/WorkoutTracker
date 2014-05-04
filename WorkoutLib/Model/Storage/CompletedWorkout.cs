using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutLib.Model.Storage
{
    public class CompletedWorkout
    {
        /// <summary>
        /// Id for the stored workout
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date this workout was completed
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Exercises performed in this workout
        /// </summary>
        public List<CompletedExercise> Exercises { get; set; }
    }
}
