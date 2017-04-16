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
    class ProximityView : View, Android.Hardware.ISensorEventListener
    {
        LinearLayout proximityLayout;
        TextView proximityTextView;

        static readonly object _syncLock = new object();

       public ProximityView(LinearLayout layout) : base(layout.Context)
        {
            proximityLayout = layout;
            proximityTextView = new TextView(layout.Context);
            proximityTextView.Text = "proximity";
            // proximityTextView.SetBackgroundColor(Android.Graphics.Color.Yellow);
            proximityTextView.TextSize = (float)18;
            proximityTextView.SetTextColor(Android.Graphics.Color.Black);
            proximityLayout.SetBackgroundColor(Android.Graphics.Color.Yellow);
            layout.AddView(proximityTextView);
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {

        }

        public void OnSensorChanged(SensorEvent e)
        {
            lock (_syncLock)
            {
                proximityTextView.Text = string.Format("Brightness={0:f}", e.Values[0]);

                proximityLayout.Alpha = (float)0.05 * e.Values[0];

            }
            Invalidate();

        }
    }
}