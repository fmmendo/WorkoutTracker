using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using WorkoutLib.ViewModel;
using Microsoft.Phone.Controls;

namespace StrongliftsTracker.Controls
{
    public partial class RestTimerView : PhoneApplicationPage
    {
        RestTimerViewModel _vm;

        public RestTimerView()
        {
            InitializeComponent();

            _vm = new RestTimerViewModel();
            DataContext = _vm;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New && e.IsNavigationInitiator == true)
                _vm.StartTimer();
            else if (e.NavigationMode == NavigationMode.Back && e.IsNavigationInitiator == false)
            { }
            else return;

        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New && e.IsNavigationInitiator == true)
                _vm.StopTimer();
            else if (e.NavigationMode == NavigationMode.Back && e.IsNavigationInitiator == false)
            { }
            else return;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
