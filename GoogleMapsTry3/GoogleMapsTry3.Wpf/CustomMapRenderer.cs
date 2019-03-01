using GoogleMapsTry3;
using GoogleMapsTry3.Wpf;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.WPF;
using Xamarin.Forms.Platform.WPF;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace GoogleMapsTry3.Wpf
{
	 internal class CustomMapRenderer : MapRenderer
	 {
		  //Microsoft.Maps.MapControl.WPF.Map nativeMap;
		  private List<CustomPin> customPins;
		  private CustomCircle circle;

		  private CustomMap customMap;

		  protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
		  {
				base.OnElementChanged(e);

				if(e.OldElement != null)
				{
					 // Unsubscribe
				}

				if(e.NewElement != null)
				{
					 customMap = (CustomMap)e.NewElement;
					 circle = customMap.Circle;
					 //Control.GetMapAsync(this);
				}
		  }

		  protected override void Appearing()
		  {
				base.Appearing();
				//MoveToMyLocation();
		  }

		  /*

			protected override void OnMapReady(Android.Gms.Maps.GoogleMap map)
			{
				 base.OnMapReady(map);

				 var circleOptions = new CircleOptions();
				 circleOptions.InvokeCenter(new LatLng(circle.Position.Latitude, circle.Position.Longitude));
				 circleOptions.InvokeRadius(circle.Radius);
				 circleOptions.InvokeFillColor(0X66FF0000);
				 circleOptions.InvokeStrokeColor(0X66FF0000);
				 circleOptions.InvokeStrokeWidth(0);

				 NativeMap.AddCircle(circleOptions);
			}*/
	 }
}
