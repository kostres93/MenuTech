using MenuTech.Models.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuTech.Models
{
    public class MenuTechRepository : IMenuTechRepository
    {
        private readonly MenuTechContext _context;

        public MenuTechRepository(MenuTechContext context)
        {
            _context = context;
        }
        public object GetUser(string userName, string password)
        {
            var base64EncodedBytes = Convert.FromBase64String(password);
            Encoding.UTF8.GetString(base64EncodedBytes);

            if (_context.Customers.Any(c => c.UserName == userName && c.Password == Encoding.UTF8.GetString(base64EncodedBytes)))
            {
                var user = _context.Customers.FirstOrDefault(c => c.UserName == userName && c.Password == Encoding.UTF8.GetString(base64EncodedBytes));
                return user;
            }
            return false;
        }

        public object GetTransactions(int Id)
        {
            var transactions = _context.StoreAccounts.Where(c => c.CustomerId == Id).Select(c => new { c.TransactionId, c.CustomerId, c.TransactionPlus, c.Refund });

            return transactions;
        }

        public decimal GetStoreAccBalance()
        {
            return _context.StoreAccounts.Where(s => s.IdStore == 1).OrderByDescending(s => s.IdStoreAccount).FirstOrDefault().Balance;
        }

        public decimal GetCustomerAccBalance(int Id)
        {
            return _context.CustomerAccounts.Where(c => c.CustomerId == Id).FirstOrDefault().Balance;
        }

        public string Pay(Payment payment)
        {
            StoreAccount storeAccount = new StoreAccount();
            CustomerAccount customerAccount = new CustomerAccount();
            CurrencyRate currencyRate = new CurrencyRate();

            customerAccount.Balance = _context.CustomerAccounts.First(c => c.CustomerId == payment.customerId).Balance;
            currencyRate.CurrencyRateValue = _context.CurrencyRates.FirstOrDefault(c => c.CurrencyCode == payment.currency).CurrencyRateValue;

            payment.amount = Convert.ToString(currencyRate.CurrencyRateValue * Convert.ToDecimal(payment.amount));

            if (customerAccount.Balance > Convert.ToDecimal(payment.amount))
            {
                int maxValue = _context.StoreAccounts.Max(x => x.IdStoreAccount);

                storeAccount.Balance = _context.StoreAccounts.First(x => x.IdStoreAccount == maxValue).Balance;
                storeAccount.Balance = storeAccount.Balance + Convert.ToDecimal(payment.amount);

                _context.StoreAccounts.Add(new StoreAccount()
                {
                    CustomerId = payment.customerId,
                    Balance = storeAccount.Balance,
                    IdStore = 1,
                    TransactionPlus = Convert.ToDecimal(payment.amount)
                });

                _context.CustomerAccounts.FirstOrDefault(c => c.CustomerId == payment.customerId).Balance -= Convert.ToDecimal(payment.amount);
                _context.SaveChanges();

                storeAccount.TransactionId = _context.StoreAccounts.First(x => x.IdStoreAccount == _context.StoreAccounts.Max(x => x.IdStoreAccount)).TransactionId;

                return storeAccount.TransactionId.ToString();
            }

            return "Not enoguh money to pay!";


        }

        public Boolean Refund (Refund refund)
        {
            StoreAccount storeAccount = _context.StoreAccounts.FirstOrDefault(c => c.CustomerId == refund.customerId
                                                     && c.TransactionId == refund.transactionId
                                                     && c.TransactionPlus == Convert.ToDecimal(refund.amount)
                                                     && c.Refund != 1);
            
            if (storeAccount == null)
            {
                return false;
            }



            storeAccount.TransactionMinus = Convert.ToDecimal(refund.amount);
            storeAccount.Balance -= Convert.ToDecimal(refund.amount);
            storeAccount.Refund = 1;

            CustomerAccount customerAccount = _context.CustomerAccounts.FirstOrDefault(c => c.CustomerId == refund.customerId);
            customerAccount.Balance+= Convert.ToDecimal(refund.amount);

            _context.Entry(storeAccount).State = EntityState.Modified;
            _context.Entry(customerAccount).State = EntityState.Modified;
            _context.SaveChanges();

            return true;
        }
    }
}
