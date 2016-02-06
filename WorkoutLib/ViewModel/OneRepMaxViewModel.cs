using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WorkoutLib.Model.Storage;

namespace WorkoutLib.ViewModel
{
    public class OneRepMaxViewModel : ViewModelBase
    {
        #region Properties
        private string _weight;
        /// <summary>
        /// Weight for the calculation
        /// </summary>
        public string Weight
        {
            get { return _weight; }
            set
            {
                _weight = value;
                NotifyPropertyChanged();
                GetOneRepMaxValue();

            }
        }

        private string _reps;
        /// <summary>
        /// Reps for the calculation
        /// </summary>
        public string Reps
        {
            get { return _reps; }
            set
            {
                _reps = value;
                NotifyPropertyChanged();
                GetOneRepMaxValue();
            }
        }

        private bool _ignore1RM;
        /// <summary>
        /// Reps for the calculation
        /// </summary>
        public bool Ignore1RM
        {
            get { return _ignore1RM; }
            set
            {
                _ignore1RM = value;
                NotifyPropertyChanged();
                GetOneRepMaxValue();
            }
        }

        /// <summary>
        /// Exercise list
        /// </summary>
        public IEnumerable<string> Exercises
        {
            get { return WorkoutService.Service.ExerciseNames; }
        }

        /// <summary>
        /// Exercise list
        /// </summary>
        public string UnitInUse
        {
            get { return UserSettings.Settings.Unit.Equals(Utilities.Unit.Imperial) ? "lbs" : "kg"; }
        }

        private string _exerciseName;
        /// <summary>
        /// Name of the exercise to save 1RM
        /// </summary>
        public string ExerciseName
        {
            get { return _exerciseName; }
            set
            {
                _exerciseName = value;
                NotifyPropertyChanged();
                //SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private double _oneRepMaxValue;
        /// <summary>
        /// Maximum lift achievable
        /// </summary>
        public double OneRepMaxValue
        {
            get
            {
                return _oneRepMaxValue;
            }
        }

        /// <summary>
        /// Maximum lift achievable
        /// </summary>
        public string OneRepMax
        {
            get
            {
                if (OneRepMaxValue > 0)
                    return String.Format("1RM = {0} {1}", OneRepMaxValue.ToString("#.##"),
                                                          UserSettings.Settings.Unit.Equals(Utilities.Unit.Imperial) ? "lbs" : "kg");
                else return String.Empty;
            }
        }

        private ObservableCollection<string> _percentages = new ObservableCollection<string>();
        /// <summary>
        /// Percentages for the 1RM value
        /// </summary>
        public ObservableCollection<string> Percentages
        {
            get
            {
                return _percentages;
            }
        }
        #endregion

        private ObservableCollection<ExerciseValues> _exercise1RMs = new ObservableCollection<ExerciseValues>();
        public ObservableCollection<ExerciseValues> Exercise1RMs
        {
            get { return _exercise1RMs; }
            set { _exercise1RMs = value; }
        }

        #region Commands
        public ButtonCommand SaveCommand { get; set; }
        public bool CanExecuteSaveCommand(object param)
        {
            return true;// !String.IsNullOrEmpty(ExerciseName) && OneRepMaxValue > 0;
        }
        public void ExecuteSaveCommand(object param)
        {
            // If no data, do nothing
            if (String.IsNullOrEmpty(ExerciseName) || OneRepMaxValue <= 0)
            {
                ShowMessage("No data to save.\nEnsure you have added your lifting weight and reps, along with the exercise name you want to save.");
                return;
            }

            var item = Exercise1RMs.FirstOrDefault(e => e.Name.Equals(ExerciseName));
            // If item exists in storage, update item
            if (item != null)
            {
                int index = Exercise1RMs.IndexOf(item);
                Exercise1RMs[index].OneRepMaxValue = OneRepMaxValue;
                Exercise1RMs[index].Percentages = Percentages;
                Exercise1RMs[index].Unit = UserSettings.Settings.Unit;
            }
            // If not, add new
            else
                Exercise1RMs.Add(new ExerciseValues() { Name=ExerciseName, OneRepMaxValue=OneRepMaxValue, Percentages=Percentages, Unit=UserSettings.Settings.Unit});

            StorageUtility.WriteSetting(Utilities.ONEREPMAXVALUES, Exercise1RMs);

            ShowMessage("One Rep Max value saved. \nYou can review all your 1RM values by swiping left.");

            WorkoutService.Service.RefreshPlan();
            NotifyPropertyChanged("ExerciseValues");
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public OneRepMaxViewModel()
        {
            SaveCommand = new ButtonCommand(ExecuteSaveCommand, CanExecuteSaveCommand);

            object o = StorageUtility.ReadSetting(Utilities.ONEREPMAXVALUES);
            if (o != null)
            {
                Exercise1RMs = (ObservableCollection<ExerciseValues>)o;
            }
            ExerciseName = Exercises.ElementAt(0);
        }

        private void GetOneRepMaxValue()
        {
            double w;
            int r;
            //if (Double.TryParse(Weight, out w) && Int32.TryParse(Reps, out r))
            Double.TryParse(Weight, out w);
            Int32.TryParse(Reps, out r);

            //{
                if (w > 0 && r > 0 && !_ignore1RM)
                {
                    _oneRepMaxValue = (w * (1 + ((float)r / 30)));

                    _percentages.Clear();
                    int[] array = { 95, 90, 85, 80, 75, 70, 65, 60, 55, 50 };
                    foreach (int val in array)
                        _percentages.Add(String.Format("{0}% - {1} {2}", val,
                            (((float)val / 100) * _oneRepMaxValue).ToString("#.##"),
                            UserSettings.Settings.Unit.Equals(Utilities.Unit.Imperial) ? "lbs" : "kg"));
                }
                else if (_ignore1RM)
                {
                    _oneRepMaxValue = w / int.Parse(UserSettings.Settings.SelectedPercentage ?? "85");
                    
                    _percentages.Clear();
                    int[] array = { 95, 90, 85, 80, 75, 70, 65, 60, 55, 50 };
                    foreach (int val in array)
                        _percentages.Add(String.Format("{0}% - {1} {2}", val, (w * ((double)val/100)).ToString("#.##"),
                            UserSettings.Settings.Unit.Equals(Utilities.Unit.Imperial) ? "lbs" : "kg"));
                }
                else
                {
                    _oneRepMaxValue = -1;
                    _percentages.Clear();
                }
            //}
            //else
            //{
            //    _oneRepMaxValue = -1;
            //    _percentages.Clear();
            //}

            NotifyPropertyChanged("OneRepMax");
            NotifyPropertyChanged("Percentages");
        }
    }
}
