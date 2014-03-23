using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutLib
{
    public static class Utilities
    {
        public enum Gender
        {
            Male,
            Female
        }
        public enum Unit
        {
            Imperial,
            Metric
        }
        /// <summary>
        /// 
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
            PartialReps
        }

        public enum PlanProgressChange
        {
            Increase,
            Decrease
        }

        public enum PlanProgressAffectedValue
        { 
            Weight, 
            Reps, 
            Time
        }

        public enum PlanProgressAffectedTime
        {
            Week,
            Month,
            Day,
            Workout
        }

        public enum PlanProgressUnit
        {
            Lbs, 
            Kg, 
            Percent, 
            Minutes, 
            Seconds,
        }

        public static readonly List<int> Percentages = new List<int>() { 50, 55, 60, 65, 70, 75, 80, 85, 90, 95 };

        public static readonly int TIME_INCREMENT =30;

        public static readonly string PLAN_START_DATE = "plan_start_date";
        public static readonly string NUMBER_COMPLETED_WORKOUTS = "number_completed_workouts";

        public static readonly string GENDER_SETTING = "setting_gender";
        public static readonly string UNIT_SETTING = "setting_unit";
        public static readonly string ONERM_SETTING = "setting_unit";
        public static readonly string NEXTWORKOUT_SETTING = "setting_nextworkout";

        public static readonly string WAIST_MEASUREMENT = "measurement_waist";
        public static readonly string MECK_MEASUREMENT = "measurement_neck";
        public static readonly string HEIGHT_MEASUREMENT = "measurement_height";
        public static readonly string LEFTUPPERARM_MEASUREMENT = "measurement_leftupperarm";
        public static readonly string LEFTLOWERARM_MEASUREMENT = "measurement_leftlowerarm";
        public static readonly string LEFTQUAD_MEASUREMENT = "measurement_leftquad";
        public static readonly string LEFTCALF_MEASUREMENT = "measurement_leftcalf";
        public static readonly string RIGHTUPPERARM_MEASUREMENT = "measurement_rightupperarm";
        public static readonly string RIGHTLOWERARM_MEASUREMENT = "measurement_rightlowerarm";
        public static readonly string RIGHTQUAD_MEASUREMENT = "measurement_rightquad";
        public static readonly string RIGHTCALF_MEASUREMENT = "measurement_rightcalf";
        public static readonly string THIGHS_MEASUREMENT = "measurement_thighs";
        public static readonly string CHEST_MEASUREMENT = "measurement_chest";
        public static readonly string SHOULDERS_MEASUREMENT = "measurement_shoulders";
        public static readonly string WEIGHT_MEASUREMENT = "measurement_weight";
        
        public static readonly string ONEREPMAXVALUES = "one_rep_max_values";

        public static string LoadJsonFromFile(string path)
        {
            using (FileStream file = File.OpenRead(path))
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    var json = sr.ReadToEndAsync();
                    return json.Result;
                }
            }
        }

        public static readonly double PoundKGFraction = 0.45359237;

        public static double PoundsToKg(double pounds)
        {
            return pounds * PoundKGFraction;
        }
        public static double KgToPounds(double kg)
        {
            return kg / PoundKGFraction;
        }
    }
}
