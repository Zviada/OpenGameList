using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Compilation.TagHelpers;
using Newtonsoft.Json;
using OpenGameListWebApp.Data;
using OpenGameListWebApp.Data.Users;
using OpenGameListWebApp.ViewModels;

namespace OpenGameListWebApp.Controllers
{
    public class AccountsController: BaseController
    {
        public AccountsController(ApplicationDbContext dbContext, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager):
            base(dbContext, signInManager, userManager)
        {
        }


        /// <summary>
        /// GET: api/accounts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var id = GetCurrentUserId();
            var user = DbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                return new JsonResult(new UserViewModel
                                      {
                                          UserName = user.UserName,
                                          Email = user.Email,
                                          DisplayName = user.DisplayName
                                      }, DefaultJsonSettings);
            }

            return NotFound(new { error = $"User ID {id} has not been found" });
        }

        /// <summary>
        /// GET: api/accounts/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return BadRequest(new { error = "Not implemented yet." });
        }

        public async Task<IActionResult> Add([FromBody] UserViewModel userViewModel)
        {
            //return a generic HTTP Status 500 (Not Found) if the client payload is invalid
            if (userViewModel == null)
            {
                return new StatusCodeResult(500);
            }

            try
            {
                ApplicationUser user = await UserManager.FindByNameAsync(userViewModel.UserName) ?? await UserManager.FindByEmailAsync(userViewModel.UserName);
                if (user != null)
                {
                    throw new Exception("User wuth such UserName or E-Mail already exists");
                }

                var now = DateTime.Now;

                //create a new User
                user = new ApplicationUser
                       {
                           UserName = userViewModel.UserName,
                           Email = userViewModel.Email,
                           CreatedDate = now,
                           LastModifiedDate = now
                       };

                //Add the user to the Db with a random password
                await UserManager.CreateAsync(user, userViewModel.Password);

                //Assign the user to the 'Registered' role
                await UserManager.AddToRoleAsync(user, "Registered");

                //Remove lockout and e-mail confirmation
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;

                DbContext.SaveChanges();

                return new JsonResult(new UserViewModel
                                      {
                                          UserName = user.UserName,
                                          Email = user.Email,
                                          DisplayName = user.DisplayName
                                      }, DefaultJsonSettings);
            }
            catch (Exception e)
            {
                return new JsonResult(new { error = e.Message });
            }
        }

        /// <summary>
        /// PUT: /api/accounts/{id}
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UserViewModel userViewModel)
        {
            if (userViewModel == null)
            {
                return NotFound(new { error = "Current User has not been found" });
            }

            try
            {
                var id = GetCurrentUserId();
                ApplicationUser user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                if (!await UserManager.CheckPasswordAsync(user, userViewModel.Password))
                {
                    throw new Exception("Old password mismatch");
                }

                //current password is ok, perform changes if any
                bool hadChanges = false;

                if (user.Email != userViewModel.Email)
                {
                    ApplicationUser findUser = await UserManager.FindByEmailAsync(userViewModel.Email);
                    if (findUser != null && findUser.Id != user.Id)
                    {
                        throw new Exception("E-mail already exists");
                    }

                    await UserManager.SetEmailAsync(user, userViewModel.Email);
                    hadChanges = true;
                }

                if (!String.IsNullOrEmpty(userViewModel.PasswordNew))
                {
                    await UserManager.ChangePasswordAsync(user, userViewModel.Password, userViewModel.PasswordNew);
                    hadChanges = true;
                }

                if (user.DisplayName != userViewModel.DisplayName)
                {
                    user.DisplayName = userViewModel.DisplayName;
                    hadChanges = true;
                }

                if (hadChanges)
                {
                    user.LastModifiedDate = DateTime.Now;
                    DbContext.SaveChanges();
                }

                //return the updated User to the client
                return new JsonResult(new UserViewModel
                                      {
                                          UserName = user.UserName,
                                          Email = user.Email,
                                          DisplayName = user.DisplayName
                                      }, DefaultJsonSettings);
            }
            catch (Exception e)
            {
                return new JsonResult(new { error = e.Message });
            }
        }

        /// <summary>
        /// DELETE: api/accounts
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        public IActionResult Delete()
        {
            return BadRequest(new { error = "not implemented yet." });
        }

        /// <summary>
        /// DELETE: api/accounts/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return BadRequest(new { error = "not implemented yet." });
        }
    }
}