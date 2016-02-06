using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutLib.Model
{
    /// <summary>
    /// Plan Settings class used to define how to change the workout as one progresses.
    /// e.g.: Increase - Weight - 5 - lbs(imperial) - every 5 - Week
    /// </summary>
    public class ProgressConfiguration
    {
        /// <summary>
        /// How to progress in the workout (increase, decrease)
        /// </summary>
        public Utilities.PlanProgressChange Progress { get; set; }

        /// <summary>
        /// What's affected when progressing (weight, reps, time)
        /// </summary>
        public Utilities.PlanProgressAffectedSetting AffectedSetting { get; set; }

        /// <summary>
        /// How much we want to progress the affected thing
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Unit for the given amount
        /// </summary>
        public Utilities.PlanProgressUnit Unit { get; set; }

        /// <summary>
        /// Amount of the given time frame
        /// </summary>
        public int TimeAmount { get; set; }

        /// <summary>
        /// How often we want to progress it
        /// </summary>
        public Utilities.PlanProgressAffectedTime TimeFrame { get; set; }

        /// <summary>
        /// Affected exercise (optional)
        /// </summary>
        public string ExerciseName { get; set; }

        /// <summary>
        /// Target exercise
        /// </summary>
        public string TargetExercise { get; set; }

        #region When the user fails during one workout
        /// <summary>
        /// How to progress in the workout (increase, decrease)
        /// </summary>
        public Utilities.PlanProgressChange FailedProgress { get; set; }

        /// <summary>
        /// What's affected when progressing (weight, reps, time)
        /// </summary>
        public Utilities.PlanProgressAffectedSetting FailedAffectedValue { get; set; }

        /// <summary>
        /// How much we want to progress the affected thing
        /// </summary>
        public double FailedAmount { get; set; }

        /// <summary>
        /// Unit for the given amount
        /// </summary>
        public Utilities.PlanProgressUnit FailedUnit { get; set; }
        #endregion

        #region When the user fails for X consecutive workouts
        /// <summary>
        /// Number of times the user needs to file for this setting to trigger
        /// </summary>
        public int ConsecutiveFailCount { get; set; }

        /// <summary>
        /// How to progress in the workout (increase, decrease)
        /// </summary>
        public Utilities.PlanProgressChange ConsecutiveFailProgress { get; set; }

        /// <summary>
        /// What's affected when progressing (weight, reps, time)
        /// </summary>
        public Utilities.PlanProgressAffectedSetting ConsecutiveFailAffectedValue { get; set; }

        /// <summary>
        /// How much we want to progress the affected thing
        /// </summary>
        public double ConsecutiveFailAmount { get; set; }

        /// <summary>
        /// Unit for the given amount
        /// </summary>
        public Utilities.PlanProgressUnit ConsecutiveFailUnit { get; set; }
        #endregion
    }
}
