using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace GoogleMapsTry3
{
    public class Location
    {
		  [PrimaryKey, AutoIncrement]
		  public int Id
		  {
				get; set;
		  }

		  private string _name;

		  [Unique, MaxLength(10)]
		  public string Name
		  {
				get
				{
					 return _name;
				}
				set
				{
					 if(_name == value)
						  return;
					 _name = value;

					 //OnPropertyChanged();
				}
		  }

		  public List<Position> LoggedPositions = new List<Position>();

		  public double centerLatitude { get; set; } //bez get;set; se to neuloží do DB
		  public double centerLongitude { get; set; }

		  public Position Center => new Position(centerLatitude, centerLongitude); //Position není serializable..asi

		  public Location() { }
		  public Location(string name, Position center)
		  {
				Name = name;
				centerLatitude = center.Latitude;
				centerLongitude = center.Longitude;
				//Center = center;
		  }
	 }
}
