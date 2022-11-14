using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace OrderApp.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

		[Required(ErrorMessage = "Username required")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password required required")]
        public string Password { get; set; }

        public string Role { get; set; } = "";

        public bool IsAdmin { get; set; } = false;
    }
}
