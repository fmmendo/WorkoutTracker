using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WorkoutLib.ViewModel
{
    public class UserSettings : ViewModelBase
    {
        #region Singleton
        private static UserSettings _settings;
        /// <summary>
        /// One unique Settings VM for the app
        /// </summary>
        public static UserSettings Settings
        {
            get
            {
                if (_settings == null)
                    _settings = new UserSettings();

                return _settings;
            }
        }
        #endregion

        #region Properties
        private Utilities.Gender _gender;
        /// <summary>
        /// User gender.
        /// Saves on change.
        /// </summary>
        public Utilities.Gender Gender
        {
            get { return _gender; }
            set
            {
                if (_gender != value)
                {
                    _gender = value;
                    StorageUtility.WriteSetting(Utilities.GENDER_SETTING, _gender);
                }
            }
        }

        private Utilities.Unit _unit;
        /// <summary>
        /// Unit to use in app.
        /// Saves on change.
        /// </summary>
        public Utilities.Unit Unit
        {
            get { return _unit; }
            set
            {
                if (_unit != value)
                {
                    _unit = value;
                    StorageUtility.WriteSetting(Utilities.UNIT_SETTING, _unit);
                }
            }
        }

        private string _selectedPercentage;
        /// <summary>
        /// Selected One Rep Max percentage for usage in the app
        /// </summary>
        public string SelectedPercentage
        {
            get { return _selectedPercentage; }
            set
            {
                _selectedPercentage = value;
                StorageUtility.WriteSetting(Utilities.ONERM_SETTING, _selectedPercentage);
            }
        }

        private bool _planProgress;
        /// <summary>
        /// Selected One Rep Max percentage for usage in the app
        /// </summary>
        public bool PlanProgress
        {
            get { return _planProgress; }
            set
            {
                _planProgress = value;
                StorageUtility.WriteSetting(Utilities.PLANPROGRESS_SETTING, _planProgress);
            }
        }
        #endregion

        /// <summary>
        /// Gender list for the view
        /// </summary>
        public IEnumerable<Utilities.Gender> GenderList
        {
            get
            {
                return Enum.GetValues(typeof(Utilities.Gender)).Cast<Utilities.Gender>();
            }
        }

        /// <summary>
        /// Unit List for the view
        /// </summary>
        public IEnumerable<Utilities.Unit> UnitList
        {
            get
            {
                return Enum.GetValues(typeof(Utilities.Unit)).Cast<Utilities.Unit>();
            }
        }

        /// <summary>
        /// List of percentages for the view
        /// </summary>
        public IEnumerable<string> Percentages
        {
            get
            {
                return Utilities.Percentages.Select(p => p.ToString());
            }
        }

        #region Button Commands
        /// <summary>
        /// ICommand for the Reset Plan Button
        /// </summary>
        public ICommand ResetPlanCommand { get; set; }
        public bool CanExecuteResetPlan(object param)
        {
            return true;
        }
        public void ExecuteResetPlan(object param)
        {
            if (ShowMessageWithConfirmation("Reset Plan?", "Are you sure you want to reset the plan back to day 1?"))
            {
                StorageUtility.WriteSetting(Utilities.NEXTWORKOUT_SETTING, 0);
                ShowMessage("Workout Plan has been reset. \nYou will now start back in day 1.");
            }
        }

        /// <summary>
        /// ICommand for the Reset App button
        /// </summary>
        public ICommand ResetAppCommand { get; set; }
        public bool CanExecuteResetApp(object param)
        {
            return true;
        }
        public void ExecuteResetApp(object param)
        {
            if (ShowMessageWithConfirmation("Reset App?", "Are you sure you want to reset? \nThis will clear all data stored by the app."))
            {
                StorageUtility.ClearSettings();
                ShowMessage("App has now been reset.");
            }
        }
        #endregion

        private UserSettings()
        {
            object gender = StorageUtility.ReadSetting(Utilities.GENDER_SETTING);
            object unit = StorageUtility.ReadSetting(Utilities.UNIT_SETTING);
            object percentage = StorageUtility.ReadSetting(Utilities.ONERM_SETTING);
            object progress = StorageUtility.ReadSetting(Utilities.PLANPROGRESS_SETTING);

            if (gender != null && gender is Utilities.Gender)
                Gender = (Utilities.Gender)gender;
            else Gender = Utilities.Gender.Male; //default

            if (unit != null && unit is Utilities.Unit)
                Unit = (Utilities.Unit)unit;
            else Unit = Utilities.Unit.Imperial; //default

            if (percentage != null && percentage is string)
                SelectedPercentage = (string)percentage;
            else SelectedPercentage = "85"; //default

            if (progress != null && progress is bool)
                PlanProgress = (bool)progress;
            else PlanProgress = true; //default

            ResetPlanCommand = new ButtonCommand(ExecuteResetPlan, CanExecuteResetPlan);
            ResetAppCommand = new ButtonCommand(ExecuteResetApp, CanExecuteResetApp);
        }
    }


}
