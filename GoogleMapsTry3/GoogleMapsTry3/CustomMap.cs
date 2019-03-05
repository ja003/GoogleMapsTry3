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
				LoggedPositions.Add(position);
				MessagingCenter.Send(this, "LogPosition");
		  }

		  /*public LoggedPositions LoggedPositions
		  {
				get
				{
					 return (LoggedPositions)GetValue(LoggedPositionsProperty);
				}
				set
				{
					 SetValue(LoggedPositionsProperty, value);
				}
		  }*/

		  /*public void LogPosition(Position position)
		  {
				LoggedPositions = new ObservableCollection<Position>();
				LoggedPositions.Add(position);
				//PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoggedPositionsProperty)));
		  }*/

		  //public static BindableProperty LoggedPositionsProperty = BindableProperty.Create(nameof(LoggedPositions), typeof(LoggedPositions), typeof(CustomMap),
		  //null);

		  /*public static void OnLoggedPositionsChanged(BindableObject bindable, object oldValue, object newValue)
		  {
				CustomMap bindable1 = ((CustomMap)bindable);
				if(bindable1.PropertyChanged == null)
					 return;
				bindable1.PropertyChanged(bindable1, new PropertyChangedEventArgs(nameof(LoggedPositionsProperty)));

				//((CustomMap)bindable).OnPolyLineAddressPointsPropertyChanged((IList<string>)oldValue, (IList<string>)newValue);
		  }*/
		  /*void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		  {
				//Here do what you need to do when the collection change
		  }*/


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
				//LoggedPositions = new ObservableCollection<Position>();
				//LoggedPositions.CollectionChanged += Items_CollectionChanged;
				//LoggedPositions = new LoggedPositions();
				//LoggedPositionsProperty.
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

				Position topLeft = new Position(center.Latitude + index.Item1 * GridStepSize, center.Longitude + index.Item2 * GridStepSize);
				Position botRight = new Position(center.Latitude + (index.Item1 - 1) * GridStepSize, center.Longitude + (index.Item2 - 1) * GridStepSize);

				return new Tuple<Position, Position>(topLeft, botRight);
		  }

		  private Tuple<int, int> GetIndexInGrid(Position position)
		  {
				if(GridCenter == null)
					 return null;

				Position center = (Position)GridCenter;
				int x = (int)((position.Latitude - center.Latitude) / GridStepSize);
				int y = (int)((position.Longitude - center.Longitude) / GridStepSize);

				System.Diagnostics.Debug.Write($"@@@@@ GetIndexInGrid {position} = [{x},{y}]");

				return new Tuple<int, int>(x, y);
		  }

		  //public new event PropertyChangedEventHandler PropertyChanged;




	 }
}
