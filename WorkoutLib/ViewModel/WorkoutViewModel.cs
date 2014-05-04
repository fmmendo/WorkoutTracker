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
                return WorkoutService.Service.CurrentWorkout.Name;
            }
        }

        /// <summary>
        /// Description for the current workout
        /// </summary>
        public string Description
        {
            get
            {
                return WorkoutService.Service.CurrentWorkout.Description;
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
                    WorkoutService.Service.LoadPlan(Utilities.LoadFileAsString("Assets\\StrongLifts.json"));
            }

            foreach (var e in WorkoutService.Service.CurrentWorkout.ExerciseList.OrderBy(o => o.Order))
            {
                ExerciseList.Add(new ExerciseViewModel(e));
            }
        }
    }
}
