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

namespace StrongliftsTracker.Views
{
    public partial class AboutView : PhoneApplicationPage
    {
        private AboutViewModel _vm;

        public AboutView()
        {
            _vm = new AboutViewModel();
            DataContext = _vm;

            InitializeComponent();
        }
    }
}