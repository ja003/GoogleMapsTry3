using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace GoogleMapsTry3
{
    public class GridLine
    {
		  public Position start;
		  public Position end;

		  public GridLine(Position start, Position end)
		  {
				this.start = start;
				this.end = end;
		  }
	 }
}
