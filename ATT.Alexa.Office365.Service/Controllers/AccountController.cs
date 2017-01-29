using ATT.Alexa.Office365.Identity;
using ATT.Alexa.Office365.Models;
using ATT.Alexa.Office365.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ATT.Alexa.Office365.Service.Controllers
{
    public class AccountController : Controller
    {
        private ICreateRepository<User> createUserRepository = null;
        private IReadRepository<User> readUserRepository = null;

        public AccountController(
            ICreateRepository<User> createUserRepository, 
            IReadRepository<User> readUserRepository)
        {
            this.createUserRepository = createUserRepository;
            this.readUserRepository = readUserRepository;
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LinkUser(User user)
        {
            // Authenticate user with O365
            UserAuthentication auth = new UserAuthentication(user);
            if (auth.IsAuthenticated())
            {
                // Create user in DB
                user.UserId = Guid.NewGuid().ToString();
                createUserRepository.Create(user);

                // Redirect to Alex app with user id.
            }
            else
            {
                // Redirect to auth error page.
            }


            return null;
        }
    }
}