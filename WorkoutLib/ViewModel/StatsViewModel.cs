using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutLib.Model.Storage;

namespace WorkoutLib.ViewModel
{
    public class StatsViewModel : ViewModelBase
    {
        public class StatItem
        {
            public string Date { get; set; }
            public double Weight { get; set; }
        }

        private Dictionary<string, List<StatItem>> _exerciseProgression = new Dictionary<string, List<StatItem>>();

        public Dictionary<string, List<StatItem>> ExerciseProgression
        {
            get { return _exerciseProgression; }
            set { _exerciseProgression = value; }
        }

        public IEnumerable<List<StatItem>> Items
        {
            get
            {
                foreach (var item in ExerciseProgression)
                {
                    yield return item.Value;
                }
            }
        }


        public StatsViewModel()
        {
            object o = StorageUtility.ReadSetting(Utilities.PLAN_LOG);
            if (o == null || !(o is PlanLog)) return;

            var workouts = (o as PlanLog).Workouts;

            foreach (var workout in workouts)
            {
                foreach (var exercise in workout.Exercises)
                {
                    if (ExerciseProgression.ContainsKey(exercise.ExerciseName))
                    {
                        ExerciseProgression[exercise.ExerciseName].Add(new StatItem { Date = workout.Date.ToString("dd/MMM"), Weight = exercise.Sets.Max(s => s.Weight) });
                    }
                    else
                    {
                        var exerciselist = new List<StatItem>();
                        exerciselist.Add(new StatItem { Date = workout.Date.ToString("dd/MMM"), Weight = exercise.Sets.Max(s => s.Weight) });
                        ExerciseProgression.Add(exercise.ExerciseName, exerciselist);
                    }
                }
            }
        }
    }
}
