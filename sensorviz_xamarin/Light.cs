using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace sensorviz_xamarin
{
    class LightView : View, Android.Hardware.ISensorEventListener
    {
        LinearLayout lightLayout;
        TextView lightTextView;

        static readonly object _syncLock = new object();

       public LightView(LinearLayout layout) : base(layout.Context)
        {
            lightLayout = layout;
            lightTextView = new TextView(layout.Context);
            lightTextView.Text = "proximity";
            // proximityTextView.SetBackgroundColor(Android.Graphics.Color.Yellow);
            lightTextView.TextSize = (float)18;
            lightTextView.SetTextColor(Android.Graphics.Color.Black);
            lightLayout.SetBackgroundColor(Android.Graphics.Color.Yellow);
            layout.AddView(lightTextView);
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {

        }

        public void OnSensorChanged(SensorEvent e)
        {
            lock (_syncLock)
            {
                lightTextView.Text = string.Format("Brightness={0:f}", e.Values[0]);

                lightLayout.Alpha = (float)0.05 * e.Values[0];

            }
            Invalidate();

        }
    }
}