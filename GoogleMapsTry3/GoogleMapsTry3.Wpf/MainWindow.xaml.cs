using System.Windows;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;

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
				LoadApplication(new GoogleMapsTry3.App());
		  }
	 }
}
