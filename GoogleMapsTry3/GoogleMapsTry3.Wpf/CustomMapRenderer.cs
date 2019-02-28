using GoogleMapsTry3;
using GoogleMapsTry3.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.WPF;
using Xamarin.Forms.Platform.WPF;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace GoogleMapsTry3.Wpf
{
	 class CustomMapRenderer : MapRenderer
	 {
		  //Microsoft.Maps.MapControl.WPF.Map nativeMap;
		  List<CustomPin> customPins;

		  CustomCircle circle;

		  protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
		  {
				base.OnElementChanged(e);

				if(e.OldElement != null)
				{
					 // Unsubscribe
				}

				if(e.NewElement != null)
				{
					 var formsMap = (CustomMap)e.NewElement;
					 circle = formsMap.Circle;
					 //Control.GetMapAsync(this);
				}
		  }

		 /* protected override void Appearing()
		  {
				base.Appearing();
		  }

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
