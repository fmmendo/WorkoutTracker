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
using WorkoutLib.Model.Storage;

namespace WorkoutLib.ViewModel
{
    public class ActivityViewModel : ViewModelBase
    {
        CompletedWorkout workoutLog;
        CompletedExercise exerciseLog;

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
        Exercise Exercise
        {
            get
            {
                return WorkoutService.Service.CurrentExercise;
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
        public string Reps
        {
            get
            {
                return WorkoutService.Service.CurrentExerciseReps;
            }
        }

        private string _buttonText;
        /// <summary>
        /// Text to display on the Next/Done button
        /// </summary>
        public string ButtonText
        {
            get { return _buttonText; }
            set
            {
                _buttonText = value;
                NotifyPropertyChanged();
            }
        }

        public string SetWeight
        {
            get
            {
                var w = Sets.ElementAt(WorkoutService.Service.CurrentSetNumber).Weight;
                if (!String.IsNullOrEmpty(w))
                    return String.Format("{0} {1}", w, UserSettings.Settings.Unit.Equals(WorkoutLib.Utilities.Unit.Imperial) ? "lbs" : "kg");
                else return String.Empty;
            }
        }

        #endregion

        #region Commands
        public ButtonCommand FailedCommand { get; set; }
        public bool CanExecuteFailedCommand(object param)
        {
            return true;
        }
        public void ExecuteFailedCommand(object param)
        {
            if (exerciseLog != null)
                exerciseLog.Successful = false;

            NextExercise();
        }
        #endregion

        public ActivityViewModel()
        {
            WorkoutService.Service.CurrentExerciseNumber = 0;
            WorkoutService.Service.CurrentSetNumber = 0;
            ButtonText = "NEXT";

            FailedCommand = new ButtonCommand(ExecuteFailedCommand, CanExecuteFailedCommand);

            Sets.Clear();
            foreach (var s in Exercise.Sets)
            {
                Sets.Add(new SetViewModel(s)); ;//AddSet(s);
            }

            Sets[WorkoutService.Service.CurrentSetNumber].CurrentSet = true;

            SetUpWorkoutLog();
        }

        //private void AddSet(Set s)
        //{
        //    double w = -1; // if w<0 we ignore it and use whatever came with the json
        //    object o = StorageUtility.ReadSetting(Utilities.ONEREPMAXVALUES);
        //    if (o != null)
        //    {
        //        var Exercise1RMs = (ObservableCollection<ExerciseValues>)o;

        //        // try to find an entry for the current exercise
        //        var item = Exercise1RMs.FirstOrDefault(ex => ex.Name.Equals(ExerciseName));
        //        if (item != null)
        //        {
        //            int index = Exercise1RMs.IndexOf(item);

        //            // calculate lift weight based on stored %RM
        //            w = Exercise1RMs[index].OneRepMaxValue > 0
        //                ? Exercise1RMs[index].OneRepMaxValue * (Int32.Parse(UserSettings.Settings.SelectedPercentage ?? "85") / (double)100)
        //                : -1;

        //            if (Exercise1RMs[index].OneRepMaxValue > 0)
        //            {
        //                double val = Exercise1RMs[index].OneRepMaxValue * (Int32.Parse(UserSettings.Settings.SelectedPercentage ?? "85") / (double)100);
        //                if (Exercise1RMs[index].Unit == UserSettings.Settings.Unit)
        //                    w = Utilities.RoundToNearestKg(val);
        //                else {
        //                    if (Exercise1RMs[index].Unit == Utilities.Unit.Imperial)
        //                        w = Utilities.PoundsToKg(val);
        //                    else
        //                        w = Utilities.KgToPounds(val);
        //                }
        //            }
        //        }
        //    }

        //    Sets.Add(new SetViewModel(s, w));
        //}

        private void SetUpWorkoutLog()
        {
            workoutLog = new CompletedWorkout();
            workoutLog.Date = DateTime.Now;
            workoutLog.Exercises = new List<CompletedExercise>();
            workoutLog.Id = Workout.Id;

            SetUpExerciseLog();
        }

        private void SetUpExerciseLog()
        {
            exerciseLog = new CompletedExercise();
            exerciseLog.ExerciseName = ExerciseName;
            exerciseLog.Sets = new List<Set>(Exercise.Sets);
            exerciseLog.Successful = true;
        }

        public bool NextExercise()
        {
            bool ret = true;
            Sets.ElementAt(WorkoutService.Service.CurrentSetNumber).CurrentSet = false;
            double weight = Double.Parse(Sets.ElementAt(WorkoutService.Service.CurrentSetNumber).Weight);
            //UpdateCurrentExerciseLog(weight, true);

            //If we're not in the last Set yet, go to next set
            if (Sets.Count() > WorkoutService.Service.CurrentSetNumber + 1)
            {
                //UpdateCurrentExerciseLog(weight, true);

                WorkoutService.Service.CurrentSetNumber++;
                ret = true;
            }
            else
            {
                //Else, if we're not in the last Exercise yet, go to next exercise
                if (WorkoutService.Service.CurrentExerciseNumber < Workout.ExerciseList.Count - 1)
                {
                    if (WorkoutService.Service.CurrentExerciseNumber + 1 == Workout.ExerciseList.Count - 1)
                        ButtonText = "DONE";

                    workoutLog.Exercises.Add(exerciseLog);

                    WorkoutService.Service.CurrentExerciseNumber++;
                    WorkoutService.Service.CurrentSetNumber = 0;
                    ret = true;

                    // load sets
                    Sets.Clear();
                    foreach (var s in Exercise.Sets)
                        Sets.Add(new SetViewModel(s));
                    //AddSet(s);

                    SetUpExerciseLog();
                }
                else
                {
                    //no more exercises
                    //UpdateCurrentExerciseLog(weight, true);
                    workoutLog.Exercises.Add(exerciseLog);
                    ret = false;

                    SaveDateIfFirstWorkout();
                    SetNextWorkout();

                    LogCompletedWorkout();
                }
            }

            NotifyPropertyChanged("ExerciseName");
            NotifyPropertyChanged("Reps");
            NotifyPropertyChanged("SetWeight");
            NotifyPropertyChanged("Description");

            Sets.ElementAt(WorkoutService.Service.CurrentSetNumber).CurrentSet = true;
            return ret;
        }

        private void UpdateCurrentExerciseLog(double weight, bool success)
        {
            exerciseLog.ExerciseName = ExerciseName;
            exerciseLog.Sets.Add(new Set
            {
                NumberOfReps = Exercise.Sets[WorkoutService.Service.CurrentSetNumber].NumberOfReps,
                SetType = Exercise.Sets[WorkoutService.Service.CurrentSetNumber].SetType,
                Unit = Exercise.Sets[WorkoutService.Service.CurrentSetNumber].Unit,
                Weight = Exercise.Sets[WorkoutService.Service.CurrentSetNumber].Weight
            });

            exerciseLog.Successful = success;
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

        private void LogCompletedWorkout()
        {
            object o = StorageUtility.ReadSetting(Utilities.PLAN_LOG);

            if (o != null)
            {
                PlanLog log = o as PlanLog;
                if (log != null)
                {
                    log.Workouts.Add(workoutLog);
                    StorageUtility.WriteSetting(Utilities.PLAN_LOG, log);
                }
            }
            else
            {
                PlanLog log = new PlanLog();
                log.Workouts = new List<CompletedWorkout>();
                log.Workouts.Add(workoutLog);
                StorageUtility.WriteSetting(Utilities.PLAN_LOG, log);
            }
        }
    }
}
