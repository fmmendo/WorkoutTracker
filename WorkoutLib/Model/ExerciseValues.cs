using System;
using System.Collections.ObjectModel;

using WorkoutLib.ViewModel;

namespace WorkoutLib.Model
{
    public class ExerciseValues
    {
        public string Name { get; set; }
        public double OneRepMaxValue { get; set; }
        public string TxtOneRepMax
        {
            get
            {
                return String.Format("{0} {1}", OneRepMaxValue.ToString("#.##"),
                                                UserSettings.Settings.Unit.Equals(Utilities.Unit.Imperial) ? "lbs" : "kg");
            }
        }
        public ObservableCollection<string> Percentages { get; set; }
        public string TextPercentages
        {
            get
            {
                return String.Join(";  ", Percentages);
            }
        }
    }
}
