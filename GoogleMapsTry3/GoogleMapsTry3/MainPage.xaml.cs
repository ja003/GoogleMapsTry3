using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GoogleMapsTry3
{
	 public partial class MainPage : ContentPage
	 {
		  public MainPage()
		  {

				//Map map = new Map(
				//myMap = new Map(
				//MapSpan.FromCenterAndRadius(
				//		  new Position(37, -122), Distance.FromMiles(0.3)))
				//{
				//	 IsShowingUser = true,
				//	 HeightRequest = 100,
				//	 WidthRequest = 960,
				//	 VerticalOptions = LayoutOptions.FillAndExpand
				//};
				//var stack = new StackLayout { Spacing = 0 };
				//stack.Children.Add(map);
				//Content = stack;


				//myMap.SetValue()


				InitializeComponent();
		  }

		  protected override void OnAppearing()
		  {
				base.OnAppearing();
				//myMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(0,0), Distance.FromMiles(0.3)));
				try
				{
					 myMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(0, 0), Distance.FromMeters(100)));
				}
				catch(System.InvalidOperationException ex)
				{
					 System.Diagnostics.Debug.WriteLine(ex.Message);
				}
		  }
	 }
}
