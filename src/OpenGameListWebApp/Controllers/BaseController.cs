using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenGameListWebApp.Data;
using OpenGameListWebApp.Data.Users;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenGameListWebApp.Controllers
{
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
        #region Common Fields

        protected ApplicationDbContext DbContext;

        protected SignInManager<ApplicationUser> SignInManager;

        protected UserManager<ApplicationUser> UserManager;

        protected JsonSerializerSettings DefaultJsonSettings => new JsonSerializerSettings { Formatting = Formatting.Indented };

        #endregion

        public BaseController(ApplicationDbContext dbContext, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            DbContext = dbContext;
            SignInManager = signInManager;
            UserManager = userManager;
        }

        /// <summary>
        /// Retrieves the .Net Core Identity User Id for the current ClaimsPrincipal
        /// </summary>
        /// <returns></returns>
        public string GetCurrentUserId()
        {
            //if the user is not authenticated, throw an exception
            if (!User.Identity.IsAuthenticated)
            {
                throw new NotSupportedException();
            }

            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
