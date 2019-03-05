using GoogleMapsTry3.Droid;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDb))]
namespace GoogleMapsTry3.Droid
{
	 public class SQLiteDb : ISQLiteDb
	 {
		  public SQLiteAsyncConnection GetConnection()
		  {
				var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				var path = Path.Combine(documentsPath, "MySQLite.db3");

				return new SQLiteAsyncConnection(path);
		  }
	 }
}