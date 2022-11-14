using System.ComponentModel.DataAnnotations;

namespace OrderApp.Models
{
    public class Order
    {
        public int OrderID { set; get; }
        public int UserID { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public double SalesValueExcludingVAT { get; set; }
        public double Discount { get; set; }
        public double SalesValueIncludingVAT { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
