using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WorkoutLib;
using WorkoutLib.Model;

namespace WorkoutLib.ViewModel
{
    public class ActivityViewModel : ViewModelBase
    {
        #region Properties
        /// <summary>
        /// Current Workout
        /// </summary>
        Workout Workout
        {
            get
            {
                return WorkoutService.Service.CurrentWorkout;
            }
        }

        /// <summary>
        /// Current Exercise
        /// </summary>
        StrengthExercise Exercise
        {
            get
            {
                return Workout.StrengthExerciseList.ElementAt(CurrentExerciseNumber);
            }
        }

        private ObservableCollection<SetViewModel> _sets = new ObservableCollection<SetViewModel>();
        /// <summary>
        /// List of all sets for current exercise
        /// </summary>
        public ObservableCollection<SetViewModel> Sets
        {
            get
            {
                return _sets;
            }
        }

        /// <summary>
        /// Name of the exercise
        /// </summary>
        public string ExerciseName { get { return Exercise.Name; } }

        /// <summary>
        /// Description for the exercise
        /// </summary>
        public string Description { get { return Exercise.Description; } }

        /// <summary>
        /// Number of Reps
        /// </summary>
        public string Reps { get { return Exercise.Sets[CurrentSet].NumberOfReps.ToString(); } }

        private string _buttonText;
        /// <summary>
        /// Text to display on the Next/Done button
        /// </summary>
        public string ButtonText
        {
            get { return _buttonText; }
            set { _buttonText = value; NotifyPropertyChanged(); }
        }

        public string SetWeight
        {

            get
            {
                var w = Sets.ElementAt(CurrentSet).Weight;
                if (!String.IsNullOrEmpty(w))
                    return String.Format("{0} {1}", w, UserSettings.Settings.Unit.Equals(WorkoutLib.Utilities.Unit.Imperial) ? "lbs" : "kg");
                else return String.Empty;
            }
        }
        #endregion

        private int CurrentExerciseNumber = 0;
        private int CurrentSet = 0;


        public ActivityViewModel()
        {
            CurrentExerciseNumber = 0;
            CurrentSet = 0;
            ButtonText = "NEXT";

            Sets.Clear();
            foreach (var s in Exercise.Sets)
            {
                double w = -1; // if w<0 we ignore it and use whatever came with the json
                object o = StorageUtility.ReadSetting(Utilities.ONEREPMAXVALUES);
                if (o != null)
                {
                    var Exercise1RMs = (ObservableCollection<ExerciseValues>)o;

                    // try to find an entry for the current exercise
                    var item = Exercise1RMs.FirstOrDefault(ex => ex.Name.Equals(ExerciseName));
                    if (item != null)
                    {
                        int index = Exercise1RMs.IndexOf(item);

                        // calculate lift weight based on stored %RM
                        w = Exercise1RMs[index].OneRepMaxValue > 0 ? Exercise1RMs[index].OneRepMaxValue * (Int32.Parse(UserSettings.Settings.SelectedPercentage ?? "85") / (double)100) : -1;
                    }

                }

                Sets.Add(new SetViewModel(s, w));
            }

            Sets[CurrentSet].CurrentSet = true;
        }

        public bool NextExercise()
        {
            bool ret = true;
            Sets[CurrentSet].CurrentSet = false;
            if (Sets.Count() > CurrentSet + 1)
            {
                CurrentSet++;
                ret = true;
            }
            else
            {
                CurrentSet = 0;
                if (Workout.StrengthExerciseList.Count > CurrentExerciseNumber + 1)
                {
                    CurrentExerciseNumber++;
                    ret = true;

                    Sets.Clear();
                    foreach (var s in Exercise.Sets)
                        Sets.Add(new SetViewModel(s));
                }
                else
                {

                    ret = false; //no more exercises

                    SaveDateIfFirstWorkout();
                    SetNextWorkout();
                    SetNumberOfCompletedWorkouts();
                }
            }

            NotifyPropertyChanged("ExerciseName");
            NotifyPropertyChanged("Reps");
            NotifyPropertyChanged("Description");

            Sets[CurrentSet].CurrentSet = true;
            return ret;
        }

        private static void SetNextWorkout()
        {
            var maxId = WorkoutService.Service.Plan.Workouts.Max(w => w.Id);
            if (WorkoutService.Service.Plan.CurrentWorkout + 1 < maxId)
                StorageUtility.WriteSetting(Utilities.NEXTWORKOUT_SETTING, WorkoutService.Service.Plan.CurrentWorkout + 1);
            else
                StorageUtility.WriteSetting(Utilities.NEXTWORKOUT_SETTING, 0);
        }

        private static void SaveDateIfFirstWorkout()
        {
            object o = StorageUtility.ReadSetting(Utilities.PLAN_START_DATE);

            if (o == null) //no date stored
                StorageUtility.WriteSetting(Utilities.PLAN_START_DATE, DateTime.Today.ToShortDateString());
        }

        private static void SetNumberOfCompletedWorkouts()
        {
            object o = StorageUtility.ReadSetting(Utilities.NUMBER_COMPLETED_WORKOUTS);

            if (o != null)
                StorageUtility.WriteSetting(Utilities.NUMBER_COMPLETED_WORKOUTS, ((int)o) + 1);
            else
                StorageUtility.WriteSetting(Utilities.NUMBER_COMPLETED_WORKOUTS, 1);
        }
    }
}
