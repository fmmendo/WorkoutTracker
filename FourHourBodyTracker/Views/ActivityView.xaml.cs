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
    public partial class ActivityView : PhoneApplicationPage
    {
        ActivityViewModel _vm = new ActivityViewModel();

        public ActivityView()
        {
            InitializeComponent();

            DataContext = _vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!_vm.NextExercise())
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            else
            { //todo: fix this
                NavigationService.Navigate(new Uri("/Views/RestTimerView.xaml", UriKind.Relative));
            }
        }
    }
}