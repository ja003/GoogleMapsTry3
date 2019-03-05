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

		  public List<Position> LoggedPositions;
		  public Position Center;

		  public Location() { }
		  public Location(string name, Position center)
		  {
				Name = name;
				Center = center;
		  }
	 }
}
