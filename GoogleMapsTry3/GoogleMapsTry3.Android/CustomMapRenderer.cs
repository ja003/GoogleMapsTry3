using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using GoogleMapsTry3;
using GoogleMapsTry3.Droid;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace GoogleMapsTry3.Droid
{
	 public class CustomMapRenderer : MapRenderer, IOnMapReadyCallback
	 {
		  //private List<CustomPin> customPins;
		  //private CustomCircle circle;
		  //private List<Position> shapeCoordinates;
		  private CustomMap customMap;

		  public CustomMapRenderer(Context context) : base(context)
		  {
				//MessagingCenter.Subscribe<CustomMap>(this, "LogPosition", OnLogPosition);
				MessagingCenter.Subscribe<CustomMap, Position>(this, "LogPosition", (source, arg) => { OnLogPosition(arg); });
		  }

		  private void OnLogPosition(Position position)
		  {
				DrawLoggedPosition(position);
		  }

		  /// <summary>
		  /// We override the OnElementChanged() event handler to get the desired instance. We also use it for updates.
		  /// </summary>
		  /// <param name="e">It contains either the NewElement or the OldElement</param>
		  protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
		  {
				base.OnElementChanged(e);

				if(e.NewElement != null)
				{
					 customMap = e.NewElement as CustomMap;
					 Control.GetMapAsync(this);
				}

				//UpdatePolyLine();
		  }

		  /// <summary>
		  /// The on element property changed callback.
		  /// </summary>
		  /// <param name="sender">The sender.</param>
		  /// <param name="e">The <see cref="PropertyChangedEventArgs"/>Instance containing the event data.</param>
		  protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		  {

				base.OnElementPropertyChanged(sender, e);
				if(this.Element == null || this.Control == null)
					 return;

				//System.Diagnostics.Debug.WriteLine($"OnElementPropertyChanged {e.PropertyName}");

				if(e.PropertyName == CustomMap.GridStepSizeProperty.PropertyName)
					 DrawGrid();
				else if(e.PropertyName == CustomMap.DebugPositionProperty.PropertyName)
					 DrawDebugPosition();
				//else if(e.PropertyName == CustomMap.LoggedPositionsProperty.PropertyName)
				//	 DrawLoggedPositions();

				//if(e.PropertyName == CustomMap.GridStepSizeProperty.PropertyName || e.PropertyName == CustomMap.DebugPositionProperty.PropertyName)
				//	 Redraw();
		  }





		  private bool onMapReadyCaled;
		  protected override void OnMapReady(GoogleMap map)
		  {
				base.OnMapReady(map);

				if(onMapReadyCaled)
					 return;
				onMapReadyCaled = true;

				MoveToMyLocation();

		  }

		  private async void MoveToMyLocation()
		  {
				//Log.WriteLine($"MoveToMyLocation");

				Plugin.Geolocator.Abstractions.IGeolocator locator = CrossGeolocator.Current;
				Plugin.Geolocator.Abstractions.Position position = await GetCurrentPosition();

				customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(1)));
				//Debug.Write($"position = {position}");

				Position userPosition = new Position(position.Latitude, position.Longitude);

				if(!customMap.IsGridCenterDefined)
				{
					 customMap.GridCenter = userPosition;
				}
				customMap.DebugPosition = userPosition;
				customMap.GridStepSize = CustomMap.MIN_GRID_STEP_SIZE; //set => it invokes (should) DrawGrid

		  }

		  public static async Task<Plugin.Geolocator.Abstractions.Position> GetCurrentPosition()
		  {
				Plugin.Geolocator.Abstractions.Position position = null;
				try
				{
					 var locator = CrossGeolocator.Current;
					 locator.DesiredAccuracy = 100;

					 position = await locator.GetLastKnownLocationAsync();

					 if(position != null)
					 {
						  //got a cahched position, so let's use it.
						  return position;
					 }

					 if(!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
					 {
						  //not available or enabled
						  return null;
					 }

					 position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

				}
				catch(Exception ex)
				{
					 //Debug.WriteLine("Unable to get location: " + ex);
				}

				if(position == null)
					 return null;

				var output = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
						position.Timestamp, position.Latitude, position.Longitude,
						position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

				//Debug.WriteLine(output);

				return position;
		  }

		  //překreslí se vše...zatím ne
		  /*private void Redraw()
		  {
				System.Diagnostics.Debug.Write("@@@@@ Redraw");

				NativeMap.Clear();
				DrawGrid();
				DrawDebugPosition();
		  }*/

		  private HashSet<Tuple<int, int>> drawnPositionsIndices = new HashSet<Tuple<int, int>>();

		  private void DrawLoggedPosition(Position position)
		  {
				System.Diagnostics.Debug.Write($"@@@@@ DrawLoggedPosition ");

				Tuple<int, int> index = customMap.GetIndexInGrid(position);
				if(drawnPositionsIndices.Contains(index))
					 return;
				drawnPositionsIndices.Add(index);

				PolygonOptions polygon = GetPolygon();

				Tuple<Position, Position> visitedArea = customMap.GetAreaOnPosition(position);
				Position topLeft = visitedArea.Item1;
				Position botRight = visitedArea.Item2;

				polygon.Add(new LatLng(topLeft.Latitude, topLeft.Longitude));
				polygon.Add(new LatLng(botRight.Latitude, topLeft.Longitude));
				polygon.Add(new LatLng(botRight.Latitude, botRight.Longitude));
				polygon.Add(new LatLng(topLeft.Latitude, botRight.Longitude));

				NativeMap.AddPolygon(polygon);
		  }


		  private void RedrawLoggedPositions()
		  {
				System.Diagnostics.Debug.Write($"@@@@@ DrawLoggedPositions {customMap.LoggedPositions.Count}");

				drawnPositionsIndices.Clear();

				foreach(Position position in customMap.LoggedPositions)
				{
					 DrawLoggedPosition(position);
				}
		  }


		  private CircleOptions debugPositionCircle;

		  /// <summary>
		  /// Minulí pozice zůstávají na mapě
		  /// </summary>
		  private void DrawDebugPosition()
		  {
				if(debugPositionCircle != null)
				{
					 debugPositionCircle.Dispose();
				}
				debugPositionCircle = new CircleOptions();
				debugPositionCircle.InvokeCenter(new LatLng(customMap.DebugPosition.Latitude, customMap.DebugPosition.Longitude));
				debugPositionCircle.InvokeRadius(50);
				debugPositionCircle.InvokeFillColor(0X66FF0000);
				debugPositionCircle.InvokeStrokeColor(0X66FF0000);
				debugPositionCircle.InvokeStrokeWidth(0);


				NativeMap.AddCircle(debugPositionCircle);
		  }


		  private void DrawGrid()
		  {
				System.Diagnostics.Debug.Write("@@@@@ DrawGrid");

				customMap.GenerateGrid();

				if(customMap.GridLines == null)
					 return;

				//smaže vše na mapě
				NativeMap.Clear();

				RedrawLoggedPositions();
				//drawnPositionsIndices.Clear();

				foreach(GridLine line in customMap.GridLines)
				{
					 PolylineOptions lineOptions = GetLine();

					 lineOptions.Add(new LatLng(line.start.Latitude, line.start.Longitude));
					 lineOptions.Add(new LatLng(line.end.Latitude, line.end.Longitude));

					 NativeMap.AddPolyline(lineOptions);
				}

				CircleOptions gridCenterCircle = new CircleOptions();
				gridCenterCircle.InvokeCenter(new LatLng(customMap.GridCenter.Latitude, customMap.GridCenter.Longitude));
				gridCenterCircle.InvokeRadius(50);
				gridCenterCircle.InvokeFillColor(Android.Graphics.Color.DarkSlateBlue);
				gridCenterCircle.InvokeStrokeColor(0X66FF0000);
				gridCenterCircle.InvokeStrokeWidth(2);

				NativeMap.AddCircle(gridCenterCircle);
		  }


		  private PolylineOptions GetLine()
		  {
				PolylineOptions lineOptions = new PolylineOptions();
				lineOptions.InvokeColor(0x660000FF);
				lineOptions.InvokeWidth(5);
				return lineOptions;
		  }

		  private PolygonOptions GetPolygon()
		  {
				PolygonOptions polygon = new PolygonOptions();
				polygon.InvokeFillColor(0x66FF0000);
				polygon.InvokeStrokeColor(0x660000FF);
				polygon.InvokeStrokeWidth(10.0f);
				return polygon;
		  }
	 }
}