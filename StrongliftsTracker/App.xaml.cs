﻿using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using StrongliftsTracker.Resources;
using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Xml;
using WorkoutLib;

namespace StrongliftsTracker
{
    public partial class App : Application
    {
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public static PhoneApplicationFrame RootFrame { get; private set; }

        enum SessionType
        {
            None,
            Home,
            DeepLink
        }

        // Set to Home when the app is launched from Primary tile.
        // Set to DeepLink when the app is launched from Deep Link.
        private SessionType sessionType = SessionType.None;

        // Set to true when the page navigation is being reset 
        bool wasRelaunched = false;

        // set to true when 15 min passed since the app was relaunched
        bool mustClearPagestack = false;

        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            WorkoutLib.WorkoutService.Service.LoadPlan(Utilities.LoadFileAsString("Assets\\StrongLifts.json"));

            // Global handler for uncaught exceptions.
            UnhandledException += Application_UnhandledException;

            // Standard XAML initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Language display initialization
            InitializeLanguage();

            // Show graphics profiling information while debugging.
            if (Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode,
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Prevent the screen from turning off while under the debugger by disabling
                // the application's idle detection.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {            
            // When a new instance of the app is launched, clear all deactivation settings
            RemoveCurrentDeactivationSettings();
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {            
            // If some interval has passed since the app was deactivated (30 seconds in this example),
            // then remember to clear the back stack of pages
            mustClearPagestack = CheckDeactivationTimeStamp();

            // If IsApplicationInstancePreserved is not true, then set the session type to the value
            // saved in isolated storage. This will make sure the session type is correct for an
            // app that is being resumed after being tombstoned.
            if (!e.IsApplicationInstancePreserved)
            {
                RestoreSessionType();
            }
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {            
            // When the applicaiton is deactivated, save the current deactivation settings to isolated storage
            SaveCurrentDeactivationSettings();
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {            
            // When the application closes, delete any deactivation settings from isolated storage
            RemoveCurrentDeactivationSettings();
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            #region AdMediator Exceptions
            if (e != null)
            {
                Exception exception = e.ExceptionObject;
                if ((exception is XmlException || exception is NullReferenceException) && exception.ToString().ToUpper().Contains("INNERACTIVE"))
                {
                    Debug.WriteLine("Handled Inneractive exception {0}", exception);
                    e.Handled = true;
                    return;
                }
                else if (exception is NullReferenceException && exception.ToString().ToUpper().Contains("SOMA"))
                {
                    Debug.WriteLine("Handled Smaato null reference exception {0}", exception);
                    e.Handled = true;
                    return;
                }
                else if ((exception is System.IO.IOException || exception is NullReferenceException) && exception.ToString().ToUpper().Contains("GOOGLE"))
                {
                    Debug.WriteLine("Handled Google exception {0}", exception);
                    e.Handled = true;
                    return;
                }
                else if (exception is ObjectDisposedException && exception.ToString().ToUpper().Contains("MOBFOX"))
                {
                    Debug.WriteLine("Handled Mobfox exception {0}", exception);
                    e.Handled = true;
                    return;
                }
                else if ((exception is NullReferenceException || exception is XamlParseException) && exception.ToString().ToUpper().Contains("MICROSOFT.ADVERTISING"))
                {
                    Debug.WriteLine("Handled Microsoft.Advertising exception {0}", exception);
                    e.Handled = true;
                    return;
                }
            }
            #endregion

            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Handle reset requests for clearing the backstack
            RootFrame.Navigated += CheckForResetNavigation;

            // Monitor deep link launching 
            RootFrame.Navigating += RootFrame_Navigating;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Event handler for the Navigating event of the root frame. Use this handler to modify
        // the default navigation behavior.
        void RootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {

            // If the session type is None or New, check the navigation Uri to determine if the
            // navigation is a deep link or if it points to the app's main page.
            if (sessionType == SessionType.None && e.NavigationMode == NavigationMode.New)
            {
                // This block will run if the current navigation is part of the app's intial launch


                // Keep track of Session Type 
                if (e.Uri.ToString().Contains("DeepLink=true"))
                {
                    sessionType = SessionType.DeepLink;
                }
                else if (e.Uri.ToString().Contains("/MainPage.xaml"))
                {
                    sessionType = SessionType.Home;
                }
            }


            if (e.NavigationMode == NavigationMode.Reset)
            {
                // This block will execute if the current navigation is a relaunch.
                // If so, another navigation will be coming, so this records that a relaunch just happened
                // so that the next navigation can use this info.
                wasRelaunched = true;
            }
            else if (e.NavigationMode == NavigationMode.New && wasRelaunched)
            {
                // This block will run if the previous navigation was a relaunch
                wasRelaunched = false;

                if (e.Uri.ToString().Contains("DeepLink=true"))
                {
                    // This block will run if the launch Uri contains "DeepLink=true" which
                    // was specified when the secondary tile was created in MainPage.xaml.cs

                    sessionType = SessionType.DeepLink;
                    // The app was relaunched via a Deep Link.
                    // The page stack will be cleared.
                }
                else if (e.Uri.ToString().Contains("/MainPage.xaml"))
                {
                    // This block will run if the navigation Uri is the main page
                    if (sessionType == SessionType.DeepLink)
                    {
                        // When the app was previously launched via Deep Link and relaunched via Main Tile, we need to clear the page stack. 
                        sessionType = SessionType.Home;
                    }
                    else
                    {
                        if (!mustClearPagestack)
                        {
                            //The app was previously launched via Main Tile and relaunched via Main Tile. Cancel the navigation to resume.
                            e.Cancel = true;
                            RootFrame.Navigated -= ClearBackStackAfterReset;
                        }
                    }
                }

                mustClearPagestack = false;
            }
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // If the app has received a 'reset' navigation, then we need to check
            // on the next navigation to see if the page stack should be reset
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // For UI consistency, clear the entire page stack
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // do nothing
            }
        }

        #endregion

        // Initialize the app's font and flow direction as defined in its localized resource strings.
        //
        // To ensure that the font of your application is aligned with its supported languages and that the
        // FlowDirection for each of those languages follows its traditional direction, ResourceLanguage
        // and ResourceFlowDirection should be initialized in each resx file to match these values with that
        // file's culture. For example:
        //
        // AppResources.es-ES.resx
        //    ResourceLanguage's value should be "es-ES"
        //    ResourceFlowDirection's value should be "LeftToRight"
        //
        // AppResources.ar-SA.resx
        //     ResourceLanguage's value should be "ar-SA"
        //     ResourceFlowDirection's value should be "RightToLeft"
        //
        // For more info on localizing Windows Phone apps see http://go.microsoft.com/fwlink/?LinkId=262072.
        //
        private void InitializeLanguage()
        {
            try
            {
                // Set the font to match the display language defined by the
                // ResourceLanguage resource string for each supported language.
                //
                // Fall back to the font of the neutral language if the Display
                // language of the phone is not supported.
                //
                // If a compiler error is hit then ResourceLanguage is missing from
                // the resource file.
                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

                // Set the FlowDirection of all elements under the root frame based
                // on the ResourceFlowDirection resource string for each
                // supported language.
                //
                // If a compiler error is hit then ResourceFlowDirection is missing from
                // the resource file.
                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {
                // If an exception is caught here it is most likely due to either
                // ResourceLangauge not being correctly set to a supported language
                // code or ResourceFlowDirection is set to a value other than LeftToRight
                // or RightToLeft.

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw;
            }
        }
            
        // Helper method for adding or updating a key/value pair in isolated storage
        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (settings.Contains(Key))
            {
                // If the value has changed
                if (settings[Key] != value)
                {
                    // Store the new value
                    settings[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                settings.Add(Key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        // Helper method for removing a key/value pair from isolated storage
        public void RemoveValue(string Key)
        {
            // If the key exists
            if (settings.Contains(Key))
            {
                settings.Remove(Key);
            }
        }

        // Called when the app is deactivating. Saves the time of the deactivation and the 
        // session type of the app instance to isolated storage.
        public void SaveCurrentDeactivationSettings()
        {
            if (AddOrUpdateValue("DeactivateTime", DateTimeOffset.Now))
            {
                settings.Save();
            }

            if (AddOrUpdateValue("SessionType", sessionType))
            {
                settings.Save();
            }

        }

        // Called when the app is launched or closed. Removes all deactivation settings from
        // isolated storage
        public void RemoveCurrentDeactivationSettings()
        {
            RemoveValue("DeactivateTime");
            RemoveValue("SessionType");
            settings.Save();
        }

        // Helper method to determine if the interval since the app was deactivated is
        // greater than 15 minutes
        bool CheckDeactivationTimeStamp()
        {
            DateTimeOffset lastDeactivated;
           
            if (settings.Contains("DeactivateTime"))
            {
                lastDeactivated = (DateTimeOffset)settings["DeactivateTime"];
            }

            var currentDuration = DateTimeOffset.Now.Subtract(lastDeactivated);

            return TimeSpan.FromSeconds(currentDuration.TotalMinutes) > TimeSpan.FromMinutes(15);
        }

        // Helper method to restore the session type from isolated storage.
        void RestoreSessionType()
        {
            if (settings.Contains("SessionType"))
            {
                sessionType = (SessionType)settings["SessionType"];
            }
        }
    }
}