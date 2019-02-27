using System.Windows;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.WPF;
using Xamarin;

namespace GoogleMapsTry3.Wpf
{
	 /// <summary>
	 /// Interaction logic for MainWindow.xaml
	 /// </summary>
	 public partial class MainWindow : FormsApplicationPage
	 {
		  public MainWindow()
		  {
				InitializeComponent();

				Forms.Init();

				//key: AIzaSyCAIDuaANxKVQoyWXlf2hU_BxGT1ldJQUg
				//Xamarin.Forms.Maps.Map( .Forms. Maps.Init("AIzaSyCAIDuaANxKVQoyWXlf2hU_BxGT1ldJQUg");

				//Xamarin.Forms.Maps.WPF.MapRenderer.
				//MapRenderer m = new MapRenderer();
				//m.Control.ke
				FormsMaps.Init("AIzaSyAcig5ny-nDBiorzsV9xf896QMYTHV5V50");


				LoadApplication(new GoogleMapsTry3.App());
		  }
	 }
}
