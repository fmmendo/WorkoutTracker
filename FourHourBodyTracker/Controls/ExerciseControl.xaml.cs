using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WorkoutLib;

namespace FourHourBodyTracker.Controls
{
    public partial class ExerciseControl : UserControl
    {
        //public StrengthExercise Exercise
        //{
        //    get { return (StrengthExercise)GetValue(ExerciseProperty); }
        //    set { SetValue(ExerciseProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for Exercise.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ExerciseProperty =
        //    DependencyProperty.Register("Exercise", typeof(StrengthExercise), typeof(ExerciseControl), new PropertyMetadata(null));

        //public string ExerciseName
        //{
        //    get { return (string)GetValue(ExerciseNameProperty); }
        //    set { SetValue(ExerciseNameProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for ExerciseName.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ExerciseNameProperty =
        //    DependencyProperty.Register("ExerciseName", typeof(string), typeof(ExerciseControl), new PropertyMetadata(""));

        public ExerciseControl()
        {

            //DataContext = this;
            InitializeComponent();
        }
    }
}
