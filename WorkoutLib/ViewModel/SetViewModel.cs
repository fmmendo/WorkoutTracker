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
                    return _weight.ToString("#.##");
                else if (_set.Weight > 0)
                    return _set.Weight.ToString("#.##");
                else
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
        /// Inidcates if this is the current set for the exercise
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
            get { return CurrentSet 
                    ? Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush
                    : new SolidColorBrush(Colors.Gray);
            }
        }

        public SetViewModel(Set s, double weight = -1)
        {
            _set = s;
            _weight = weight;
        }
    }
}
