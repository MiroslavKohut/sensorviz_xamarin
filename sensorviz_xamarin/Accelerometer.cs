using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Android.Hardware;

using OxyPlot.Xamarin.Android;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;

namespace sensorviz_xamarin
{


    class AccelerometerView : View, Android.Hardware.ISensorEventListener
    {

        PlotView view;
        float time = 0.1F;
        static readonly object syncLock = new object();
        SensorManager _sensorManager;
        PlotModel plotModel;

        LinearLayout accLayout;

        public AccelerometerView(LinearLayout layout) : base(layout.Context)
        {

            accLayout = layout;
            view = new PlotView(layout.Context);
            layout.AddView(view);
            CreatePlotModel();
         }

         private void CreatePlotModel()
         {
            plotModel = new PlotModel { Title = "Accelerometer" };

            //generate a random percentage distribution between the 5
            //cake-types (see axis below)
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Maximum = 10,
                Minimum = -10,
                IsPanEnabled = false,
                IsZoomEnabled = false

            });

            plotModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "CakeAxis",
                AxisDistance = 0,
                IsZoomEnabled = false,
                IsPanEnabled = false,

                ItemsSource = new[]
                    {
                "Z",
                "Y",
                "X"
                }
            });
            view.Model = plotModel;
         }


         public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
            {
                // Do nothing
            }

         public void OnSensorChanged(SensorEvent e)
         {

            lock (syncLock)
            {
                plotModel.Series.Clear();
                var barSeries = new BarSeries
                {
                    ItemsSource = new List<BarItem>(new[]
                 {
                new BarItem{ Value = (e.Values[2]), Color = OxyColors.Red},
                new BarItem{ Value = (e.Values[1]), Color = OxyColors.Green},
                new BarItem{ Value = (e.Values[0]), Color = OxyColors.Blue}}),

                    LabelPlacement = LabelPlacement.Inside,
                    LabelFormatString = "{0:.00} m/s^-2"
                };
                plotModel.Series.Add(barSeries);
                view.InvalidatePlot(true);
            }
        }

    }
}

