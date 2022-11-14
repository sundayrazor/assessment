using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrderApp.Authorization;
using OrderApp.Models;

namespace OrderApp.Controllers
{
    public class LoginController : Controller
    {
	    private readonly ApiKeyHandler _apiKeyHandler = new ();

	    public IActionResult Index()
		{
			var response = new User();
			return View(response);
		}

		[HttpPost]
		public IActionResult Index(User model)
		{
			if (!ModelState.IsValid) return View(model);

			var user = new User()
			{
				Username = model.Username,
				Password = model.Password,
				Role = model.Role
			};

			var response = _apiKeyHandler.SendGetRequest("Users");
			if (response.IsSuccessStatusCode)
			{
				string data = response.Content.ReadAsStringAsync().Result;
				var loginUse = JsonConvert.DeserializeObject<List<User>>(data)?.FirstOrDefault(o =>
					model.Username.Equals(o.Username) && model.Password.Equals(o.Password));
				if (loginUse != null && loginUse.Username.Equals(model.Username))
				{
					HttpContext.Session.SetString("Username", loginUse.Username);
					HttpContext.Session.SetString("UserID", loginUse.UserID.ToString());
					HttpContext.Session.SetString("Role", loginUse.Role);

					return RedirectToAction("Index", "Home");
				}
			}
			else
				ViewData["ErrorMes"] = "Username or password is wrong";

			return View(model);
		}

        public IActionResult Register()
        {
            var response = new User();
            return View(response);
        }

		[HttpPost]
		public  ActionResult Register(User model)
		{
			if (!ModelState.IsValid) return View(model);

			var user = new User()
			{
				UserID = model.IsAdmin? 1: 2,
				Username = model.Username,
				Password = model.Password,
				Role = model.IsAdmin? "Admin": "Sales People"
			};
			
			var response = _apiKeyHandler.SendPostRequest(user, "Users");
			// Response fail, not part of the requirements
			if (response.IsSuccessStatusCode)
			{

                HttpContext.Session.SetString("Username", model.Username);
                HttpContext.Session.SetString("UserID", model.UserID+"");
                HttpContext.Session.SetString("Role", model.Role);

				return RedirectToAction("Index", "Home");
			}

			return View(model);
		}

		[HttpGet]
		public IActionResult Logout()
		{
			HttpContext.Session.SetString("Username", "");
			HttpContext.Session.SetString("UserID", "");
			HttpContext.Session.SetString("Role", "");

			return RedirectToAction("Index", "Home");
		}
	}
}
