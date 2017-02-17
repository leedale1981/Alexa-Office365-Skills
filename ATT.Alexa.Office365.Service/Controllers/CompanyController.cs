using ATT.Alexa.Office365.Identity;
using ATT.Alexa.Office365.Models;
using ATT.Alexa.Office365.Repositories;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ATT.Alexa.Office365.Service.Controllers
{
    public class CompanyController : Controller
    {
        private ICreateRepositoryAsync<Company> createUserRepository = null;
        private IReadRepositoryAsync<Company> readUserRepository = null;

        public CompanyController(
            ICreateRepositoryAsync<Company> createUserRepository, 
            IReadRepositoryAsync<Company> readUserRepository)
        {
            this.createUserRepository = createUserRepository;
            this.readUserRepository = readUserRepository;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add(Company company)
        {
            this.createUserRepository.Create(company);

            return View("CompanyAdded", company);
        }
    }
}