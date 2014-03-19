using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WorkoutLib.Model;

namespace WorkoutLib.ViewModel
{
    public class ExerciseViewModel : ViewModelBase
    {
        private StrengthExercise _exercise;

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

        public ExerciseViewModel(StrengthExercise e)
        {
            _exercise = e;
            
            // Read the setting from isolated storage
            double w = -1; // if w<0 we ignore it and use whatever came with the json
            object o = StorageUtility.ReadSetting(Utilities.ONEREPMAXVALUES);
            if (o != null)
            {
                var Exercise1RMs = (ObservableCollection<ExerciseValues>)o;

                // try to find an entry for the current exercise
                var item = Exercise1RMs.FirstOrDefault(ex => ex.Name.Equals(Name));
                if (item != null)
                {
                    int index = Exercise1RMs.IndexOf(item);

                    // calculate lift weight based on stored %RM
                    w = Exercise1RMs[index].OneRepMaxValue > 0 ? Exercise1RMs[index].OneRepMaxValue * (Int32.Parse(UserSettings.Settings.SelectedPercentage ?? "85")/(double)100) : -1;
                }

            }

            Sets = new ObservableCollection<SetViewModel>();
            foreach (var s in _exercise.Sets)
            {
                Sets.Add(new SetViewModel(s, w));
            }
        }
    }
}
