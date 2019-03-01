using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace GoogleMapsTry3
{
	 public class CustomMap : Map
	 {
		  public List<CustomPin> CustomPins { get; set; }
		  public CustomCircle Circle { get; set; }
		  public List<Position> ShapeCoordinates { get; set; }
		  public Position GridCenter { get; set; }
		  public Action<Action> OnMapReady { get; internal set; }

		  public CustomMap()
		  {
				ShapeCoordinates = new List<Position>();
		  }
	 }
}
