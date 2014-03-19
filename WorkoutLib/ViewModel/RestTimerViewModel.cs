using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WorkoutLib.ViewModel
{
    public class RestTimerViewModel : ViewModelBase
    {
        DispatcherTimer _timer = new DispatcherTimer();

        private int _restSeconds;
        /// <summary>
        /// Remaining rest time in seconds
        /// </summary>
        public int RestSeconds
        {
            get { return _restSeconds; }
            set
            {
                _restSeconds = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("RestSecondsString");
            }
        }

        private Visibility _timerVisibility;
        /// <summary>
        /// Gets or set the visibility of the View
        /// </summary>
        public Visibility TimerVisibility
        {
            get { return _timerVisibility; }
            set
            {
                _timerVisibility = value;
                NotifyPropertyChanged();

                if (_timerVisibility.Equals(Visibility.Visible))
                {
                    StartTimer();
                }
                else { }
            }
        }

        public string RestSecondsString
        { get { return RestSeconds > 0 ? RestSeconds.ToString() : "Go!"; } }

        #region Commands

        /// <summary>
        /// Less Time COmmand
        /// </summary>
        public ButtonCommand LessTimeCommand { get; set; }
        public bool CanExecuteLessTimeCommand(object param)
        {
            return true;
        }
        public void ExecuteLessTimeCommand(object param)
        {
            if (RestSeconds > Utilities.TIME_INCREMENT)
                RestSeconds -= Utilities.TIME_INCREMENT;
            else
            {
                RestSeconds = 0;
                _timer.Stop();
            }
        }

        /// <summary>
        /// More Time Command
        /// </summary>
        public ButtonCommand MoreTimeCommand { get; set; }
        public bool CanExecuteMoreTimeCommand(object param)
        {
            return true;
        }
        public void ExecuteMoreTimeCommand(object param)
        {
            RestSeconds += Utilities.TIME_INCREMENT;
        }

        /// <summary>
        /// "I'm Done" Command
        /// </summary>
        public ButtonCommand ImDoneCommand { get; set; }
        public bool CanExecuteImDoneCommand(object param)
        {
            return true;
        }
        public void ExecuteImDOneCommand(object param)
        {
            TimerVisibility = Visibility.Collapsed;
        }

        #endregion

        public RestTimerViewModel()
        {
            RestSeconds = 60;

            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Tick += _timer_Tick;

            MoreTimeCommand = new ButtonCommand(ExecuteMoreTimeCommand, CanExecuteMoreTimeCommand);
            LessTimeCommand = new ButtonCommand(ExecuteLessTimeCommand, CanExecuteLessTimeCommand);
        }

        public void StartTimer()
        {
            if (_timer != null)
                _timer.Start();
        }
        public void StopTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
                RestSeconds = 60;
            }
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            RestSeconds -= 1;

            if (RestSeconds <= 0)
                StopTimer();
        }
    }
}
