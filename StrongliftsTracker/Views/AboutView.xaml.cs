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
using Microsoft.Phone.Tasks;

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

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as HyperlinkButton;
            if (button == null) return;

            var wbt = new WebBrowserTask();
            wbt.Uri = new Uri("http://stronglifts.com/5x5/", UriKind.Absolute);
            wbt.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = "Feedback: Stronglifts Tracker";
            emailComposeTask.Body = "";
            emailComposeTask.To = "feedback@fmendo.com ";

            emailComposeTask.Show();
        }
    }
}