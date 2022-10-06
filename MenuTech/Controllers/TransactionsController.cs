using MenuTech.Models;
using MenuTech.Models.Repository;
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
        private MenuTechContext _context;
        private readonly IMenuTechRepository _menuTechRepository;

        public TransactionsController(MenuTechContext menuTechContext, IMenuTechRepository menuTechRepository)
        {
            _context = menuTechContext;
            _menuTechRepository = menuTechRepository;

        }
        [HttpGet]
        public object GetTransctions(int Id)
        {
            return _menuTechRepository.GetTransactions(Id);
        }
    }
}
