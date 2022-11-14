using System.ComponentModel.DataAnnotations;

namespace OrderApp.Models.Properties
{
    public class OderDetailViewModel
    {
        [Key]
        public OrderDetail OrderDetail { get; set; }
        public Product?  OrderedProduct { get; set; }
        public Order ThisOrder { get; set; }


        public OderDetailViewModel(OrderDetail orderDetail, Product? orderedProduct, Order thisOrder)
        {
	        OrderDetail = orderDetail;
            OrderedProduct = orderedProduct;
	        ThisOrder = thisOrder;
        }
    }
}
