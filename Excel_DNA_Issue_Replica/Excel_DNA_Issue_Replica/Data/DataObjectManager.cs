
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Timers;
using System.Diagnostics;


namespace Excel_DNA_Issue_Replica
{
	class DataObjectManager
	{
		private Timer _timer;
		//Map storing Symbol as Key and Value as another Map of Strategy and MockDataObject
		public ConcurrentDictionary<string, ConcurrentDictionary<string, MockDataObject>> _dataObjectMap;
		private static DataObjectManager _dataObjectManagerInstance = null;

		private DataObjectManager()
		{
			_dataObjectMap = new ConcurrentDictionary<string, ConcurrentDictionary<string, MockDataObject>>();

			_timer = new Timer();
			_timer.AutoReset = true;
			_timer.Interval = 100;
			_timer.Elapsed += timer_Elapsed;
			_timer.Start();
		}

		//Made the class as Singleton
		public static DataObjectManager getInstance() {
			if (_dataObjectManagerInstance == null)
			{
				_dataObjectManagerInstance = new DataObjectManager();
			}
			return _dataObjectManagerInstance;
		}

		//At Each time Elapsed instance, I increment the existing Buy Quantity value for each each MockDataObject present
		//in the map by 100
		//This is basically an alternate to the TCP Connection, which is actually updating the Data in these MockDataObjects.
		//Frequency of Updates -> 100 milliseconds
		void timer_Elapsed(object sender, ElapsedEventArgs e) {
			foreach (KeyValuePair<string, ConcurrentDictionary<string, MockDataObject>> entry in _dataObjectMap)
			{
				foreach (KeyValuePair<string, MockDataObject> innerEntry in entry.Value)
				{
					innerEntry.Value.BUY_QUANTITY += 100;
					Trace.TraceInformation("Value of Buy Quantity for symbol : " + entry.Key + " and terminal : " + innerEntry.Key + " updated to value : " + innerEntry.Value.BUY_QUANTITY);
					foreach (var obs in innerEntry.Value.BUY_QUANTITY_OBSERVER)
						obs.OnNext(innerEntry.Value.BUY_QUANTITY);


					//The Problem starts after Here
					//In the Logged statement, I clearly get the Updated quantity being pushed to the Observers. 
					//But in Excel, at some point of time, the data stops updating Altogether, in some of the Cells.
					//Also, if I delete the Formula and rewrite it once again in the same cell, the Updated Values start Appearing.

					//Thus, I suspect, it's not a problem with Data updates from backend
					//It can be either some misconfiguration/incorrect use of Excel DNA in my code, or an Excel DNA Library Bug.
				}
			}
		}

	}
}
