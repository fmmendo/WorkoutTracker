using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using StrongliftsTracker.Resources;

namespace StrongliftsTracker
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();

            Loaded += MainPage_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //clear back stack
            while (NavigationService.CanGoBack) NavigationService.RemoveBackEntry();

            base.OnNavigatedFrom(e);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            adRotatorControl.Invalidate();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Settings.xaml", UriKind.Relative));
        }

        private void BeginWorkout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/WorkoutView.xaml", UriKind.Relative));
        }

        private void OneRepMax_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/OneRepMaxView.xaml", UriKind.Relative));
        }

        private void Measurements_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/MeasurementsView.xaml", UriKind.Relative));
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}