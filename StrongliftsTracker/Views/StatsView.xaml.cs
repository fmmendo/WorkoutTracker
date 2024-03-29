﻿using Microsoft.Phone.Controls;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls;
using WorkoutLib.ViewModel;

namespace StrongliftsTracker.Views
{
    public partial class StatsView : PhoneApplicationPage
    {
        private List<Color> Colours = new List<Color> 
        { 
            Colors.Blue,
            Colors.Orange,
            Colors.Green,
            Colors.Red,
            Colors.Yellow,
            Colors.Purple,
            Colors.White,
            Colors.Brown,
            Colors.Cyan,
            Colors.DarkGray };

        StatsViewModel _vm;

        public StatsView()
        {
            InitializeComponent();
            
            _vm = new StatsViewModel();
            DataContext = _vm;

            legend.Children.Clear();
            int count = 0;
            foreach (var item in _vm.ExerciseProgression)
            {
                var series = new LineSeries();
                series.ItemsSource = item.Value;
                series.CategoryBinding = new PropertyNameDataPointBinding("Date");
                series.ValueBinding = new PropertyNameDataPointBinding("Weight");
                series.Stroke = new SolidColorBrush(Colours[count]);
                this.chart.Series.Add(series);

                TextBlock tb = new TextBlock();
                tb.Text = item.Key;
                tb.Foreground = new SolidColorBrush(Colours[count]);
                legend.Children.Add(tb);

                count++;
            }
        }
    }
}