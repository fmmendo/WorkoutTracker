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
    public partial class OneRepMaxView : PhoneApplicationPage
    {
        OneRepMaxViewModel _vm;

        public OneRepMaxView()
        {
            InitializeComponent();

            _vm = new OneRepMaxViewModel();
            var popup = (Action<string>)(msg => MessageBox.Show(msg));
            _vm.SetMessageBox(popup);
            DataContext = _vm;
        }
    }
}