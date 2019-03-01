using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Util;
using Android.Widget;
using GoogleMapsTry3;
using GoogleMapsTry3.Droid;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace GoogleMapsTry3.Droid
{
	 public class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter
	 {
		  private List<CustomPin> customPins;
		  private CustomCircle circle;
		  private List<Position> shapeCoordinates;
		  private Position gridCenter;
		  CustomMap customMap;

		  public CustomMapRenderer(Context context) : base(context)
		  {
		  }

		  protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
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
		  }

		  protected override void OnMapReady(GoogleMap map)
		  {
				base.OnMapReady(map);

				NativeMap.InfoWindowClick += OnInfoWindowClick;
				NativeMap.SetInfoWindowAdapter(this);

				//var circleOptions = new CircleOptions();
				//circleOptions.InvokeCenter(new LatLng(circle.Position.Latitude, circle.Position.Longitude));
				//circleOptions.InvokeRadius(circle.Radius);
				//circleOptions.InvokeFillColor(0X66FF0000);
				//circleOptions.InvokeStrokeColor(0X66FF0000);
				//circleOptions.InvokeStrokeWidth(0);

				//NativeMap.AddCircle(circleOptions);

				MoveToMyLocation();

				
		  }

		  private async void MoveToMyLocation()
		  {
				//Log.WriteLine($"MoveToMyLocation");
				

				var locator = CrossGeolocator.Current;
				Plugin.Geolocator.Abstractions.Position position = await locator.GetPositionAsync();
				customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(1)));
				//Debug.Write($"position = {position}");

				Position userPosition = new Position(position.Latitude, position.Longitude);

				gridCenter = new Position(userPosition.Latitude, userPosition.Longitude);

				DrawShapeCoordinates();
				DrawGrid();
		  }


		  private void DrawGrid()
		  {
				int steps = 10;
				float stepSize = 0.01f;
				//PolygonOptions polygonOptions = new PolygonOptions();
				//polygonOptions.InvokeFillColor(0x66FF0000);
				//polygonOptions.InvokeStrokeColor(0x660000FF);
				//polygonOptions.InvokeStrokeWidth(20.0f);

				for(int x = -steps; x < steps; x++)
				{
					 for(int y = -steps; y < steps; y++)
					 {
						  PolygonOptions polygonOptions = GetPolygon();
						  double topLeftLatitude = gridCenter.Latitude + x * stepSize;
						  double topLeftLongitude = gridCenter.Longitude + y * stepSize;

						  polygonOptions.Add(new LatLng(topLeftLatitude, topLeftLongitude));
						  polygonOptions.Add(new LatLng(topLeftLatitude + stepSize, topLeftLongitude));
						  polygonOptions.Add(new LatLng(topLeftLatitude + stepSize, topLeftLongitude - stepSize));
						  polygonOptions.Add(new LatLng(topLeftLatitude, topLeftLongitude - stepSize));

						  NativeMap.AddPolygon(polygonOptions);

					 }
				}
				//NativeMap.AddPolygon(polygonOptions);
		  }

		  private PolygonOptions GetPolygon()
		  {
				PolygonOptions polygonOptions = new PolygonOptions();
				polygonOptions.InvokeFillColor(0x66FF0000);
				polygonOptions.InvokeStrokeColor(0x660000FF);
				polygonOptions.InvokeStrokeWidth(5.0f);
				return polygonOptions;
		  }

		  private void DrawShapeCoordinates()
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
		  }


		  protected override MarkerOptions CreateMarker(Pin pin)
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
		  }

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