using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WorkoutLib.Model;
using WorkoutLib.Model.Storage;
using WorkoutLib.ViewModel;

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
        /// Gets the current Exercise
        /// </summary>
        public Exercise CurrentExercise
        {
            get
            {
                if (CurrentWorkout == null)
                    return null;

                return CurrentWorkout.ExerciseList.ElementAt(CurrentExerciseNumber);
            }
        }

        /// <summary>
        /// Gets the number of reps for the current exercise
        /// </summary>
        public string CurrentExerciseReps
        {
            get
            {
                if (CurrentExercise == null || CurrentExercise.Sets == null || CurrentExercise.Sets.Count() <= 0)
                    return String.Empty;

                return CurrentExercise.Sets[CurrentSetNumber].NumberOfReps.ToString();
            }
        }

        /// <summary>
        /// Exercise list
        /// </summary>
        public IEnumerable<string> ExerciseNames
        {
            get
            {
                List<string> names = new List<string>();

                if (WorkoutService.Service.Plan == null)
                    return names;

                foreach (var w in WorkoutService.Service.Plan.Workouts)
                {
                    foreach (var e in w.ExerciseList)
                        if (!names.Contains(e.Name))
                            names.Add(e.Name);
                }

                return names;
            }
        }

        private int _currentExerciseNumber = 0;
        /// <summary>
        /// Current Exercise Number
        /// </summary>
        public int CurrentExerciseNumber
        {
            get { return _currentExerciseNumber; }
            set { _currentExerciseNumber = value; }
        }

        private int _currentSetNumber = 0;
        /// <summary>
        /// Current Set Number
        /// </summary>
        public int CurrentSetNumber
        {
            get { return _currentSetNumber; }
            set { _currentSetNumber = value; }
        }

        private WorkoutService()
        {
            Plan = null;
        }

        /// <summary>
        /// Loads the plan
        /// </summary>
        /// <param name="json">JSON encoded plan</param>
        public void LoadPlan(string json)
        {
            Plan = Utilities.DeserializePlan(json);

            bool canProgress;
            object progress = StorageUtility.ReadSetting(Utilities.PLANPROGRESS_SETTING);
            if (progress != null && progress is bool)
                canProgress = (bool)progress;
            else canProgress = true; //default

            // Retrieve ID for today's workout
            object workoutID = StorageUtility.ReadSetting(Utilities.NEXTWORKOUT_SETTING);
            if (workoutID == null)
            {
                Plan.CurrentWorkout = 0;
                StorageUtility.WriteSetting(Utilities.NEXTWORKOUT_SETTING, 0);
            }
            else
                Plan.CurrentWorkout = (int)workoutID;

            if (Plan.ProgressConfiguration != null && canProgress)
            {
                RefreshPlan();
            }
        }

        public void RefreshPlan()
        {
            //UpdateSets();
            //UpdateReps();
            //UpdateTimes();
            UpdateWeights();
        }

        /// <summary>
        /// Clears the plan
        /// </summary>
        public void ClearPlan()
        {
            _plan = null;
        }

        #region Update Plan (increments)
        /// <summary>
        /// 
        /// </summary>
        private void UpdateWeights()
        {
            try
            {
                object o = StorageUtility.ReadSetting(Utilities.PLAN_LOG);
                //if (o == null || !(o is PlanLog))
                {
                        object orm = StorageUtility.ReadSetting(Utilities.ONEREPMAXVALUES);
                    foreach (var e in Plan.Workouts.ElementAt(Plan.CurrentWorkout).ExerciseList)
                    {
                        //double w = -1; // if w<0 we ignore it and use whatever came with the json
                        if (orm != null)
                        {
                            var Exercise1RMs = (ObservableCollection<ExerciseValues>)orm;

                            // try to find an entry for the current exercise
                            var item = Exercise1RMs.FirstOrDefault(ex => ex.Name.Equals(e.Name));
                            if (item != null)
                            {
                                int index = Exercise1RMs.IndexOf(item);

                                // calculate lift weight based on stored %RM
                                var w = Exercise1RMs[index].OneRepMaxValue > 0 ? Exercise1RMs[index].OneRepMaxValue * (int.Parse(UserSettings.Settings.SelectedPercentage ?? "85") / (double)100) : -1;

                                e.Sets.ForEach(s => { s.Weight = w; s.Unit = item.Unit; });
                            }
                        }
                    }
                    //return;
                }

                var log = o as PlanLog;
                if (log == null || log.Workouts.Count <= 0)
                    return;

                foreach (var config in Plan.ProgressConfiguration.Where(p => p.AffectedSetting.Equals(Utilities.PlanProgressAffectedSetting.Weight)))
                {
                    if (CurrentWorkout.ExerciseList.Any(e => e.Name.Equals(config.ExerciseName)))
                    {
                        // take last X workouts
                        var lastNWorkouts = log.Workouts.OrderByDescending(w => w.Date)
                                                        .Where(w => w.Exercises.Any(e => e.ExerciseName.Equals(config.TargetExercise)))
                                                        .Take(config.ConsecutiveFailCount);

                        CompletedExercise lastWorkout = null;
                        if (lastNWorkouts.Any())
                            lastWorkout = lastNWorkouts.FirstOrDefault().Exercises.First(e => e.ExerciseName.Equals(config.TargetExercise));

                        if (!(lastNWorkouts != null && lastNWorkouts.Any() && lastWorkout != null))
                            continue;

                        if (!String.IsNullOrEmpty(config.TargetExercise))
                        {
                            // Update current workout's exercise to last workouts setting, so we can work from there
                            for (int i = 0; i < lastWorkout.Sets.Count; i++)
                            {
                                CurrentWorkout.ExerciseList.FirstOrDefault(e => e.Name.Equals(config.TargetExercise)).Sets[i] = new Set()
                                {
                                    NumberOfReps = lastWorkout.Sets[i].NumberOfReps,
                                    SetType = lastWorkout.Sets[i].SetType,
                                    Unit = lastWorkout.Sets[i].Unit,
                                    Weight = lastWorkout.Sets[i].Weight
                                };
                            }
                            //CurrentWorkout.ExerciseList.FirstOrDefault(e => e.Name.Equals(config.TargetExercise)).Sets = new List<Set>(lastWorkout.Sets);

                            // if last N failed
                            if (lastNWorkouts.Count(w => w.Exercises.Any(e => e.ExerciseName.Equals(config.TargetExercise) && !e.Successful)).Equals(config.ConsecutiveFailCount))
                            {
                                switch (config.ConsecutiveFailProgress)
                                {
                                    case Utilities.PlanProgressChange.Decrease:
                                        // find exercise, and for every set, apply weight modifier
                                        CurrentWorkout.ExerciseList.FirstOrDefault(e => e.Name.Equals(config.TargetExercise))
                                                                   .Sets
                                                                   .ForEach(s => s.Weight += UpdateWeightFromGivenWorkout(s, config.ConsecutiveFailAmount, config.ConsecutiveFailUnit, -1));
                                        break;
                                    case Utilities.PlanProgressChange.Maintain:
                                        // nothing to do, we already loaded last workout's setting
                                        continue;
                                    case Utilities.PlanProgressChange.Increase:
                                        // find exercise, and for every set, apply weight modifier
                                        CurrentWorkout.ExerciseList.FirstOrDefault(e => e.Name.Equals(config.TargetExercise))
                                                                   .Sets
                                                                   .ForEach(s => s.Weight += UpdateWeightFromGivenWorkout(s, config.ConsecutiveFailAmount, config.ConsecutiveFailUnit, 1));
                                        break;
                                }
                            }
                            //else, if last failed
                            else if (!lastNWorkouts.First().Exercises.First(e => e.ExerciseName.Equals(config.TargetExercise)).Successful)
                            {
                                switch (config.FailedProgress)
                                {
                                    case Utilities.PlanProgressChange.Decrease:
                                        // find exercise, and for every set, apply weight modifier
                                        CurrentWorkout.ExerciseList.FirstOrDefault(e => e.Name.Equals(config.TargetExercise))
                                                                   .Sets
                                                                   .ForEach(s => s.Weight += UpdateWeightFromGivenWorkout(s, config.FailedAmount, config.FailedUnit, -1));
                                        break;
                                    case Utilities.PlanProgressChange.Maintain:
                                        // nothing to do, we already loaded last workout's setting
                                        continue;
                                    case Utilities.PlanProgressChange.Increase:
                                        // find exercise, and for every set, apply weight modifier
                                        CurrentWorkout.ExerciseList.FirstOrDefault(e => e.Name.Equals(config.TargetExercise))
                                                                   .Sets
                                                                   .ForEach(s => s.Weight += UpdateWeightFromGivenWorkout(s, config.FailedAmount, config.FailedUnit, 1));
                                        break;
                                }
                            }
                            //else, all succeeded
                            else
                            {
                                switch (config.Progress)
                                {
                                    case Utilities.PlanProgressChange.Decrease:
                                        // find exercise, and foreach set, apply weight modifier
                                        CurrentWorkout.ExerciseList.FirstOrDefault(e => e.Name.Equals(config.TargetExercise))
                                                                   .Sets
                                                                   .ForEach(s => s.Weight += UpdateWeightFromGivenWorkout(s, config.Amount, config.Unit, -1));
                                        break;
                                    case Utilities.PlanProgressChange.Maintain:
                                        // nothing to do, we already loaded last workout's setting
                                        continue;
                                    case Utilities.PlanProgressChange.Increase:
                                        // find exercise, and foreach set, apply weight modifier
                                        CurrentWorkout.ExerciseList.FirstOrDefault(e => e.Name.Equals(config.TargetExercise))
                                                                   .Sets
                                                                   .ForEach(s => s.Weight += UpdateWeightFromGivenWorkout(s, config.Amount, config.Unit, 1));
                                        break;
                                }
                            }
                        }
                        // if it applies to all exercises
                        else
                        {
                            //object o = null;
                            //int progressTimes = -1;

                            //switch (config.TimeFrame)
                            //{
                            //    case Utilities.PlanProgressAffectedTime.Day:
                            //        o = StorageUtility.ReadSetting(Utilities.PLAN_START_DATE);
                            //        if (o != null)
                            //        {
                            //            var date = DateTime.Parse(o as string);
                            //            progressTimes = (int)DateTime.Today.Subtract(date).TotalDays / config.TimeAmount;
                            //        }
                            //        break;
                            //    case Utilities.PlanProgressAffectedTime.Week:
                            //        o = StorageUtility.ReadSetting(Utilities.PLAN_START_DATE);
                            //        if (o != null)
                            //        {
                            //            var date = DateTime.Parse(o as string);
                            //            progressTimes = ((int)DateTime.Today.Subtract(date).TotalDays / 7) / config.TimeAmount;
                            //        }
                            //        break;
                            //    case Utilities.PlanProgressAffectedTime.Month:
                            //        o = StorageUtility.ReadSetting(Utilities.PLAN_START_DATE);
                            //        if (o != null)
                            //        {
                            //            var date = DateTime.Parse(o as string);
                            //            progressTimes = ((int)DateTime.Today.Subtract(date).TotalDays / 30) / config.TimeAmount;
                            //        }
                            //        break;
                            //    case Utilities.PlanProgressAffectedTime.Exercise:
                            //    case Utilities.PlanProgressAffectedTime.Workout:
                            //        break;
                            //}

                            //Plan.Workouts.SelectMany(w => w.ExerciseList)
                            //             .SelectMany(e => e.Sets)
                            //             .Select(s => s.Weight += 1);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(config.TargetExercise + " threw an expection");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateTimes()
        {
            foreach (var config in Plan.ProgressConfiguration)
            {
                if (config.AffectedSetting.Equals(Utilities.PlanProgressAffectedSetting.Time))
                {
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateReps()
        {
            foreach (var config in Plan.ProgressConfiguration)
            {
                if (config.AffectedSetting.Equals(Utilities.PlanProgressAffectedSetting.Reps))
                {
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateSets()
        {
            foreach (var config in Plan.ProgressConfiguration)
            {
                if (config.AffectedSetting.Equals(Utilities.PlanProgressAffectedSetting.Set))
                {

                }
            }
        }
        #endregion

        /// <summary>
        /// Returns the weight to be added to previous workout's weight. (can be negative, resulting in a subtraction)
        /// </summary>
        /// <param name="set">Source set</param>
        /// <param name="amount">amount defined by config</param>
        /// <param name="unit">unit in which amount is described</param>
        /// <param name="modifier">1/-1 whether we want to add or subtract</param>
        /// <returns>value to be added</returns>11
        private double UpdateWeightFromGivenWorkout(Set set, double amount, Utilities.PlanProgressUnit unit, int modifier)
        {
            switch (unit)
            {
                case Utilities.PlanProgressUnit.Kg:
                    if (set.Unit.Equals(Utilities.Unit.Metric))
                        return Utilities.RoundToNearestKg(amount * modifier);
                    else
                        return Utilities.KgToPounds(amount) * modifier;

                case Utilities.PlanProgressUnit.Lbs:
                    if (set.Unit.Equals(Utilities.Unit.Metric))
                        return Utilities.PoundsToKg(amount) * modifier;
                    else
                        return Utilities.RoundToNearestPound(amount * modifier);

                case Utilities.PlanProgressUnit.Percent:
                    if (set.Unit.Equals(Utilities.Unit.Metric))
                        return Utilities.RoundToNearestKg(set.Weight * amount * modifier);
                    else
                        return Utilities.RoundToNearestPound(set.Weight * amount * modifier);

                case Utilities.PlanProgressUnit.Minutes:
                case Utilities.PlanProgressUnit.Reps:
                case Utilities.PlanProgressUnit.Seconds:
                case Utilities.PlanProgressUnit.Sets:
                default:
                    return 0;
            }
        }
    }
}
