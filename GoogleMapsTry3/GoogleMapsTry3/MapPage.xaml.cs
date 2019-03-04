using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace GoogleMapsTry3
{
	 [XamlCompilation(XamlCompilationOptions.Compile)]
	 public partial class MapPage : ContentPage
	 {
		  private const float DEBUG_POS_INCREMENT = 0.001f;

		  public MapPage()
		  {
				InitializeComponent();

				btnPlus.Clicked += OnBtnPlusClicked;
				btnMinus.Clicked += OnBtnMinusClicked;

				btnUp.Clicked += (sender, ea) => { MoveDebugPosition(Direction.Up); };
				btnRight.Clicked += (sender, ea) => { MoveDebugPosition(Direction.Right); };
				btnDown.Clicked += (sender, ea) => { MoveDebugPosition(Direction.Down); };
				btnLeft.Clicked += (sender, ea) => { MoveDebugPosition(Direction.Left); };
		  }

		  private void MoveDebugPosition(Direction direction)
		  {
				float latitudeIncrement = 0;
				float longitudeIncrement = 0;
				switch(direction)
				{
					 case Direction.Up:
						  latitudeIncrement = DEBUG_POS_INCREMENT;
						  break;
					 case Direction.Down:
						  latitudeIncrement = -DEBUG_POS_INCREMENT;
						  break;
					 case Direction.Right:
						  longitudeIncrement = DEBUG_POS_INCREMENT;
						  break;
					 case Direction.Left:
						  longitudeIncrement = -DEBUG_POS_INCREMENT;
						  break;
				}
				customMap.DebugPosition = new Position(customMap.DebugPosition.Latitude + latitudeIncrement, customMap.DebugPosition.Longitude + longitudeIncrement);

		  }

		  private void OnBtnPlusClicked(object sender, EventArgs e)
		  {
				customMap.IncreaseGridStepSize();
		  }

		  private void OnBtnMinusClicked(object sender, EventArgs e)
		  {
				customMap.DecreaseGridStepSize();
		  }


		  /*private void OnMapReady(Action pOnMoveToLocation)
		  {
				MoveToMyLocation(pOnMoveToLocation);
		  }*/

		  /*private async void MoveToMyLocation(Action pOnMoveToLocation)
		  {
				//Log.WriteLine($"MoveToMyLocation");


				var locator = CrossGeolocator.Current;
				Plugin.Geolocator.Abstractions.Position position = await locator.GetPositionAsync();
				customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(1)));
				//Debug.Write($"position = {position}");

				Position userPosition = new Position(position.Latitude, position.Longitude);

				customMap.GridCenter = new Position(userPosition.Latitude, userPosition.Longitude);


				pOnMoveToLocation?.Invoke();
		  }*/


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

	 public enum Direction
	 {
		  Up,
		  Right,
		  Down,
		  Left
	 }
}