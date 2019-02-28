using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace GoogleMapsTry3
{
	 [XamlCompilation(XamlCompilationOptions.Compile)]
	 public partial class MapPage : ContentPage
	 {
		  public MapPage()
		  {
				InitializeComponent();

				var pin = new CustomPin
				{
					 Type = PinType.Place,
					 Position = new Position(37.79752, -122.40183),
					 Label = "Xamarin San Francisco Office",
					 Address = "394 Pacific Ave, San Francisco CA",
					 Id = "Xamarin",
					 Url = "http://xamarin.com/about/"
				};

				var position = new Position(37.79752, -122.40183);
				customMap.Circle = new CustomCircle
				{
					 Position = position,
					 Radius = 1000
				};

				customMap.CustomPins = new List<CustomPin> { pin };
				customMap.Pins.Add(pin);
				//customMap.MoveToRegion(MapSpan.FromCenterAndRadius(
				  //new Position(37.79752, -122.40183), Distance.FromMiles(1.0)));
				customMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(1.0)));


		  }
	 }
}