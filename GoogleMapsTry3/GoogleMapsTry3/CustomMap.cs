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

		  //public List<CustomPin> CustomPins { get; set; }
		  //public CustomCircle Circle { get; set; }
		  //public List<Position> ShapeCoordinates { get; set; }
		  //public Action<Action> OnMapReady { get; internal set; }
		  public List<GridLine> GridLines;

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

		  public Position GridCenter { get; set; }
		  public static BindableProperty GridStepSizeProperty =
				BindableProperty.Create(nameof(GridStepSize), typeof(float), typeof(CustomMap), null,
					 propertyChanged: OnGridStepSizePropertyChanged);

		  private static void OnGridStepSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		  {
				((CustomMap)bindable).OnGridStepSizePropertyChanged((float)oldValue, (float)newValue);
		  }

		  private void OnGridStepSizePropertyChanged(float oldValue, float newValue)
		  {
				//GenerateGrid();
		  }

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


				double longitude = GridCenter.Longitude + steps * GridStepSize; //top
				double latitude = GridCenter.Latitude;
				for(int x = -steps; x < steps; x++)
				{
					 latitude = GridCenter.Latitude + x * GridStepSize;

					 lines.Add(new GridLine(new Position(latitude, longitude), new Position(latitude, longitude - 2 * steps * GridStepSize)));

				}

				latitude = GridCenter.Latitude - steps * GridStepSize; //left
				for(int y = -steps; y < steps; y++)
				{
					 longitude = GridCenter.Longitude + y * GridStepSize;

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
