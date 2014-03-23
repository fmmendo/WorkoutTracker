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
    public class PlanProgressConfiguration
    {
        /// <summary>
        /// How to progress in the workout (increase, decrease)
        /// </summary>
        public Utilities.PlanProgressChange Progress { get; set; }

        /// <summary>
        /// What's affected when progressing (weight, reps, time)
        /// </summary>
        public Utilities.PlanProgressAffectedValue AffectedValue { get; set; }

        /// <summary>
        /// How much we want to progress the affected thing
        /// </summary>
        public int Amount { get; set; }

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
    }
}
