using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms.Maps;

namespace GoogleMapsTry3
{
	 public class LoggedPositions : INotifyPropertyChanged
	 {
		  public List<Position> Positions = new List<Position>();

		  /*public LoggedPositions(PropertyChangedEventHandler handler)
			{
				 PropertyChanged = handler;
			}*/

		  public void Log(Position positions)
		  {
				this.Positions.Add(positions);
				OnPropertyChanged();
								
		  }

		  public event PropertyChangedEventHandler PropertyChanged;

		  private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		  {
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		  }
	 }
}
