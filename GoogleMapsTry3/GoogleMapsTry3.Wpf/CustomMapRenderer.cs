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
		  Microsoft.Maps.MapControl.WPF.Map nativeMap;
		  List<CustomPin> customPins;

		  protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
		  {
				base.OnElementChanged(e);

				if(e.OldElement != null)
				{

					 //nativeMap.InfoWindowClick -= OnInfoWindowClick;
				}
		  }
	 }
}
