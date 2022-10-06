using MenuTech.Models;
using MenuTech.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MenuTech.Controllers
{

    [ApiController]
    public class StoreController : ControllerBase
    {

        private MenuTechContext _context;
        private readonly IMenuTechRepository _menuTechRepository;

        public StoreController(MenuTechContext menuTechContext, IMenuTechRepository menuTechRepository)
        {
            _context = menuTechContext;
            _menuTechRepository = menuTechRepository;

        }

        //public StoreController(MenuTechContext menuTechContext)
        //{
        //    _context = menuTechContext;
        //}


        [HttpGet]
        [Route("[controller]/GetStoreAccBalance")]
        public decimal GetStoreAccBalance()
        {
            return _menuTechRepository.GetStoreAccBalance();
        }


        [HttpGet]
        [Route("[controller]/GetCustomerAccBalance/{Id}")]
        [Authorize]
        public decimal GetCustomerAccBalance(int Id)
        {
            return _menuTechRepository.GetCustomerAccBalance(Id);
        }


        [HttpPost]
        [Route("[controller]/Pay")]
        [Authorize]
        public string Pay([FromBody] object json)
        {

            Payment payment = new Payment();

            payment.customerId = Convert.ToInt32(System.Text.Json.JsonSerializer.Deserialize<Payment>(json.ToString()).customerId);
            payment.amount = Convert.ToString(System.Text.Json.JsonSerializer.Deserialize<Payment>(json.ToString()).amount);
            payment.currency = Convert.ToInt32(System.Text.Json.JsonSerializer.Deserialize<Payment>(json.ToString()).currency);

            return _menuTechRepository.Pay(payment);
        }

        [HttpPost]
        [Route("[controller]/Refund")]
        [Authorize]
        public Boolean Refund([FromBody] object json)
        {

            Refund refund = new Refund();
            Guid guid;

            var dataobj = JToken.Parse(json.ToString());
            Guid.TryParse(dataobj["transactionId"]?.ToString(), out guid);

            refund.customerId = System.Text.Json.JsonSerializer.Deserialize<Refund>(json.ToString()).customerId;
            // refund.transaction = System.Text.Json.JsonSerializer.Deserialize<Refund>( json.ToString()).transaction;
            refund.transactionId = guid;
            refund.amount = System.Text.Json.JsonSerializer.Deserialize<Refund>(json.ToString()).amount;

            return _menuTechRepository.Refund(refund);
         
        }
    }
}
