using System.Collections.ObjectModel;
using WorkoutLib.Model;

namespace WorkoutLib.ViewModel
{
    public class ExerciseViewModel : ViewModelBase
    {
        private Exercise _exercise;

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return _exercise.Name; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Order
        {
            get { return _exercise.Order; }
        }

        /// <summary>
        /// Collection of viewmodels to display the Sets
        /// </summary>
        public ObservableCollection<SetViewModel> Sets { get; set; }

        public ExerciseViewModel(Exercise e)
        {
            _exercise = e;
            //double w = GetOneRepMaxWeight();

            Sets = new ObservableCollection<SetViewModel>();
            foreach (var s in _exercise.Sets)
            {
                Sets.Add(new SetViewModel(s,-1));
            }
        }

        /// <summary>
        /// Reads the one rep max values from storage and returns the weight 
        /// for the current exercise if it exists, otherwise returns -1
        /// </summary>
        /// <returns></returns>
        //private double GetOneRepMaxWeight()
        //{
        //    // Read the setting from isolated storage
        //    double w = -1; // if w<0 we ignore it and use whatever came with the json
        //    object o = StorageUtility.ReadSetting(Utilities.ONEREPMAXVALUES);
        //    if (o != null)
        //    {
        //        var Exercise1RMs = (ObservableCollection<ExerciseValues>)o;

        //        // try to find an entry for the current exercise
        //        var item = Exercise1RMs.FirstOrDefault(ex => ex.Name.Equals(Name));
        //        if (item != null)
        //        {
        //            int index = Exercise1RMs.IndexOf(item);

        //            // calculate lift weight based on stored %RM
        //            if (Exercise1RMs[index].OneRepMaxValue > 0)
        //            {
        //                // if it's imperial but we want metric
        //                if (Exercise1RMs[index].Unit.Equals(WorkoutLib.Utilities.Unit.Imperial) && UserSettings.Settings.Unit.Equals(WorkoutLib.Utilities.Unit.Metric))
        //                    w = Utilities.PoundsToKg(Exercise1RMs[index].OneRepMaxValue);
        //                // if it's metric but we want imperial
        //                else if (Exercise1RMs[index].Unit.Equals(WorkoutLib.Utilities.Unit.Metric) && UserSettings.Settings.Unit.Equals(WorkoutLib.Utilities.Unit.Imperial))
        //                    w = Utilities.KgToPounds(Exercise1RMs[index].OneRepMaxValue);
        //                else
        //                    w = Exercise1RMs[index].OneRepMaxValue;

        //                w = w * (Int32.Parse(UserSettings.Settings.SelectedPercentage ?? "85") / (double)100);
        //            }
        //        }
        //    }
        //    return w;
        //}
    }
}
