using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutLib;
using WorkoutLib.Model;

namespace WorkoutLib.ViewModel
{
    public class WorkoutViewModel : ViewModelBase
    {
        /// <summary>
        /// CUrrent Plan
        /// </summary>
        public Plan Plan
        {
            get
            {
                return WorkoutService.Service.Plan;
            }
        }

        /// <summary>
        /// Current Workout
        /// </summary>
        public string WorkoutName
        {
            get
            {
                return Plan.Workouts.ElementAt(Plan.CurrentWorkout).Name;
            }
        }

        /// <summary>
        /// List of exercise viewmodels for the ExerciseControl
        /// </summary>
        public ObservableCollection<ExerciseViewModel> ExerciseList { get; set; }

        public WorkoutViewModel()
        {
            ExerciseList = new ObservableCollection<ExerciseViewModel>();

            object workoutID = StorageUtility.ReadSetting(Utilities.NEXTWORKOUT_SETTING);
            if (workoutID != null)
            {
                if (Plan.CurrentWorkout != (int)workoutID)
                    WorkoutService.Service.LoadPlan(Utilities.LoadJsonFromFile("Assets\\StrongLifts.json"));
            }
            

            foreach (var e in Plan.Workouts.ElementAt(Plan.CurrentWorkout).StrengthExerciseList.OrderBy(o => o.Order))
            {
                ExerciseList.Add(new ExerciseViewModel(e));
            }
        }
    }
}
