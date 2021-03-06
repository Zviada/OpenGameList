﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using OpenGameListWebApp.Data.Comments;
using OpenGameListWebApp.Data.Items;
using OpenGameListWebApp.Data.Users;

namespace OpenGameListWebApp.Data
{
    public class DbSeeder
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public DbSeeder(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task SeedAsync()
        {
            //Verify that DB is created
            _dbContext.Database.EnsureCreated();

            //Create default users
            if (await _dbContext.Users.CountAsync() == 0)
            {
                await CreateUsersAsync();
            }

            if (await _dbContext.Items.CountAsync() == 0)
            {
                CreateItems();
            }
        }

        private async Task CreateUsersAsync()
        {
            DateTime createdDate = new DateTime(2017, 3, 1, 12, 30, 0);
            DateTime lastModifiedDate = DateTime.Now;
            string roleAdministrator = "Administrators";
            string roleRegistered = "Registered";

            //Create Roles (if they doesn't exist yet)
            if (!await _roleManager.RoleExistsAsync(roleAdministrator))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleAdministrator));
            }

            if (!await _roleManager.RoleExistsAsync(roleRegistered))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleRegistered));
            }
            
            //Creating "Admin" user
            var userAdmin = new ApplicationUser
                            {
                                UserName = "Admin",
                                Email = "admin@opengamelist.com",
                                CreatedDate = createdDate,
                                LastModifiedDate = lastModifiedDate
                            };

            // Insert "Admin" into the Databse and also assign the "Administrator" role to him
            if (await _userManager.FindByIdAsync(userAdmin.Id) == null)
            {
                await _userManager.CreateAsync(userAdmin, "Pass4Admin");
                await _userManager.AddToRoleAsync(userAdmin, roleAdministrator);

                //Remove lockout and E-Mail confirmation.
                userAdmin.EmailConfirmed = true;
                userAdmin.LockoutEnabled = false;
            }

#if DEBUG
            // Create some sample users
            var ryanUser = new ApplicationUser
                                  {
                                      UserName = "Ryan",
                                      Email = "ryan@opengamelist.com",
                                      CreatedDate = createdDate,
                                      LastModifiedDate = lastModifiedDate,
                                      EmailConfirmed = true,
                                      LockoutEnabled = false
                                  };

            var soliceUser = new ApplicationUser
                                  {
                                      UserName = "Solice",
                                      Email = "solice@opengamelist.com",
                                      CreatedDate = createdDate,
                                      LastModifiedDate = lastModifiedDate,
                                      EmailConfirmed = true,
                                      LockoutEnabled = false
                                  };

            var vodanUser = new ApplicationUser
                                  {
                                      UserName = "Vodan",
                                      Email = "vodan@opengamelist.com",
                                      CreatedDate = createdDate,
                                      LastModifiedDate = lastModifiedDate,
                                      EmailConfirmed = true,
                                      LockoutEnabled = false
                                  };
            
            // Insert sample registered users into the Database and also assign the "Registered" role to him
            if (await _userManager.FindByIdAsync(ryanUser.Id) == null)
            {
                await _userManager.CreateAsync(ryanUser, "Pass4Ryan");
                await _userManager.AddToRoleAsync(ryanUser, roleRegistered);
                //remove lockout and email confirmation
                ryanUser.EmailConfirmed = true;
                ryanUser.LockoutEnabled = false;
            }

            if (await _userManager.FindByIdAsync(soliceUser.Id) == null)
            {
                await _userManager.CreateAsync(soliceUser, "Pass4Solice");
                await _userManager.AddToRoleAsync(soliceUser, roleRegistered);
                //remove lockout and email confirmation
                soliceUser.EmailConfirmed = true;
                soliceUser.LockoutEnabled = false;
            }

            if (await _userManager.FindByIdAsync(vodanUser.Id) == null)
            {
                await _userManager.CreateAsync(vodanUser, "Pass4Vodan");
                await _userManager.AddToRoleAsync(vodanUser, roleRegistered);
                //remove lockout and email confirmation
                vodanUser.EmailConfirmed = true;
                vodanUser.LockoutEnabled = false;
            }
#endif
            await _dbContext.SaveChangesAsync();
        }

        private void CreateItems()
        {
            DateTime createdDate = new DateTime(2017, 3, 1, 12, 30, 0);
            DateTime lastModifiedDate = DateTime.Now;

            var authorId = _dbContext.Users.FirstOrDefault(u => u.UserName == "Admin")?.Id;
#if DEBUG
            var num = 1000;
            for (int id = 1; id <= num; id++)
            {
                _dbContext.Items.Add(GetSampleItem(id, authorId, num - 1, new DateTime(2016, 12, 31).AddDays(-num)));
            }
#endif

            EntityEntry<Item> e1 = _dbContext.Items.Add(new Item
                                                        {
                                                            UserId = authorId,
                                                            Title = "Magarena",
                                                            Description = "Single-player fantasy card game similar to Magic: The Gathering",
                                                            Text = @"Loosely based on Magic: The Gathering, the game lets you play against a computer opponent or another human being.
The game features a well-developed AI, an intuitive and clear interface and an enticing level of gameplay.",
                                                            Notes = "This is a sample record created by the Code-First Configuration class",
                                                            ViewCount = 2343,
                                                            CreatedDate = createdDate,
                                                            LastModifiedDate = lastModifiedDate
                                                        });
            EntityEntry<Item> e2 = _dbContext.Items.Add(new Item()
                                                        {
                                                            UserId = authorId,
                                                            Title = "Minetest",
                                                            Description = "Open-Source alternative to Minecraft",
                                                            Text = @"The Minetest gameplay is very similar to Minecraft's:
you are playing in a 3D open world, where you can create and/or remove various types of blocks.
Minetest feature both single-player and multiplayer game modes.
It also has support for custom mods, additional texture packs and other custom/personalization options.
Minetest has been released in 2015 under GNU Lesser General Public License.",
                                                            Notes = "This is a sample record created by the Code-First Configuration class",
                                                            ViewCount = 4180,
                                                            CreatedDate = createdDate,
                                                            LastModifiedDate = lastModifiedDate
                                                        });

            EntityEntry<Item> e3 = _dbContext.Items.Add(new Item()
                                                        {
                                                            UserId = authorId,
                                                            Title = "Relic Hunters Zero",
                                                            Description = "A free game about shooting evil space ducks with tiny,cute guns.",
                                                            Text = @"Relic Hunters Zero is fast, tactical and also very smooth to play.
It also enables the users to look at the source code, so they can can get creative and keep this game alive, fun and free for years to come.
The game is also available on Steam.",
                                                            Notes = "This is a sample record created by the Code-First Configuration class",
                                                            ViewCount = 5203,
                                                            CreatedDate = createdDate,
                                                            LastModifiedDate = lastModifiedDate
                                                        });

            EntityEntry<Item> e4 = _dbContext.Items.Add(new Item()
                                                        {
                                                            UserId = authorId,
                                                            Title = "SuperTux",
                                                            Description = "A classic 2D jump and run, side-scrolling game similar to the Super Mario series.",
                                                            Text = @"The game is currently under Milestone 3. The Milestone 2, which is currently out, features the following:
- a nearly completely rewritten game engine based on OpenGL, OpenAL, SDL2, ...
- support for translations
- in-game manager for downloadable add-ons and translations
- Bonus Island III, a for now unfinished Forest Island and the development levels in Incubator Island
- a final boss in Icy Island
- new and improved soundtracks and sound effects
... and much more!
The game has been released under the GNU GPL license.",
                                                            Notes = "This is a sample record created by the Code-First Configuration class",
                                                            ViewCount = 9602,
                                                            CreatedDate = createdDate,
                                                            LastModifiedDate = lastModifiedDate
                                                        });

            EntityEntry<Item> e5 = _dbContext.Items.Add(new Item()
                                                        {
                                                            UserId = authorId,
                                                            Title = "Scrabble3D",
                                                            Description = "A 3D-based revamp to the classic Scrabble game.",
                                                            Text = @"Scrabble3D extends the gameplay of the classic game Scrabble by adding a new whole third dimension.
Other than playing left to right or top to bottom, you'll be able to place your tiles above or beyond other tiles.
Since the game features more fields, it also uses a larger letter set.
You can either play against the computer, players from your LAN or from the Internet.
The game also features a set of game servers where you can challenge players from all over the world and get ranked into anofficial, ELO-based rating/ladder system.",
                                                            Notes = "This is a sample record created by the Code-First Configuration class",
                                                            ViewCount = 6754,
                                                            CreatedDate = createdDate,
                                                            LastModifiedDate = lastModifiedDate
                                                        });

#if DEBUG
            if (!_dbContext.Comments.Any())
            {
                int numComments = 10; //comments per item
                for (int i = 1; i <= numComments; i++)
                {
                    _dbContext.Comments.Add(GetSampleComment(i, e1.Entity.Id, authorId, createdDate.AddDays(i)));
                    _dbContext.Comments.Add(GetSampleComment(i, e2.Entity.Id, authorId, createdDate.AddDays(i)));
                    _dbContext.Comments.Add(GetSampleComment(i, e3.Entity.Id, authorId, createdDate.AddDays(i)));
                    _dbContext.Comments.Add(GetSampleComment(i, e4.Entity.Id, authorId, createdDate.AddDays(i)));
                    _dbContext.Comments.Add(GetSampleComment(i, e5.Entity.Id, authorId, createdDate.AddDays(i)));
                }
            }
#endif

            _dbContext.SaveChanges();
        }

        #region Utility methods

        private static Item GetSampleItem(int id, string authorId, int viewCount, DateTime createdDate)
        {
            return new Item
                   {
                       UserId = authorId,
                       Title = $"Item {id} Title",
                       Description = $"This is a sample descriotion for item {id}: Lorem ipsum dolor sit amet.",
                       Notes = "This is a sample record created by the Code-First Configuration class",
                       ViewCount = viewCount,
                       CreatedDate = createdDate,
                       LastModifiedDate = createdDate
                   };
        }

        private static Comment GetSampleComment(int n, int itemId, string authorId, DateTime createdDate)
        {
            return new Comment
                   {
                       ItemId = itemId,
                       UserId = authorId,
                       ParentId = null,
                       Text = $"Sample comment #{n} for the item #{itemId}",
                       CreatedDate = createdDate,
                       LastModifiedDate = createdDate
                   };
        }

        #endregion
    }
}