using MenuTech.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuTech.Controllers
{
    [Route("api/[controller]/{Id}")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        [HttpGet]
        public object GetTransctions(int Id)
        {

            MenuTechContext context = new MenuTechContext();

            var transactions = context.StoreAccounts.Where(c=>c.CustomerId==Id ).Select(c=> new {c.TransactionId,c.CustomerId,c.TransactionPlus,c.Refund });



            return transactions;
        }
    }
}
