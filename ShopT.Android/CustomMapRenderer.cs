using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using ShopT.Droid;
using ShopT.Models.Maps;
using ShopT.StaticValues;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace ShopT.Droid
{
    public class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter
    {
        List<CustomPin> customPins;

        public CustomMapRenderer(Context context) : base(context)
        {
        }
        
        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                NativeMap.InfoWindowClick -= OnInfoWindowClick;
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                customPins = formsMap.CustomPins;
            }
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            NativeMap.InfoWindowClick += OnInfoWindowClick;
            NativeMap.SetInfoWindowAdapter(this);
        }

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.periscope));
            return marker;
        }

        void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            //var customPin = GetCustomPin(e.Marker);
            //if (customPin == null)
            //{
            //    throw new Exception("Custom pin not found");
            //}

            //if (!string.IsNullOrWhiteSpace(customPin.Url))
            //{
            //    var url = Android.Net.Uri.Parse(customPin.Url);
            //    var intent = new Intent(Intent.ActionView, url);
            //    intent.AddFlags(ActivityFlags.NewTask);
            //    Android.App.Application.Context.StartActivity(intent);
            //}
        }

        public Android.Views.View GetInfoContents(Marker marker)
        {
            var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
            if (inflater != null)
            {
                Android.Views.View view;

                var customPin = GetCustomPin(marker);
                if (customPin == null)
                {
                    throw new Exception("Custom pin not found");
                }

                view = inflater.Inflate(Resource.Layout.MapInfoWindow, null);

                var InfoImage = view.FindViewById<ImageView>(Resource.Id.InfoWindowImage);
                var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
                var infoAddress = view.FindViewById<TextView>(Resource.Id.InfoWindowAddress);
                
                if (InfoImage != null)
                {
                    var imageBitmap = GetImageBitmapFromUrl(customPin.Images);
                    InfoImage.SetImageBitmap(imageBitmap);
                }
                if (infoTitle != null)
                {
                    infoTitle.Text = marker.Title;
                }
                if (infoAddress != null)
                {
                    infoAddress.Text = customPin.Address;
                }

                return view;
            }
            return null;
        }

        public Android.Views.View GetInfoWindow(Marker marker)
        {
            return null;
        }

        CustomPin GetCustomPin(Marker annotation)
        {
            var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
            foreach (var pin in customPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new System.Net.WebClient())
            {
                byte[] imageBytes;
                try
                {
                    imageBytes = webClient.DownloadData(ApiStrings.SHOPT_HUB + ApiStrings.IMAGES_FOLDER + url);
                }
                catch (Exception)
                {

                    imageBytes = null;
                }
                //byte[] imageBytes = webClient.DownloadData(ApiStrings.SHOPT_HUB + ApiStrings.IMAGES_FOLDER + url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
    }
}