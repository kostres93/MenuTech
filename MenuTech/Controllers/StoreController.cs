using MenuTech.Models;
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
    //[Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        MenuTechContext context = new MenuTechContext();


        //[Route("api/[controller]")]
        [HttpGet]
        [ActionName("GetStoreAccBalance")]
        [Route("[controller]/GetStoreAccBalance")]
        public decimal GetStoreAccBalance()
        {
            //TODO: izbaci baratanje sa bazom iz kontrolera ubaci samo poziv metoda
            return context.StoreAccounts.Where(s => s.IdStore == 1).OrderByDescending(s => s.IdStoreAccount).FirstOrDefault().Balance; ;
        }

        [HttpGet]
        //[Route("api/[controller]/{Id}")]
        [ActionName("GetCustomerAccBalance")]
        [Route("[controller]/GetCustomerAccBalance/{Id}")]
        [Authorize]
        public decimal GetCustomerAccBalance(int Id)
        {
            decimal balance = 0;

            balance = context.CustomerAccounts.Where(c => c.CustomerId == Id).FirstOrDefault().Balance;

            return balance;
        }

        [HttpPost]
        //[Route("api/[controller]")]
        [Route("[controller]/Pay")]
        [ActionName("Pay")]
        [Authorize]
        public string Pay([FromBody] object json)
        {
            
            Payment payment = new Payment();
            StoreAccount storeAccount = new StoreAccount();
            CustomerAccount customerAccount = new CustomerAccount();
            CurrencyRate currencyRate = new CurrencyRate();

            payment.customerId = Convert.ToInt32(System.Text.Json.JsonSerializer.Deserialize<Payment>(json.ToString()).customerId);
            payment.amount = Convert.ToString(System.Text.Json.JsonSerializer.Deserialize<Payment>(json.ToString()).amount);
            payment.currency = Convert.ToInt32(System.Text.Json.JsonSerializer.Deserialize<Payment>(json.ToString()).currency);

            customerAccount.Balance = context.CustomerAccounts.First(c => c.CustomerId == payment.customerId).Balance;
            currencyRate.CurrencyRateValue = context.CurrencyRates.FirstOrDefault(c => c.CurrencyCode == payment.currency).CurrencyRateValue;

            payment.amount = Convert.ToString(currencyRate.CurrencyRateValue * Convert.ToDecimal(payment.amount));
            
            
            if (customerAccount.Balance > Convert.ToDecimal(payment.amount))
            {
                int maxValue = context.StoreAccounts.Max(x => x.IdStoreAccount);

                storeAccount.Balance = context.StoreAccounts.First(x => x.IdStoreAccount == maxValue).Balance;
                storeAccount.Balance = storeAccount.Balance + Convert.ToDecimal(payment.amount);

                context.StoreAccounts.Add(new StoreAccount()
                {
                    CustomerId = payment.customerId,
                    Balance = storeAccount.Balance,
                    IdStore = 1,
                    TransactionPlus = Convert.ToDecimal(payment.amount)
                });

                context.CustomerAccounts.FirstOrDefault(c => c.CustomerId == payment.customerId).Balance -= Convert.ToDecimal(payment.amount);
                context.SaveChanges();

                storeAccount.TransactionId = context.StoreAccounts.First(x => x.IdStoreAccount == context.StoreAccounts.Max(x => x.IdStoreAccount)).TransactionId;

                return storeAccount.TransactionId.ToString();
            }

            return "Not enoguh money to pay!";
        }

        [HttpPost]
        [ActionName("Refund")]
        [Route("[controller]/Refund")]
        [Authorize]
        public Boolean Refund([FromBody] object json)
        {

          
            Refund refund = new Refund();

            var dataobj = JToken.Parse(json.ToString());
           
            Guid guid;
            Guid.TryParse(dataobj["transactionId"]?.ToString(), out guid);

            
            refund.customerId = System.Text.Json.JsonSerializer.Deserialize<Refund>(json.ToString()).customerId;
            // refund.transaction = System.Text.Json.JsonSerializer.Deserialize<Refund>( json.ToString()).transaction;
            refund.transactionId = guid;
            refund.amount = System.Text.Json.JsonSerializer.Deserialize<Refund>(json.ToString()).amount;
           
     

           StoreAccount storeAccount= context.StoreAccounts.FirstOrDefault(c => c.CustomerId == refund.customerId
                                                    && c.TransactionId == refund.transactionId
                                                    && c.TransactionPlus == Convert.ToDecimal(refund.amount) 
                                                    && c.Refund!=1);
            if (storeAccount==null)
            {
                return false;
            }

            storeAccount.TransactionMinus= Convert.ToDecimal(refund.amount);
            storeAccount.Balance-= Convert.ToDecimal(refund.amount);
            storeAccount.Refund = 1;

            context.Entry(storeAccount).State = EntityState.Modified;
            context.SaveChanges();

            return true;
        }
    }
}
