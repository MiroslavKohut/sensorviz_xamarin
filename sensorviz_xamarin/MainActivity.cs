﻿using System;

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
using sensorviz_xamarin;

namespace TextureViewCameraStream
{
    [Activity(Label = "TextureViewCameraStream", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
    ScreenOrientation = ScreenOrientation.Landscape)]
    public class Activity1 : Activity, TextureView.ISurfaceTextureListener
    {
        CompassView compass;
        ProximityView proximity;
        LightView light;
        AccelerometerView accelerometer;

        SensorManager sensorService;

        Sensor proximitySensor;
        Sensor lightSensor;
        Sensor ori;
        Sensor acc;

        Camera camera;

        TextureView textureView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.RequestFeature(WindowFeatures.NoTitle);

            SetContentView(sensorviz_xamarin.Resource.Layout.Main);

            //add camera texture view
            textureView = FindViewById<TextureView>(sensorviz_xamarin.Resource.Id.textureView1);
            textureView.SurfaceTextureListener = this;

            //add proximity layout 
            LinearLayout lightLayout = FindViewById<LinearLayout>(sensorviz_xamarin.Resource.Id.light);
            light = new LightView(lightLayout);

            //add proximity layout 
            LinearLayout proximityLayout = FindViewById<LinearLayout>(sensorviz_xamarin.Resource.Id.proximity);
            proximity = new ProximityView(proximityLayout);

            //add compass layout
            LinearLayout compassLayout = FindViewById<LinearLayout>(sensorviz_xamarin.Resource.Id.compass);
            compass = new CompassView(compassLayout);

            //add accelerometer layout
            LinearLayout accelerometerLayout = FindViewById<LinearLayout>(sensorviz_xamarin.Resource.Id.accelerometer);
            accelerometer = new AccelerometerView(accelerometerLayout);

            //get Sensor manager
            sensorService = (SensorManager)GetSystemService(Context.SensorService);

            proximitySensor = sensorService.GetDefaultSensor(SensorType.Proximity);
            lightSensor = sensorService.GetDefaultSensor(SensorType.Light); // Get a Light Sensor
            ori = sensorService.GetDefaultSensor(SensorType.Orientation);    //Get orientation
            acc = sensorService.GetDefaultSensor(SensorType.Accelerometer);    //Get orientation

            // Register a listeners
            sensorService.RegisterListener(proximity, proximitySensor, Android.Hardware.SensorDelay.Normal);
            sensorService.RegisterListener(light, lightSensor, Android.Hardware.SensorDelay.Game);
            sensorService.RegisterListener(compass, ori, SensorDelay.Fastest);
            sensorService.RegisterListener(accelerometer, acc, SensorDelay.Fastest);

        }

        protected override void OnResume()
        {
            base.OnResume();

            // Register a listeners
            sensorService.RegisterListener(proximity, proximitySensor, Android.Hardware.SensorDelay.Normal);
            sensorService.RegisterListener(light, lightSensor, Android.Hardware.SensorDelay.Game);
            sensorService.RegisterListener(compass, ori, SensorDelay.Fastest);
            sensorService.RegisterListener(accelerometer, acc, SensorDelay.Fastest);
        }

        protected override void OnPause()
        {
            base.OnPause();
            sensorService.UnregisterListener(proximity);
            sensorService.UnregisterListener(light);
            sensorService.UnregisterListener(compass);
            sensorService.UnregisterListener(accelerometer);

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

    }
}