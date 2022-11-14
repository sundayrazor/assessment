using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrderApp.Authorization;
using OrderApp.Models;
using OrderApp.Models.Properties;

namespace OrderApp.Controllers
{
    public class OrderController : Controller
    {
	    private readonly ApiKeyHandler _apiKeyHandler = new ();

	    public ActionResult Index()
	    {
	        IEnumerable<Order>? orders = new List<Order>();
	        try
	        {

		        var response = _apiKeyHandler.SendGetRequest("Orders");

		        if (response.IsSuccessStatusCode)
		        {
			        string data = response.Content.ReadAsStringAsync().Result;
			        orders = JsonConvert.DeserializeObject<List<Order>>(data);
			        return View(orders);
		        }
	        }
	        catch (Exception _)
	        {
		        return View(orders);
	        }
	        return View(orders);
        }


        public ActionResult Details(int id)
        {
	        var orderDetailProperties = new List<OderDetailViewModel>();

	        try
			{
				var response = _apiKeyHandler.SendGetRequest("Orders/"+id);

				if (response.IsSuccessStatusCode)
				{
					var data = response.Content.ReadAsStringAsync().Result;
					var order = JsonConvert.DeserializeObject<Order>(data);
					if (order != null)
					{
						var details = order.OrderDetails;
						foreach (var detail in details)
						{
							response = _apiKeyHandler.SendGetRequest("Products/"+detail.ProductID);
							if (response.IsSuccessStatusCode)
							{
								data = response.Content.ReadAsStringAsync().Result;
								orderDetailProperties.Add(new(detail, JsonConvert.DeserializeObject<Product>(data), order));
							}
						}
					}
					
					return View(orderDetailProperties);
				}
			}

			catch (Exception _)
			{
				return View(orderDetailProperties);
			}
			return View(orderDetailProperties);
        }
    }
}
