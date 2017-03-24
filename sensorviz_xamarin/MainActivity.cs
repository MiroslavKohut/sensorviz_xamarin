using System;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Hardware.Camera2;
using Android.Hardware;
using Android.Content.PM;
//using Android.Graphics;



using Android.Content;
using Android.Runtime;

namespace TextureViewCameraStream
{
    [Activity(Label = "TextureViewCameraStream", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
    ScreenOrientation = ScreenOrientation.Landscape)]
    public class Activity1 : Activity, TextureView.ISurfaceTextureListener, Android.Hardware.ISensorEventListener
    {
        Camera camera;
        TextureView textureView;
        FrameLayout mainLayout;

        static readonly object _syncLock = new object();
        TextView proximityTextView;
        LinearLayout proximityLayout;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.RequestFeature(WindowFeatures.NoTitle);
            textureView = new TextureView(this);
            textureView.SurfaceTextureListener = this;

            camera = Camera.Open();

            var sensorService = (SensorManager)GetSystemService(Context.SensorService);

            // Get a Light Sensor
            var lightSensor = sensorService.GetDefaultSensor(SensorType.Light);

            // Register this class a listener for light sensor
            sensorService.RegisterListener(this, lightSensor, Android.Hardware.SensorDelay.Game);


            try
            {
                camera.SetPreviewTexture(textureView.SurfaceTexture);
                camera.StartPreview();

            }
            catch (Java.IO.IOException ex)
            {
                Console.WriteLine(ex.Message);
            }



            SetContentView(sensorviz_xamarin.Resource.Layout.Main);
            textureView = FindViewById<TextureView>(sensorviz_xamarin.Resource.Id.textureView1);
            textureView.SurfaceTextureListener = this;
           
            //add proximity layout
            proximityLayout = FindViewById<LinearLayout>(sensorviz_xamarin.Resource.Id.proximity);

            proximityTextView = new TextView(proximityLayout.Context);
            proximityTextView.Text = "proximity";
            proximityTextView.SetBackgroundColor(Android.Graphics.Color.Yellow);
            proximityTextView.SetTextColor(Android.Graphics.Color.DarkGray);

            proximityLayout.AddView(proximityTextView);


            mainLayout = FindViewById<FrameLayout>(sensorviz_xamarin.Resource.Id.mainLayout1);
        }

        public void OnSurfaceTextureAvailable(Android.Graphics.SurfaceTexture surface, int w, int h)
        {
            camera = Camera.Open();

            textureView.LayoutParameters = new FrameLayout.LayoutParams(w, h);

            try
            {
                camera.SetPreviewTexture(surface);
                camera.StartPreview();

            }
            catch (Java.IO.IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool OnSurfaceTextureDestroyed(Android.Graphics.SurfaceTexture surface)
        {
            camera.StopPreview();
            camera.Release();

            return true;
        }

        public void OnSurfaceTextureSizeChanged(Android.Graphics.SurfaceTexture surface, int width, int height)
        {
            // camera takes care of this
        }

        public void OnSurfaceTextureUpdated(Android.Graphics.SurfaceTexture surface)
        {

        }


        public void OnSensorChanged(SensorEvent s)
        {
            lock (_syncLock)
            {
                proximityTextView.Text = string.Format("Brightness={0:f}", s.Values[0]);
              
                proximityLayout.Alpha = (float)0.05 * s.Values[0];
                
            }
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            // Your processing here
        }


    }
}