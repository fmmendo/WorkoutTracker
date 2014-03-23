using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WorkoutLib.ViewModel;

namespace FourHourBodyTracker.Views
{
    public partial class Settings : PhoneApplicationPage
    {
        UserSettings _vm;

        public Settings()
        {
            InitializeComponent();

            var popup = (Action<string>)(msg => MessageBox.Show(msg));
            var confirm = (Func<string, string, bool>)((msg, capt) => MessageBox.Show(msg, capt, MessageBoxButton.OKCancel) == MessageBoxResult.OK);

            _vm = UserSettings.Settings;
            _vm.SetMessageBox(popup);
            _vm.SetConfirmMessageBox(confirm);
            
            DataContext = _vm;
        }
    }
}