using Microsoft.AspNetCore.Mvc;
using OrderApp.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using OrderApp.Authorization;
using OrderApp.Calculations;

namespace OrderApp.Controllers
{
    public class HomeController : Controller
    {
		private readonly ApiKeyHandler _apiKeyHandler = new ();

        public IActionResult Index()
        {
	        IEnumerable<Product>? products = new List<Product>();

	        try
	        {
		        var response = _apiKeyHandler.SendGetRequest("Products");
		        if (response.IsSuccessStatusCode)
		        {
			        string data = response.Content.ReadAsStringAsync().Result;
			        products = JsonConvert.DeserializeObject<List<Product>>(data);
			        return View(products);
		        }
	        }
	        catch (Exception ex)
	        {
		        return View(products);

	        }

            return View(products);
		}

		[HttpPost]
        public IActionResult Index(IEnumerable<Product>? products)
        {
			if (products == null) return RedirectToAction("Index", "Home");

			var selectedProduct = new List<Product>();
			var details = new List<OrderDetail>();

			foreach (var product in products)
			{
				if (product.IsSelected)
				{
					details.Add(new()
					{
						OrderDetailID = 0,
						OrderID = 0,
						ProductID = product.ProductID,
						Quantity = product.Quantity
					});

					selectedProduct.Add(product);
				}
					
			}

			var orderCalc = new OrderCalc(selectedProduct);
			var totalCost = orderCalc.GetTotalCost();
			var discount = orderCalc.GetDiscount(totalCost);
			var salesValueExcludingVat = orderCalc.GetSalesValueExcludingVat(totalCost, discount);
	        var salesValueIncludingVat = orderCalc.GetSalesValueIncludingVat(totalCost);

	        if (int.TryParse(HttpContext.Session.GetString("UserID"), out var userId))
			{
				var order = new Order()
				{
					OrderID = 0,
					UserID = userId,
					OrderDate = DateTime.Now,
					CustomerName = HttpContext.Session.GetString("Username") ?? "string",
					SalesValueExcludingVAT = salesValueExcludingVat,
					Discount = discount,
					SalesValueIncludingVAT = salesValueIncludingVat,
					OrderDetails = details
				};

				var response = _apiKeyHandler.SendPostRequest(order, "Orders");

				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("Index", "Home");
				}
			}

				

	        return RedirectToAction("Index", "Home");
        }

        public IActionResult Details(int id)
		{
			var product = new Product();

			try
			{
				var response = _apiKeyHandler.SendGetRequest("Products/"+id);
				if (response.IsSuccessStatusCode)
				{
					string data = response.Content.ReadAsStringAsync().Result;
					product = JsonConvert.DeserializeObject<Product>(data);
					return View(product);
				}
			}
			catch (Exception ex)
			{
				return View(product);

			}

			return View(product);
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}