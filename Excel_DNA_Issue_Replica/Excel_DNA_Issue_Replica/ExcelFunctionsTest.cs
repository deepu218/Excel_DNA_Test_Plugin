using ExcelDna.Integration;

namespace Excel_DNA_Issue_Replica
{

	//I have shown 1 sample function here, which will fetch the BuyQuantity, when 2 String Inputs namely 'Symbol' and 'Strategy' are provided 
	//while calling the Function in Excel.
	public static class ExcelFunctionsTest
	{
		[ExcelFunction(Name = "Get_Buy_Quantity", Description = "Get the Buy Quantity updated over the day", IsVolatile = true)]
		public static object Get_Buy_Quantity([ExcelArgument(Name = "Symbol", Description = "Identifier of the Commodity")] string symbol,
			[ExcelArgument(Name = "Strategy", Description = "Strategy Name")] string strategy)
		{

			return ExcelAsyncUtil.Observe("Get_Buy_Quantity", symbol + strategy, () => new BuyQuantityObserver(symbol, strategy));

		}
	}
}