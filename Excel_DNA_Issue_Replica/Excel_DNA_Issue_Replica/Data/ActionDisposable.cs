using System;

//Action Disposable Defined
namespace Excel_DNA_Issue_Replica
{
	class ActionDisposable : IDisposable
	{
		Action _disposeAction;

		public ActionDisposable(Action disposeAction)
		{
			_disposeAction = disposeAction;

		}

		public void Dispose()
		{
			_disposeAction();
		}
	}
}
