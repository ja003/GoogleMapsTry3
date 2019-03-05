using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GoogleMapsTry3
{
	 public class CustomMap : Map
	 {
		  private const float STEP_INCREMENT = 0.001f;

		  public SQLiteAsyncConnection _connection;
		  private List<Location> savedLocations;
		  public Location activeLocation;

		  public CustomMap()
		  { }


		  public void InitDB()
		  {
				_connection = DependencyService.Get<ISQLiteDb>().GetConnection();
		  }

		  public async void OnMoveToMyLocation(Position userPosition)
		  {
				await _connection.CreateTableAsync<Location>();

				var locations = await _connection.Table<Location>().ToListAsync();
				savedLocations = new List<Location>(locations);

				activeLocation = savedLocations.Find(a => a.Name == "praha");

				if(activeLocation == null)
				{
					 System.Diagnostics.Debug.Write("@@@@@ creating praha");
					 activeLocation = new Location("praha", userPosition);
					 await _connection.InsertAsync(activeLocation);
					 //customMap.GridCenter = userPosition;
				}
				DebugPosition = userPosition;
				GridStepSize = MIN_GRID_STEP_SIZE; //set => it invokes DrawGrid
		  }

		  /*private const string GridCenterKey = "GridCenter";
		  public Position GridCenter
		  {
				get
				{
					 return (Position)Application.Current.Properties[GridCenterKey];
					 //if(Application.Current.Properties.ContainsKey(GridCenterKey))
					 //  return (Position)Application.Current.Properties[GridCenterKey];
					 //return null;
				}
				set
				{
					 Application.Current.Properties[GridCenterKey] = value;
				}
		  }

		  public bool IsGridCenterDefined => Application.Current.Properties.ContainsKey(GridCenterKey);
		  */

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
				//LoggedPositions.Clear();
				LoggedPositions.Add(position);
				MessagingCenter.Send(this, "LogPosition", position);
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
				GridLines = GenerateGridLines();
		  }

		  private List<GridLine> GenerateGridLines()
		  {
				System.Diagnostics.Debug.Write("@@@@@ DrawGrid2");
				int steps = 10;
				//float stepSize = 0.01f;

				List<GridLine> lines = new List<GridLine>();

				double latitude = activeLocation.Center.Latitude;
				double longitude = activeLocation.Center.Longitude - steps * GridStepSize; //left
																													//lines from left to right
				for(int y = -steps; y < steps; y++)
				{
					 latitude = activeLocation.Center.Latitude + y * GridStepSize;

					 lines.Add(new GridLine(new Position(latitude, longitude), new Position(latitude, longitude + 2 * steps * GridStepSize)));

				}

				latitude = activeLocation.Center.Latitude + steps * GridStepSize; //top
																										//lines from top to bottom
				for(int x = -steps; x < steps; x++)
				{
					 longitude = activeLocation.Center.Longitude + x * GridStepSize;

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
				float newValue = GridStepSize - STEP_INCREMENT;
				newValue = Math.Max(MIN_GRID_STEP_SIZE, newValue);
				GridStepSize = newValue; //calls onPropertyChanged
		  }

		  public Tuple<Position, Position> GetAreaOnPosition(Position position)
		  {
				return GetAreaOnIndex(GetIndexInGrid(position));
		  }

		  private Tuple<Position, Position> GetAreaOnIndex(Tuple<int, int> index)
		  {
				int y = index.Item2 > 0 ? index.Item2 : index.Item2 + 1;
				int x = index.Item1 < 0 ? index.Item1 : index.Item1 - 1;
				Position topLeft = new Position(activeLocation.Center.Latitude + y * GridStepSize, activeLocation.Center.Longitude + x * GridStepSize);
				Position botRight = new Position(topLeft.Latitude - GridStepSize, topLeft.Longitude + GridStepSize);
				//Position botRight = new Position(GridCenter.Latitude + (index.Item2) * GridStepSize, GridCenter.Longitude + (index.Item1 - 1) * GridStepSize);

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
				//central coordinate system doesnt contain 0 index

				double xDouble = (position.Longitude - activeLocation.Center.Longitude) / GridStepSize;
				int xSign = Math.Sign(xDouble);
				int x = (int)Math.Ceiling(Math.Abs(xDouble)) * xSign;

				double yDouble = (position.Latitude - activeLocation.Center.Latitude) / GridStepSize;
				int ySign = Math.Sign(yDouble);
				int y = (int)Math.Ceiling(Math.Abs(yDouble)) * ySign;


				//if(Math.Abs(xDouble) < 1)
				//{
				//	 xDouble += Math.Sign(xDouble);
				//}
				//int x = (int)xDouble;

				//double yDouble = (position.Latitude - GridCenter.Latitude) / GridStepSize;
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
