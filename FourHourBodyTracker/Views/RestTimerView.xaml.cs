﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using WorkoutLib.ViewModel;
using Microsoft.Phone.Controls;

namespace FourHourBodyTracker.Controls
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
            _vm.StartTimer();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            _vm.StopTimer();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
