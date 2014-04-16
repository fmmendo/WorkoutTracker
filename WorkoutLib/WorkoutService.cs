using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutLib.Model;

namespace WorkoutLib
{
    public class WorkoutService
    {
        #region Singleton
        private static WorkoutService _service;
        /// <summary>
        /// Singleton Instance
        /// </summary>
        public static WorkoutService Service
        {
            get
            {
                if (_service == null)
                    _service = new WorkoutService();
                return _service; 
            }
        }
        #endregion

        private Plan _plan;
        /// <summary>
        /// The current plan
        /// </summary>
        public Plan Plan
        {
            get { return _plan; }
            set { _plan = value; }
        }

        /// <summary>
        /// Gets the current workout
        /// </summary>
        public Workout CurrentWorkout
        {
            get
            {
                if (Plan == null || Plan.Workouts == null || Plan.Workouts.Count() <= 0)
                    return null;

                return Plan.Workouts.ElementAt(Plan.CurrentWorkout);
            }
        }

        /// <summary>
        /// Exercise list
        /// </summary>
        public IEnumerable<string> Exercises
        {
            get
            {
                List<string> names = new List<string>();

                if (WorkoutService.Service.Plan == null)
                    return names;

                foreach (var w in WorkoutService.Service.Plan.Workouts)
                {
                    foreach (var e in w.StrengthExerciseList)
                        if (!names.Contains(e.Name))
                            names.Add(e.Name);
                }

                return names;
            }
        }

        private WorkoutService()
        {
            _plan = null;
        }

        public void LoadPlan(string json)
        {
            _plan = DeserializePlan(json);

            object workoutID = StorageUtility.ReadSetting(Utilities.NEXTWORKOUT_SETTING);
            if (workoutID == null)
            {
                _plan.CurrentWorkout = 0;
                StorageUtility.WriteSetting(Utilities.NEXTWORKOUT_SETTING, 0);
            }
            else
                _plan.CurrentWorkout = (int)workoutID;
        }

        public void ClearPlan()
        {
            _plan = null;
        }

        public static Plan DeserializePlan(string json)
        {
            if (String.IsNullOrEmpty(json))
                return new Plan();

            json = json.Replace("\n", "").Replace("\r", "");

            RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(json);
            rootObject.Plan.CurrentWorkout = 1;
            return rootObject.Plan;
        }

        public static string SerializePlan(Plan plan)
        {
            var rootoject = new RootObject() { Plan = plan };
            string json = JsonConvert.SerializeObject(rootoject);
            return json;
        }
    }
}
