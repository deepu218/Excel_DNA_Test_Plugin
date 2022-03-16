using System;
using System.Collections.Generic;
using System.Diagnostics;
using ExcelDna.Integration;
using System.Timers;
using System.Collections.Concurrent;

namespace Excel_DNA_Issue_Replica
{
	class BuyQuantityObserver : IExcelObservable
	{
		private List<IExcelObserver> _observerList;
		private string _symbol;
		private string _strategy;
		private Timer _timer;

		public BuyQuantityObserver(string symbol, string strategy)
		{
			_observerList = new List<IExcelObserver>();
			_symbol = symbol;
			_strategy = strategy;
			_timer = new Timer();
			_timer.AutoReset = true;
			_timer.Interval = 1000;
			_timer.Elapsed += _timer_Elapsed;

			if (!DataObjectManager.getInstance()._dataObjectMap.ContainsKey(_symbol))
			{
				DataObjectManager.getInstance()._dataObjectMap[symbol] = new ConcurrentDictionary<string,MockDataObject>();
				DataObjectManager.getInstance()._dataObjectMap[symbol][strategy] = new MockDataObject();
			}


			Trace.TraceInformation("Observer Created for symbol : " + symbol + " and strategy : " + strategy);
			
		}

		void _timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (!DataObjectManager.getInstance()._dataObjectMap.ContainsKey(_symbol))
			{
				_timer.Stop();
				DataObjectManager.getInstance()._dataObjectMap[_symbol][_strategy].BUY_QUANTITY_OBSERVER = _observerList;
				timer_tick(DataObjectManager.getInstance()._dataObjectMap[_symbol][_strategy].BUY_QUANTITY);
			}
			else
			{
				timer_tick(0);
			}

		}

		public IDisposable Subscribe(IExcelObserver observer)
		{
			_observerList.Add(observer);
			if (!DataObjectManager.getInstance()._dataObjectMap.ContainsKey(_symbol))
			{
				_timer.Stop();
				DataObjectManager.getInstance()._dataObjectMap[_symbol][_strategy].BUY_QUANTITY_OBSERVER = _observerList;
				timer_tick(DataObjectManager.getInstance()._dataObjectMap[_symbol][_strategy].BUY_QUANTITY);
			}
			else
			{
				timer_tick(0);
				_timer.Start();
			}

			return new ActionDisposable(() =>
			{
				Trace.TraceInformation("Unsubscribed for Buy Quantity for Symbol " + _symbol + " and strategy " + _strategy);
				_observerList.Remove(observer);
			})
			{

			};
		}

		void timer_tick(object _now){
			foreach (var obs in _observerList)
				obs.OnNext(_now);
		}
	}
}
