using MenuTech.Models;
using MenuTech.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private MenuTechContext _context;
        private readonly IMenuTechRepository _menuTechRepository;

        public LoginController(MenuTechContext menuTechContext, IMenuTechRepository menuTechRepository)
        {
            _context = menuTechContext;
            _menuTechRepository = menuTechRepository;
        }


        [HttpGet]
        public object GetUser(string userName, string password)
        {
            try
            {
                var user = _menuTechRepository.GetUser(userName, password);

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
