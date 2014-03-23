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
    public partial class WorkoutView : PhoneApplicationPage
    {
        WorkoutViewModel _vm = new WorkoutViewModel();

        public WorkoutView()
        {
            InitializeComponent();
            DataContext = _vm;
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/ActivityView.xaml", UriKind.Relative));
        }
    }
}