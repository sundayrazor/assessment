using OrderApp.Models;

namespace OrderApp.Calculations
{
    public class OrderCalc
    {
	    private readonly IEnumerable<Product> _products;

	    public OrderCalc(IEnumerable<Product> products)
	    {
		    _products = products;
	    }

	    public double GetTotalCost()
	    {
			var totalCost = 0.0;
		    foreach (var product in _products)
		    {
			    totalCost += product.Quantity * product.UnitPrice;
		    }

		    return totalCost;
	    }

	    public double GetDiscount(double salesValue) =>
		    salesValue is >= 200 and < 500
			    ? 0.03 * salesValue
			    : salesValue >= 500
				    ? 0.10 * salesValue
				    : 0;

	    public double GetSalesValueExcludingVat(double totalCost, double discount) =>
		    totalCost - discount;

	    public double GetSalesValueIncludingVat(double salesValueExcludingVat) =>
			salesValueExcludingVat + 0.15 * salesValueExcludingVat;

	    
    }
}
