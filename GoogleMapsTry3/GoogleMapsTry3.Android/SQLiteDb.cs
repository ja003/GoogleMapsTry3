using System;
using System.IO;
using SQLite;

namespace GoogleMapsTry3.Droid
{
	 public class SQLiteDb : ISQLiteDb
	 {
		  public SQLiteAsyncConnection GetConnection()
		  {
				var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				var path = Path.Combine(documentsPath, "MySQLite.db");

				return new SQLiteAsyncConnection(path);
		  }
	 }
}