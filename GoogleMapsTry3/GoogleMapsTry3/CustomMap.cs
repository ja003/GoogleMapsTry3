using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GoogleMapsTry3
{
	 public class CustomMap : Map
	 {
		  private const float STEP_INCREMENT = 0.001f;

		  private const string GridCenterKey = "GridCenter";
		  public Position? GridCenter
		  {
				get
				{
					 if(Application.Current.Properties.ContainsKey(GridCenterKey))
						  return (Position)Application.Current.Properties[GridCenterKey];
					 return null;
				}
				set
				{
					 Application.Current.Properties[GridCenterKey] = value;
				}
		  }

		  //public List<CustomPin> CustomPins { get; set; }
		  //public CustomCircle Circle { get; set; }
		  //public List<Position> ShapeCoordinates { get; set; }
		  //public Action<Action> OnMapReady { get; internal set; }
		  public List<GridLine> GridLines;

		  public Position DebugPosition
		  {
				get
				{
					 return (Position)GetValue(DebugPositionProperty);
				}
				set
				{
					 SetValue(DebugPositionProperty, value);
				}
		  }
		  public static BindableProperty DebugPositionProperty = BindableProperty.Create(nameof(DebugPosition), typeof(Position), typeof(CustomMap));

		  public float GridStepSize
		  {
				get
				{
					 return (float)GetValue(GridStepSizeProperty);
				}
				set
				{
					 SetValue(GridStepSizeProperty, value);
				}
		  }

		  public CustomMap()
		  {
				//ShapeCoordinates = new List<Position>();
		  }

		  public static BindableProperty GridStepSizeProperty =
				BindableProperty.Create(nameof(GridStepSize), typeof(float), typeof(CustomMap));


		  /*private static void OnGridStepSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		  {
				((CustomMap)bindable).OnGridStepSizePropertyChanged((float)oldValue, (float)newValue);
		  }

		  private void OnGridStepSizePropertyChanged(float oldValue, float newValue)
		  {
				//GenerateGrid();
		  }*/

		  public void GenerateGrid()
		  {
				if(this.GridCenter != null)
				{
					 this.GridLines = GenerateGridLines();
					 if(PropertyChanged != null)
					 {
						  PropertyChanged(this,
								new PropertyChangedEventArgs(nameof(GridStepSizeProperty)));
					 }
				}
		  }

		  private List<GridLine> GenerateGridLines()
		  {
				System.Diagnostics.Debug.Write("@@@@@ DrawGrid2");
				int steps = 10;
				//float stepSize = 0.01f;

				List<GridLine> lines = new List<GridLine>();

				if(GridCenter == null)
				{
					 return null;
				}
				Position center = (Position)GridCenter;

				double longitude = center.Longitude + steps * GridStepSize; //top
				double latitude = center.Latitude;
				for(int x = -steps; x < steps; x++)
				{
					 latitude = center.Latitude + x * GridStepSize;

					 lines.Add(new GridLine(new Position(latitude, longitude), new Position(latitude, longitude - 2 * steps * GridStepSize)));

				}

				latitude = center.Latitude - steps * GridStepSize; //left
				for(int y = -steps; y < steps; y++)
				{
					 longitude = center.Longitude + y * GridStepSize;

					 lines.Add(new GridLine(new Position(latitude, longitude), new Position(latitude + 2 * steps * GridStepSize, longitude)));
				}

				return lines;
		  }

		  internal void IncreaseGridStepSize()
		  {
				GridStepSize += STEP_INCREMENT;
		  }
		  internal void DecreaseGridStepSize()
		  {
				GridStepSize -= STEP_INCREMENT;
				GridStepSize = Math.Max(0, GridStepSize);
		  }

		  public event PropertyChangedEventHandler PropertyChanged;




	 }
}
