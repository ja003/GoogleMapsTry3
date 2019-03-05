using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace GoogleMapsTry3
{
	 public interface ISQLiteDb
	 {
		  SQLiteAsyncConnection GetConnection();
	 }
}
