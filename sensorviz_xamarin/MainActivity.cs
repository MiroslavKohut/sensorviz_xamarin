using System;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Hardware;
using Android.Content.PM;

using Android.Content;
using Android.Runtime;


namespace TextureViewCameraStream
{
    [Activity(Label = "TextureViewCameraStream", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
    ScreenOrientation = ScreenOrientation.Landscape)]
    public class Activity1 : Activity, TextureView.ISurfaceTextureListener
    {
        Camera camera;
        TextureView textureView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.RequestFeature(WindowFeatures.NoTitle);
            textureView = new TextureView(this);
            textureView.SurfaceTextureListener = this;

            camera = Camera.Open();

           // _textureView.LayoutParameters = new FrameLayout.LayoutParams(w, h);

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
