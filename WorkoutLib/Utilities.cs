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
    public static class Utilities
    {
        #region Enumerations
        /// <summary>
        /// User gender
        /// </summary>
        public enum Gender
        {
            Male,
            Female
        }

        /// <summary>
        /// Unit to be used throughout the app
        /// </summary>
        public enum Unit
        {
            Imperial,
            Metric
        }

        /// <summary>
        /// Types of weightlifting sets
        /// </summary>
        public enum SetType
        {
            Normal,
            Superset,
            DropSet,
            RestPause,
            ForcedReps,
            NegativeReps,
            CheatingReps,
            PartialReps,
            WarmUp
        }

        /// <summary>
        /// Types of exercises
        /// </summary>
        public enum ExerciseType
        {
            Strength,
            Cardio
        }

        /// <summary>
        /// Indicates how to progress with the plan
        /// </summary>
        public enum PlanProgressChange
        {
            Increase,
            Decrease,
            Maintain
        }

        /// <summary>
        /// Indicates the setting to be modified
        /// </summary>
        public enum PlanProgressAffectedSetting
        { 
            Weight, 
            Reps,
            Set,
            Time
        }

        /// <summary>
        /// Determines how often to progress a certain setting
        /// </summary>
        public enum PlanProgressAffectedTime
        {
            Week,
            Month,
            Day,
            Workout,
            Exercise
        }

        /// <summary>
        /// Unit in which the progress amount is specified
        /// </summary>
        public enum PlanProgressUnit
        {
            Lbs, 
            Kg, 
            Percent, 
            Minutes, 
            Seconds,
            Reps,
            Sets
        }
        #endregion

        public static readonly List<int> Percentages = new List<int>() { 50, 55, 60, 65, 70, 75, 80, 85, 90, 95 };

        public static readonly int TIME_INCREMENT = 30;

        public static readonly string PLAN_START_DATE = "plan_start_date";
        //public static readonly string NUMBER_COMPLETED_WORKOUTS = "number_completed_workouts";
        public static readonly string PLAN_LOG = "plan_log";

        public static readonly string GENDER_SETTING = "setting_gender";
        public static readonly string UNIT_SETTING = "setting_unit";
        public static readonly string ONERM_SETTING = "setting_onerm";
        public static readonly string NEXTWORKOUT_SETTING = "setting_nextworkout";
        public static readonly string PLANPROGRESS_SETTING = "setting_planprogress";

        //public static readonly string WAIST_MEASUREMENT = "measurement_waist";
        //public static readonly string MECK_MEASUREMENT = "measurement_neck";
        //public static readonly string HEIGHT_MEASUREMENT = "measurement_height";
        //public static readonly string LEFTUPPERARM_MEASUREMENT = "measurement_leftupperarm";
        //public static readonly string LEFTLOWERARM_MEASUREMENT = "measurement_leftlowerarm";
        //public static readonly string LEFTQUAD_MEASUREMENT = "measurement_leftquad";
        //public static readonly string LEFTCALF_MEASUREMENT = "measurement_leftcalf";
        //public static readonly string RIGHTUPPERARM_MEASUREMENT = "measurement_rightupperarm";
        //public static readonly string RIGHTLOWERARM_MEASUREMENT = "measurement_rightlowerarm";
        //public static readonly string RIGHTQUAD_MEASUREMENT = "measurement_rightquad";
        //public static readonly string RIGHTCALF_MEASUREMENT = "measurement_rightcalf";
        //public static readonly string THIGHS_MEASUREMENT = "measurement_thighs";
        //public static readonly string CHEST_MEASUREMENT = "measurement_chest";
        //public static readonly string SHOULDERS_MEASUREMENT = "measurement_shoulders";
        //public static readonly string WEIGHT_MEASUREMENT = "measurement_weight";
        
        public static readonly string ONEREPMAXVALUES = "one_rep_max_values";

        /// <summary>
        /// Load a file and returns it's text contents
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns></returns>
        public static string LoadFileAsString(string path)
        {
            using (FileStream file = File.OpenRead(path))
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    var text = sr.ReadToEndAsync();
                    return text.Result;
                }
            }
        }

        /// <summary>
        /// Deserializes a JSON string containing details for a Plan
        /// </summary>
        /// <param name="json">JSON encoded plan</param>
        /// <returns>Plan</returns>
        public static Plan DeserializePlan(string json)
        {
            if (String.IsNullOrEmpty(json))
                return new Plan();

            json = json.Replace("\n", "").Replace("\r", "");

            RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(json);
            rootObject.Plan.CurrentWorkout = 1;
            return rootObject.Plan;
        }

        /// <summary>
        /// Serializes the given plan into a JSON string
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public static string SerializePlan(Plan plan)
        {
            var rootoject = new RootObject() { Plan = plan };
            string json = JsonConvert.SerializeObject(rootoject);
            return json;
        }

        /// <summary>
        /// Amount of Kilograms in a Pound
        /// </summary>
        public static readonly double PoundKGFraction = 0.45359237;

        /// <summary>
        /// Converts Pounds to Kilograms
        /// </summary>
        /// <param name="pounds">Weight in Pounds</param>
        /// <returns>Weight in Kilograms</returns>
        public static double PoundsToKg(double pounds)
        {
            return RoundToNearestKg(pounds * PoundKGFraction);
        }
        
        /// <summary>
        /// Converts Kilograms to Pounds
        /// </summary>
        /// <param name="kg">Weight in Kilograms</param>
        /// <returns>Weight in Pounds</returns>
        public static double KgToPounds(double kg)
        {
            return RoundToNearestPound(kg / PoundKGFraction);
        }

        /// <summary>
        /// Rounds given weight to nearest 2.5kg plate
        /// </summary>
        /// <param name="kg"></param>
        /// <returns></returns>
        public static double RoundToNearestKg(double kg)
        {
            return (2.5 - kg % 2.5) + kg;
        }

        /// <summary>
        /// Rounds given weight to nearest 5lbs plate
        /// </summary>
        /// <param name="pound"></param>
        /// <returns></returns>
        public static double RoundToNearestPound(double pound)
        {
            return (5 - pound % 5) + pound;
        }
    }
}
