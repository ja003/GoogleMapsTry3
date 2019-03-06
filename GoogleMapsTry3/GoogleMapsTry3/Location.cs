using System;
using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace GoogleMapsTry3
{
	 [Serializable]
	 public class Location
	 {
		  //[PrimaryKey, AutoIncrement]
		  //public int Id
		  //{
		  //get; set;
		  //}

		  //private string _name;

		  //[Unique, MaxLength(10)]
		  //public string Name
		  //{
		  //get
		  //{
		  //	 return _name;
		  //}
		  //set
		  //{
		  //	 if(_name == value)
		  //		  return;
		  //	 _name = value;

		  //	 //OnPropertyChanged();
		  //}
		  //}
		  public string Name;

		  //public List<double> loggedPositionsLatitudes { get; set; }
		  //public List<double> loggedPositionsLongitudes { get; set; }

		  public List<Position> LoggedPositions = new List<Position>();

		  public Position Center;

		  //private double centerLatitude { get; set; } //bez get;set; se to neuloží do DB
		  //private double centerLongitude { get; set; }

		  //public Position Center => new Position(centerLatitude, centerLongitude); //Position není serializable..asi

		  public Location() { }
		  public Location(string name, Position center)
		  {
				Name = name;
				Center = center;
				//centerLatitude = center.Latitude;
				//centerLongitude = center.Longitude;
				//loggedPositionsLatitudes = new List<double>();
				//loggedPositionsLongitudes = new List<double>();
		  }

		  public List<Position> GetLoggedPositions()
		  {
				return LoggedPositions;
				//List<Position> positions = new List<Position>();
				//for(int i = 0; i < loggedPositionsLatitudes.Count; i++)
				//{
				//	 positions.Add(new Position(loggedPositionsLatitudes[i], loggedPositionsLongitudes[i]));
				//}
				//return positions;
		  }

		  public void LogPosition(Position position)
		  {
				if(LoggedPositions == null)
				{
					 LoggedPositions = new List<Position>();
				}
				LoggedPositions.Add(position);
				//if(loggedPositionsLatitudes == null)
				//{
				//	 loggedPositionsLatitudes = new List<double>();
				//	 loggedPositionsLongitudes = new List<double>();
				//}
				//loggedPositionsLatitudes.Add(position.Latitude);
				//loggedPositionsLongitudes.Add(position.Longitude);
		  }
	 }
}
