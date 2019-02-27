
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;

namespace GoogleMapsTry3.Droid
{
	 [Activity(Label = "GoogleMapsTry3", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	 public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	 {
		  private const int REQUEST_CODE_AccessFineLocation = 1000;

		  protected override void OnCreate(Bundle savedInstanceState)
		  {
				base.OnCreate(savedInstanceState);

				_savedInstanceState = savedInstanceState;
				ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.AccessFineLocation }, REQUEST_CODE_AccessFineLocation);

		  }

		  Bundle _savedInstanceState;
		  private void Init()
		  {
				global::Xamarin.Forms.Forms.Init(this, _savedInstanceState);
				global::Xamarin.FormsMaps.Init(this, _savedInstanceState);

				LoadApplication(new App());
		  }

		  public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		  {
				switch(requestCode)
				{
					 case REQUEST_CODE_AccessFineLocation:
						  break;
					 default: return;

				}
				
				base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

				Init();
		  }

	 }
}