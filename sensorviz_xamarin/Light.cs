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
using Android.Media;

namespace sensorviz_xamarin
{

    class LightView : View, Android.Hardware.ISensorEventListener
    {
        LinearLayout lightLayout;
        TextView lightTextView;
        MediaPlayer player;
        const float lowLimit = 25;
        const float highLimit = 3500;

        static readonly object _syncLock = new object();

       public LightView(LinearLayout layout) : base(layout.Context)
        {
            lightLayout = layout;
            lightTextView = new TextView(layout.Context);

            lightTextView.Text = "light";
            lightTextView.TextSize = (float)16;
            lightTextView.SetTextColor(Android.Graphics.Color.Black);

            lightLayout.SetBackgroundColor(Android.Graphics.Color.Yellow);
            
            layout.AddView(lightTextView);

            player = MediaPlayer.Create(layout.Context, Resource.Drawable.sound);

        }

        ~LightView()
        {
            player.Stop();
           
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {

        }

        public void OnSensorChanged(SensorEvent e)
        {
           
            lock (_syncLock)
            {
                lightTextView.Text = string.Format("Brightness={0:f}", e.Values[0]);

                lightLayout.Alpha = (float)0.001 * e.Values[0];

                float value = (float)e.Values[0];

                if (value < lowLimit || value > highLimit)
                {
                    if (!player.IsPlaying)
                        player.Start();
                }
                else
                {
                   
                   if (player.IsPlaying)
                        player.Pause();
                }
            }
            Invalidate();

        }
    }
}