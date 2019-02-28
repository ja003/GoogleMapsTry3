using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GoogleMapsTry3
{
	 public class MapPageCS : ContentPage
	 {
		  //alternative..
		  public MapPageCS()
		  {
				var customMap = new CustomMap
				{
					 MapType = MapType.Street,
					 WidthRequest = App.ScreenWidth,
					 HeightRequest = App.ScreenHeight
				};

				var pin = new CustomPin
				{
					 Type = PinType.Place,
					 Position = new Position(37.79752, -122.40183),
					 Label = "Xamarin San Francisco Office",
					 Address = "394 Pacific Ave, San Francisco CA",
					 Id = "Xamarin",
					 Url = "http://xamarin.com/about/"

				};

				customMap.ShapeCoordinates.Add(new Position(37.797513, -122.402058));
				customMap.ShapeCoordinates.Add(new Position(37.798433, -122.402256));
				customMap.ShapeCoordinates.Add(new Position(37.798582, -122.401071));
				customMap.ShapeCoordinates.Add(new Position(37.797658, -122.400888));

				customMap.CustomPins = new List<CustomPin> { pin };
				customMap.Pins.Add(pin);
				customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(37.79752, -122.40183), Distance.FromMiles(1.0)));

				Content = customMap;
		  }
	 }
}
