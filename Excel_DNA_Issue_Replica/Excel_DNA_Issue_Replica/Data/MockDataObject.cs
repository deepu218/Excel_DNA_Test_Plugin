using System.Collections.Generic;
using ExcelDna.Integration;

namespace Excel_DNA_Issue_Replica
{
	class MockDataObject
	{

		//This is basically containing the Observer List for each symbol. And also the updated BUY Quantity at any instance
		//Basically it is a Data Holding Object

		public string Symbol { get; set; }
		public double BUY_QUANTITY { get; set; }

		public List<IExcelObserver> BUY_QUANTITY_OBSERVER { get; set; }



	}
}
