using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Widget;
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
				System.Diagnostics.Debug.WriteLine($"OnElementPropertyChanged");

				base.OnElementPropertyChanged(sender, e);
				if(this.Element == null || this.Control == null)
					 return;

				System.Diagnostics.Debug.WriteLine($"OnElementPropertyChanged {e.PropertyName}");

				//if(e.PropertyName == CustomMap.GridStepSizeProperty.PropertyName)
				//	 DrawGrid();
				//else if(e.PropertyName == CustomMap.DebugPositionProperty.PropertyName)
				//	 DrawDebugPosition();
				if(e.PropertyName == CustomMap.GridStepSizeProperty.PropertyName || e.PropertyName == CustomMap.DebugPositionProperty.PropertyName)
					 Redraw();
		  }

		  bool onMapReadyCaled;
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
				customMap.GridCenter = userPosition;
				customMap.DebugPosition = userPosition;
				customMap.GridStepSize = 0.01f; //set => it invokes (should) DrawGrid
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

		  private void Redraw()
		  {
				System.Diagnostics.Debug.Write("@@@@@ Redraw");

				NativeMap.Clear();
				DrawGrid();
				DrawDebugPosition();
		  }


		  private void DrawDebugPosition()
		  {
				CircleOptions circleOptions = new CircleOptions();
				circleOptions.InvokeCenter(new LatLng(customMap.DebugPosition.Latitude, customMap.DebugPosition.Longitude));
				circleOptions.InvokeRadius(100);
				circleOptions.InvokeFillColor(0X66FF0000);
				circleOptions.InvokeStrokeColor(0X66FF0000);
				circleOptions.InvokeStrokeWidth(0);

				NativeMap.AddCircle(circleOptions);
		  }


		  private void DrawGrid()
		  {
				System.Diagnostics.Debug.Write("@@@@@ DrawGrid");

				customMap.GenerateGrid();

				if(customMap.GridLines == null)
					 return;

				//NativeMap.Clear();

				foreach(GridLine line in customMap.GridLines)
				{
					 PolylineOptions lineOptions = GetLine();

					 lineOptions.Add(new LatLng(line.start.Latitude, line.start.Longitude));
					 lineOptions.Add(new LatLng(line.end.Latitude, line.end.Longitude));

					 NativeMap.AddPolyline(lineOptions);
				}
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
				PolygonOptions polygonOptions = new PolygonOptions();
				polygonOptions.InvokeFillColor(0x66FF0000);
				polygonOptions.InvokeStrokeColor(0x660000FF);
				polygonOptions.InvokeStrokeWidth(5.0f);
				return polygonOptions;
		  }

	 }
}