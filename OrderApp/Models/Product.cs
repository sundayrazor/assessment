namespace OrderApp.Models
{
    public class Product
    {
	    public int ProductID { get; set; }
	    public string ProductName { get; set; }
	    public int Quantity { get; set; }
	    public double UnitPrice { get; set; }
	    public string Description { get; set; }
	    public bool IsSelected { get; set; } = false;
    }
}
