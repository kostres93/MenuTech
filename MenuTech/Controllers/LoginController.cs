using MenuTech.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuTech.Controllers
{
    [Route("api/[controller]/{userName}/{password}")]
    [ApiController]
    public class LoginController : ControllerBase
    {
       
        [HttpGet]
        public object GetUser(string userName, string password)
        {
            
            MenuTechContext context = new MenuTechContext();

            var base64EncodedBytes =Convert.FromBase64String(password);
            Encoding.UTF8.GetString(base64EncodedBytes);

            if (context.Customers.Any(c=>c.UserName==userName && c.Password== Encoding.UTF8.GetString(base64EncodedBytes)))
            {
                var user = context.Customers.FirstOrDefault(c => c.UserName == userName && c.Password == Encoding.UTF8.GetString(base64EncodedBytes));
                return user;
            }
            

            return false;
        }
    }
}
