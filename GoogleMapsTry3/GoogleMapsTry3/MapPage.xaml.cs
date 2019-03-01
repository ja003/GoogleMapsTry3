using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
				customMap.CustomPins = new List<CustomPin> { pin };
				customMap.Pins.Add(pin);

				//customMap.ShapeCoordinates.Add(new Position(37.797513, -122.402058));
				//customMap.ShapeCoordinates.Add(new Position(37.798433, -122.402256));
				//customMap.ShapeCoordinates.Add(new Position(37.798582, -122.401071));
				//customMap.ShapeCoordinates.Add(new Position(37.797658, -122.400888));

				/*var polygonOptions = new PolygonOptions();
				polygonOptions.InvokeFillColor(0x66FF0000);
				polygonOptions.InvokeStrokeColor(0x660000FF);
				polygonOptions.InvokeStrokeWidth(30.0f);

				foreach(var position in shapeCoordinates)
				{
					 polygonOptions.Add(new LatLng(position.Latitude, position.Longitude));
				}
				NativeMap.AddPolygon(polygonOptions);*/



				var position = new Position(37.79752, -122.40183);
				customMap.Circle = new CustomCircle
				{
					 Position = position,
					 Radius = 1000
				};

				//customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(37.79752, -122.40183), Distance.FromMiles(1.0)));

				//MoveToMyLocation();
				/*

				
				customMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(1.0)));*/

				customMap.OnMapReady += OnMapReady;
		  }

		  private void OnMapReady(Action pOnMoveToLocation)
		  {
				MoveToMyLocation(pOnMoveToLocation);
		  }

		  private async void MoveToMyLocation(Action pOnMoveToLocation)
		  {
				//Log.WriteLine($"MoveToMyLocation");


				var locator = CrossGeolocator.Current;
				Plugin.Geolocator.Abstractions.Position position = await locator.GetPositionAsync();
				customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(1)));
				//Debug.Write($"position = {position}");

				Position userPosition = new Position(position.Latitude, position.Longitude);

				customMap.GridCenter = new Position(userPosition.Latitude, userPosition.Longitude);


				pOnMoveToLocation?.Invoke();
		  }


		  /*private async void MoveToMyLocation()
		  {
				Debug.Write($"MoveToMyLocation");
				if(Device.RuntimePlatform == Device.WPF)
				{
					 Debug.Write($"WPF");
					 return;
				}

				var locator = CrossGeolocator.Current;
				Plugin.Geolocator.Abstractions.Position position = await locator.GetPositionAsync();
				customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(1)));
				Debug.Write($"position = {position}");

				Position userPosition = new Position(position.Latitude, position.Longitude);

				SetGrid(userPosition);
		  }

		  private void SetGrid(Position pCenter)
		  {
				customMap.GridCenter = new Position(pCenter.Latitude, pCenter.Longitude);
				//const int dist = 1;
				//customMap.ShapeCoordinates.Add(new Position(pCenter.Latitude - dist, pCenter.Longitude));
				//customMap.ShapeCoordinates.Add(new Position(pCenter.Latitude + dist, pCenter.Longitude));
				//customMap.ShapeCoordinates.Add(new Position(pCenter.Latitude, pCenter.Longitude - dist));
				//customMap.ShapeCoordinates.Add(new Position(pCenter.Latitude, pCenter.Longitude + dist));
		  }*/
	 }
}