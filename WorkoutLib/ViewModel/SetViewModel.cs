using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WorkoutLib.Model;

namespace WorkoutLib.ViewModel
{
    public class SetViewModel : ViewModelBase
    {
        private Set _set;

        /// <summary>
        /// Number of reps for this set
        /// </summary>
        public int NumberOfReps { get { return _set.NumberOfReps; } }

        private double _weight;
        /// <summary>
        /// Weight for this set.
        /// If the constructor was passed another weight, return that instead
        /// </summary>
        public string Weight
        {
            get
            {
                if (_weight > 0)
                {
                    return _weight.ToString("#.##");
                }
                else
                {
                    if (_set.Weight > 0)
                    {

                        if (_set.Unit.Equals(UserSettings.Settings.Unit))
                            return _set.Weight.ToString("#.##");
                        else if (_set.Unit.Equals(Utilities.Unit.Imperial) && (UserSettings.Settings.Unit.Equals(Utilities.Unit.Metric)))
                            return Utilities.PoundsToKg(_set.Weight).ToString("#.##");
                        else if (_set.Unit.Equals(Utilities.Unit.Metric) && (UserSettings.Settings.Unit.Equals(Utilities.Unit.Imperial)))
                            return Utilities.KgToPounds(_set.Weight).ToString("#.##");
                    }
                    else
                        return String.Empty;
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// Unit in which the weight is described
        /// </summary>
        public string Unit
        {
            get
            {
                if (String.IsNullOrEmpty(Weight))
                    return String.Empty;
                else
                    return UserSettings.Settings.Unit.Equals(Utilities.Unit.Imperial) ? "lbs" : "kg";
            }
        }

        private bool _currentSet;
        /// <summary>
        /// Indicates if this is the current set for the exercise
        /// </summary>
        public bool CurrentSet
        {
            get { return _currentSet; }
            set
            {
                _currentSet = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("SetForegroundColour");
            }
        }

        public SolidColorBrush SetForegroundColour
        {
            get
            {
                return CurrentSet
                  ? Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush
                  : new SolidColorBrush(Colors.Gray);
            }
        }

        public SetViewModel(Set s, double weight = -1)
        {
            _set = s;
            _weight = weight;
            //_weight = RecalculateWeightIfRequired(weight);
        }

        //private static double RecalculateWeightIfRequired(double weight)
        //{
        //    if (WorkoutService.Service.Plan.ProgressConfiguration != null && WorkoutService.Service.Plan.ProgressConfiguration.AffectedValue == Utilities.PlanProgressAffectedValue.Weight)
        //    {
        //        int progressTimes = -1;
        //        object o = null;
        //        switch (WorkoutService.Service.Plan.ProgressConfiguration.TimeFrame)
        //        {

        //            case Utilities.PlanProgressAffectedTime.Day:
        //                o = StorageUtility.ReadSetting(Utilities.PLAN_START_DATE);
        //                if (o != null)
        //                {
        //                    var date = DateTime.Parse(o as string);
        //                    progressTimes = (int)DateTime.Today.Subtract(date).TotalDays / WorkoutService.Service.Plan.ProgressConfiguration.TimeAmount;
        //                }
        //                break;
        //            case Utilities.PlanProgressAffectedTime.Week:
        //                o = StorageUtility.ReadSetting(Utilities.PLAN_START_DATE);
        //                if (o != null)
        //                {
        //                    var date = DateTime.Parse(o as string);
        //                    progressTimes = ((int)DateTime.Today.Subtract(date).TotalDays / 7) / WorkoutService.Service.Plan.ProgressConfiguration.TimeAmount;
        //                }
        //                break;
        //            case Utilities.PlanProgressAffectedTime.Month:
        //                o = StorageUtility.ReadSetting(Utilities.PLAN_START_DATE);
        //                if (o != null)
        //                {
        //                    var date = DateTime.Parse(o as string);
        //                    progressTimes = ((int)DateTime.Today.Subtract(date).TotalDays / 30) / WorkoutService.Service.Plan.ProgressConfiguration.TimeAmount;
        //                }
        //                break;
        //            case Utilities.PlanProgressAffectedTime.Workout:
        //                break;
        //        }

        //        for (int i = 0; i < progressTimes; i++)
        //        {
        //            if (WorkoutService.Service.Plan.ProgressConfiguration.Unit == Utilities.PlanProgressUnit.Percent)
        //            {
        //                weight *= WorkoutService.Service.Plan.ProgressConfiguration.Amount / 100;
        //            }
        //            else if (WorkoutService.Service.Plan.ProgressConfiguration.Unit == Utilities.PlanProgressUnit.Kg)
        //            {
        //                if (UserSettings.Settings.Unit == Utilities.Unit.Metric)
        //                    weight += WorkoutService.Service.Plan.ProgressConfiguration.Amount;
        //                else
        //                    weight += Utilities.KgToPounds(WorkoutService.Service.Plan.ProgressConfiguration.Amount);
        //            }
        //            else if (WorkoutService.Service.Plan.ProgressConfiguration.Unit == Utilities.PlanProgressUnit.Lbs)
        //            {
        //                if (UserSettings.Settings.Unit == Utilities.Unit.Imperial)
        //                    weight += WorkoutService.Service.Plan.ProgressConfiguration.Amount;
        //                else
        //                    weight += Utilities.PoundsToKg(WorkoutService.Service.Plan.ProgressConfiguration.Amount);
        //            }
        //        }
        //    }
        //    return weight;
        //}
    }
}
