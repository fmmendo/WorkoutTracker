using System;
using System.Collections.ObjectModel;

using WorkoutLib.ViewModel;

namespace WorkoutLib.Model.Storage
{
    public class ExerciseValues
    {
        /// <summary>
        /// Exercise Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// One Rep Max
        /// </summary>
        public double OneRepMaxValue { get; set; }
        
        /// <summary>
        /// Formatted One Rep Max string
        /// </summary>
        public string TxtOneRepMax
        {
            get
            {
                return String.Format("{0} {1}", OneRepMaxValue.ToString("#.##"), Unit.Equals(Utilities.Unit.Imperial) ? "lbs" : "kg");
            }
        }

        /// <summary>
        /// Permutations of One Rep Max
        /// </summary>
        public ObservableCollection<string> Percentages { get; set; }
        
        /// <summary>
        /// Text version of percentages
        /// </summary>
        public string TextPercentages
        {
            get
            {
                return String.Join(";  ", Percentages);
            }
        }

        /// <summary>
        /// Unit in wich the exercise is defined
        /// </summary>
        public WorkoutLib.Utilities.Unit Unit { get; set; }
    }
}
