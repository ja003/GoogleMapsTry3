using SQLite;
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

				customMap.LogPosition(customMap.DebugPosition);
				//customMap.LoggedPositions.Log(customMap.DebugPosition);
				//customMap.LoggedPositions = customMap.LoggedPositions;

		  }

		  private void OnBtnPlusClicked(object sender, EventArgs e)
		  {
				customMap.IncreaseGridStepSize();
		  }

		  private void OnBtnMinusClicked(object sender, EventArgs e)
		  {
				customMap.DecreaseGridStepSize();
		  }

	 }

	 public enum Direction
	 {
		  Up,
		  Right,
		  Down,
		  Left
	 }
}