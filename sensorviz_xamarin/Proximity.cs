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
        ImageView imageView;


       public ProximityView(LinearLayout layout) : base(layout.Context)
        {
            proximityLayout = layout;
            proximityTextView = new TextView(layout.Context);
            imageView = new ImageView(layout.Context);

            proximityTextView.Text = "proximity";
            proximityTextView.TextSize = (float)18;
            proximityTextView.SetTextColor(Android.Graphics.Color.Black);

            layout.AddView(proximityTextView);
            layout.AddView(imageView);

        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {

        }

        public void OnSensorChanged(SensorEvent e)
        {
            lock (_syncLock)
            {
                if (e.Values[0] > 0)
                {
                    imageView.SetImageResource(Resource.Drawable.light_on);
                }
                else
                {
                    imageView.SetImageResource(Resource.Drawable.light_off);
                }
            }
            Invalidate();

        }
    }
}