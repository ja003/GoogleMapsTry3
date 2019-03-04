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
		  private List<CustomPin> customPins;
		  private CustomCircle circle;
		  private List<Position> shapeCoordinates;
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

				if(e.PropertyName == CustomMap.GridStepSizeProperty.PropertyName)
					 DrawGrid();
		  }


		  /*protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
		  {
				base.OnElementChanged(e);

				if(e.OldElement != null)
				{
					 NativeMap.InfoWindowClick -= OnInfoWindowClick;
				}

				if(e.NewElement != null)
				{
					 customMap = (CustomMap)e.NewElement;
					 customPins = customMap.CustomPins;

					 circle = customMap.Circle;

					 shapeCoordinates = customMap.ShapeCoordinates;
					 gridCenter = customMap.GridCenter;

					 Control.GetMapAsync(this);
				}
		  }*/

		  bool onMapReadyCaled;
		  protected override void OnMapReady(GoogleMap map)
		  {
				base.OnMapReady(map);

				//NativeMap.InfoWindowClick += OnInfoWindowClick;
				//NativeMap.SetInfoWindowAdapter(this);

				//var circleOptions = new CircleOptions();
				//circleOptions.InvokeCenter(new LatLng(circle.Position.Latitude, circle.Position.Longitude));
				//circleOptions.InvokeRadius(circle.Radius);
				//circleOptions.InvokeFillColor(0X66FF0000);
				//circleOptions.InvokeStrokeColor(0X66FF0000);
				//circleOptions.InvokeStrokeWidth(0);

				//NativeMap.AddCircle(circleOptions);

				if(onMapReadyCaled)
					 return;
				onMapReadyCaled = true;

				MoveToMyLocation();
				//if(onMapReadyInvoked)
				//	 return;
				//onMapReadyInvoked = true;
				//customMap.OnMapReady.Invoke(DrawGrid2);

		  }

		  private async void MoveToMyLocation()
		  {
				//Log.WriteLine($"MoveToMyLocation");


				Plugin.Geolocator.Abstractions.IGeolocator locator = CrossGeolocator.Current;
				//Plugin.Geolocator.Abstractions.Position position = await locator.GetPositionAsync();
				Plugin.Geolocator.Abstractions.Position position = await GetCurrentPosition();

				customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(1)));
				//Debug.Write($"position = {position}");

				Position userPosition = new Position(position.Latitude, position.Longitude);

				//gridCenter = new Position(userPosition.Latitude, userPosition.Longitude);

				//DrawShapeCoordinates();
				//DrawGrid();
				//DrawGrid2();
				customMap.GridCenter = userPosition;
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


		  private void DrawGrid()
		  {
				System.Diagnostics.Debug.Write("@@@@@ DrawGrid");

				customMap.GenerateGrid();

				if(customMap.GridLines == null)
					 return;

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

		  /*private void DrawShapeCoordinates()
		  {
				if(shapeCoordinates.Count == 0)
					 return;

				var polygonOptions = new PolygonOptions();
				polygonOptions.InvokeFillColor(0x66FF0000);
				polygonOptions.InvokeStrokeColor(0x660000FF);
				polygonOptions.InvokeStrokeWidth(20.0f);

				foreach(var position in shapeCoordinates)
				{
					 polygonOptions.Add(new LatLng(position.Latitude, position.Longitude));
				}

				NativeMap.AddPolygon(polygonOptions);
		  }*/


		  /*protected override MarkerOptions CreateMarker(Pin pin)
		  {
				var marker = new MarkerOptions();
				marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
				marker.SetTitle(pin.Label);
				marker.SetSnippet(pin.Address);
				marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin));
				return marker;
		  }

		  private void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
		  {
				var customPin = GetCustomPin(e.Marker);
				if(customPin == null)
				{
					 throw new Exception("Custom pin not found");
				}

				if(!string.IsNullOrWhiteSpace(customPin.Url))
				{
					 var url = Android.Net.Uri.Parse(customPin.Url);
					 var intent = new Intent(Intent.ActionView, url);
					 intent.AddFlags(ActivityFlags.NewTask);
					 Android.App.Application.Context.StartActivity(intent);
				}
		  }

		  public Android.Views.View GetInfoContents(Marker marker)
		  {
				var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
				if(inflater != null)
				{
					 Android.Views.View view;

					 var customPin = GetCustomPin(marker);
					 if(customPin == null)
					 {
						  throw new Exception("Custom pin not found");
					 }

					 if(customPin.Id.ToString() == "Xamarin")
					 {
						  view = inflater.Inflate(Resource.Layout.XamarinMapInfoWindow, null);
					 }
					 else
					 {
						  view = inflater.Inflate(Resource.Layout.MapInfoWindow, null);
					 }

					 var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
					 var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);

					 if(infoTitle != null)
					 {
						  infoTitle.Text = marker.Title;
					 }
					 if(infoSubtitle != null)
					 {
						  infoSubtitle.Text = marker.Snippet;
					 }

					 return view;
				}
				return null;
		  }*/

		  //private void DrawGrid2()
		  //{
		  //System.Diagnostics.Debug.Write("@@@@@ DrawGrid2");
		  //int steps = 10;
		  //float stepSize = 0.01f;
		  ////PolygonOptions polygonOptions = new PolygonOptions();
		  ////polygonOptions.InvokeFillColor(0x66FF0000);
		  ////polygonOptions.InvokeStrokeColor(0x660000FF);
		  ////polygonOptions.InvokeStrokeWidth(20.0f);


		  //double longitude = gridCenter.Longitude + steps * stepSize; //top
		  //double latitude = gridCenter.Latitude;
		  //for(int x = -steps; x < steps; x++)
		  //{
		  //	 PolylineOptions lineOptions = GetLine();
		  //	 latitude = gridCenter.Latitude + x * stepSize;

		  //	 lineOptions.Add(new LatLng(latitude, longitude));
		  //	 lineOptions.Add(new LatLng(latitude, longitude - 2 * steps * stepSize));

		  //	 NativeMap.AddPolyline(lineOptions);
		  //}

		  //latitude = gridCenter.Latitude - steps * stepSize; //left
		  //for(int y = -steps; y < steps; y++)
		  //{
		  //	 PolylineOptions lineOptions = GetLine();
		  //	 longitude = gridCenter.Longitude + y * stepSize;

		  //	 lineOptions.Add(new LatLng(latitude, longitude));
		  //	 lineOptions.Add(new LatLng(latitude + 2 * steps * stepSize, longitude));

		  //	 NativeMap.AddPolyline(lineOptions);
		  //}
		  //}

		  public Android.Views.View GetInfoWindow(Marker marker)
		  {
				return null;
		  }

		  private CustomPin GetCustomPin(Marker annotation)
		  {
				var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
				foreach(var pin in customPins)
				{
					 if(pin.Position == position)
					 {
						  return pin;
					 }
				}
				return null;
		  }
	 }
}