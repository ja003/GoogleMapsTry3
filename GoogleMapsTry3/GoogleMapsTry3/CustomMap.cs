using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
					 //if(LoggedPositions == null)
					 //  LoggedPositions = new List<Position>();
					 //LoggedPositions.Add(value);
				}
		  }
		  public static BindableProperty DebugPositionProperty = BindableProperty.Create(nameof(DebugPosition), typeof(Position), typeof(CustomMap));

		  public List<Position> LoggedPositions = new List<Position>();

		  public void LogPosition(Position position)
		  {
				LoggedPositions.Clear();
				LoggedPositions.Add(position);
				MessagingCenter.Send(this, "LogPosition");
		  }
		  

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
					 //if(PropertyChanged != null) sračka?
					 //{
						//  PropertyChanged(this,
						//		new PropertyChangedEventArgs(nameof(GridStepSizeProperty)));
					 //}
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

				double latitude = center.Latitude; 
				double longitude = center.Longitude - steps * GridStepSize; //left
				//lines from left to right
				for(int y = -steps; y < steps; y++)
				{
					 latitude = center.Latitude + y * GridStepSize;

					 lines.Add(new GridLine(new Position(latitude, longitude), new Position(latitude, longitude + 2 * steps * GridStepSize)));

				}

				latitude = center.Latitude + steps * GridStepSize; //top
				//lines from top to bottom
				for(int x = -steps; x < steps; x++)
				{
					 longitude = center.Longitude + x * GridStepSize;

					 lines.Add(new GridLine(new Position(latitude, longitude), new Position(latitude - 2 * steps * GridStepSize, longitude)));
				}

				return lines;
		  }

		  public const float MIN_GRID_STEP_SIZE = 0.001f;

		  internal void IncreaseGridStepSize()
		  {
				GridStepSize += STEP_INCREMENT;
		  }
		  internal void DecreaseGridStepSize()
		  {
				GridStepSize -= STEP_INCREMENT;
				GridStepSize = Math.Max(MIN_GRID_STEP_SIZE, GridStepSize);
		  }

		  public Tuple<Position, Position> GetAreaOnPosition(Position position)
		  {
				return GetAreaOnIndex(GetIndexInGrid(position));
		  }

		  private Tuple<Position, Position> GetAreaOnIndex(Tuple<int, int> index)
		  {
				if(GridCenter == null)
					 return null;
				Position center = (Position)GridCenter;

				int y = index.Item2 > 0 ? index.Item2 : index.Item2 + 1;
				int x = index.Item1 < 0 ? index.Item1 : index.Item1 - 1;
				Position topLeft = new Position(center.Latitude + y * GridStepSize, center.Longitude + x * GridStepSize);
				Position botRight = new Position(topLeft.Latitude - GridStepSize, topLeft.Longitude + GridStepSize);
				//Position botRight = new Position(center.Latitude + (index.Item2) * GridStepSize, center.Longitude + (index.Item1 - 1) * GridStepSize);

				return new Tuple<Position, Position>(topLeft, botRight);
		  }

		  /// <summary>
		  /// Item1 = x = longitude
		  /// Item2 = y = latitude
		  /// </summary>
		  /// <param name="position"></param>
		  /// <returns></returns>
		  public Tuple<int, int> GetIndexInGrid(Position position)
		  {
				if(GridCenter == null)
					 return null;

				Position center = (Position)GridCenter;
				//central coordinate system doesnt contain 0 index

				double xDouble = (position.Longitude - center.Longitude) / GridStepSize;
				int xSign = Math.Sign(xDouble);
				int x = (int)Math.Ceiling(Math.Abs(xDouble)) * xSign;

				double yDouble = (position.Latitude - center.Latitude) / GridStepSize;
				int ySign = Math.Sign(yDouble);
				int y = (int)Math.Ceiling(Math.Abs(yDouble)) * ySign;


				//if(Math.Abs(xDouble) < 1)
				//{
				//	 xDouble += Math.Sign(xDouble);
				//}
				//int x = (int)xDouble;

				//double yDouble = (position.Latitude - center.Latitude) / GridStepSize;
				//if(Math.Abs(yDouble) < 1)
				//{
				//	 yDouble += Math.Sign(yDouble);
				//}
				//int y = (int)yDouble;

				System.Diagnostics.Debug.Write($"@@@@@ GetIndexInGrid {position.Latitude},{position.Longitude} = [{x},{y}]");

				return new Tuple<int, int>(x, y);
		  }

		  //public new event PropertyChangedEventHandler PropertyChanged;




	 }
}
