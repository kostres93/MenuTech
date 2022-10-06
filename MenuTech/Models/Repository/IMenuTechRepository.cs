using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuTech.Models.Repository
{
    public interface IMenuTechRepository
    {
        object GetUser(string userName, string password);

        object GetTransactions(int id);

        decimal GetStoreAccBalance();

        decimal GetCustomerAccBalance(int Id);

        string Pay(Payment payment);
        Boolean Refund(Refund refund);

    }
}
