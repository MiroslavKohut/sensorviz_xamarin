using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Hardware;
using Android.Graphics;

namespace sensorviz_xamarin
{
    class CompassView : View, Android.Hardware.ISensorEventListener
    {
        LinearLayout compassLayout;

        private Paint mPaint = new Paint();
            private Path mPath = new Path();
            private float[] data;
        //private Compass compass;

            public CompassView(LinearLayout layout) : base(layout.Context)
            {
            //      compass = (Compass)context;

                 compassLayout = layout;
                // Construct a wedge-shaped path
                mPath.MoveTo(0, -50);
                mPath.LineTo(-20, 60);
                mPath.LineTo(0, 50);
                mPath.LineTo(20, 60);
                mPath.Close();
            compassLayout.AddView(this);
            compassLayout.Alpha = (float)0.5;
            


        }

        protected override void OnDraw(Canvas canvas)
            {
                Paint paint = mPaint;

                canvas.DrawColor(Color.White);

                paint.AntiAlias = true;
                paint.Color = Color.Black;
                paint.SetStyle(Paint.Style.Fill);

                int w = canvas.Width;
                int h = canvas.Height;
                int cx = w / 2;
                int cy = h / 2;

                canvas.Translate(cx, cy);

                if (data != null)
                    canvas.Rotate(-data[0]);

                canvas.DrawPath(mPath, mPaint);
          
            }

       
            public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
            {
                // Do nothing
            }

            public void OnSensorChanged(SensorEvent e)
            {
                data = e.Values.ToArray();

                Invalidate();
            }

    }
}

