﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nelibur.ObjectMapper;
using Newtonsoft.Json;
using OpenGameListWebApp.Data;
using OpenGameListWebApp.Data.Items;
using OpenGameListWebApp.ViewModels;

namespace OpenGameListWebApp.Controllers
{
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        private ApplicationDbContext _dbContext;

        public ItemsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region RESTful methods

        /// <summary>
        /// GET: api/items
        /// </summary>
        /// <returns>Nothing: this method will raise a HttpNotFound HTTP exception, since we aren't supporting this API call</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return NotFound(new { Error = "Not found" });
        }

        /// <summary>
        /// GET: api/items/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A JSON-serialized object representing a single item.</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = _dbContext.Items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                return new JsonResult(TinyMapper.Map<ItemViewModel>(item), DefaultJsonSettings);
            }

            return NotFound(new { Error = $"Item ID {id} has not been found" });
        }

        /// <summary>
        /// POST: api/items
        /// </summary>
        /// <param name="ivm"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add([FromBody] ItemViewModel ivm)
        {
            if (ivm != null)
            {
                var item = TinyMapper.Map<Item>(ivm);

                //Server side initialized fields
                item.CreatedDate = DateTime.Now;
                item.LastModifiedDate = item.CreatedDate;

                //TODO: replace the following with the current user's id when authentication will be available
                item.UserId = _dbContext.Users.SingleOrDefault(u => u.UserName == "Admin")?.Id;
                _dbContext.Items.Add(item);
                _dbContext.SaveChanges();
                return new JsonResult(TinyMapper.Map<ItemViewModel>(item), DefaultJsonSettings);
            }

            return new StatusCodeResult(500);
        }

        /// <summary>
        /// PUT: api/items/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ivm"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ItemViewModel ivm)
        {
            if (ivm != null)
            {
                var item = _dbContext.Items.SingleOrDefault(i => i.Id == id);
                if (item != null)
                {
                    //update Item
                    item.UserId = ivm.UserId;
                    item.Description = ivm.Description;
                    item.Flags = ivm.Flags;
                    item.Notes = ivm.Notes;
                    item.Text = ivm.Text;
                    item.Title = ivm.Title;

                    //server side properties
                    item.LastModifiedDate = DateTime.Now;

                    _dbContext.SaveChanges();
                    return new JsonResult(TinyMapper.Map<ItemViewModel>(item), DefaultJsonSettings);
                }
            }

            return NotFound(new { Error = $"Item ID {id} has not been found" });
        }

        /// <summary>
        /// DELETE: api/items/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _dbContext.Items.SingleOrDefault(i => i.Id == id);
            if (item != null)
            {
                _dbContext.Items.Remove(item);
                _dbContext.SaveChanges();
                return new OkResult();
            }

            return NotFound(new { Error = $"Item ID {id} has not been found" });
        }

        #endregion

        /// <summary>
        /// GET: /api/items/GetLatest
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetLatest")]
        public IActionResult GetLatest()
        {
            return GetLatest(DefaultNumberOfItems);
        }

        /// <summary>
        /// GET: api/items/GetLatest/{n}
        /// ROUTING TYPE: attribute-based
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        [HttpGet("GetLatest/{n}")]
        public IActionResult GetLatest(int n)
        {
            n = n > MaxNumberOfItems ? MaxNumberOfItems : n;
            var items = _dbContext.Items.OrderByDescending(item => item.CreatedDate).Take(n).ToArray();
            return new JsonResult(ToItemViewModelList(items), DefaultJsonSettings);
        }

        /// <summary>
        /// GET: api/items/GetMostViewed
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMostViewed")]
        public IActionResult GetMostViewed()
        {
            return GetMostViewed(DefaultNumberOfItems);
        }

        /// <summary>
        /// GET: /api/items/GetMostViewed/{n}
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        [HttpGet("GetMostViewed/{n}")]
        public IActionResult GetMostViewed(int n)
        {
            n = n > MaxNumberOfItems ? MaxNumberOfItems : n;
            var items = _dbContext.Items.OrderByDescending(item => item.ViewCount).Take(n).ToArray();
            return new JsonResult(ToItemViewModelList(items), DefaultJsonSettings);
        }

        /// <summary>
        /// GET: api/items/GetRandom
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRandom")]
        public IActionResult GetRandom()
        {
            return GetRandom(DefaultNumberOfItems);
        }

        /// <summary>
        /// GET: /api/items/GetRandom/{n}
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        [HttpGet("GetRandom/{n}")]
        public IActionResult GetRandom(int n)
        {
            n = n > MaxNumberOfItems ? MaxNumberOfItems : n;
            var items = _dbContext.Items.OrderBy(item => Guid.NewGuid()).Take(n).ToArray();
            return new JsonResult(ToItemViewModelList(items), DefaultJsonSettings);
        }

        #region Private members

        private List<ItemViewModel> ToItemViewModelList(IEnumerable<Item> items)
        {
            var list = new List<ItemViewModel>();
            foreach (var item in items)
            {
                list.Add(TinyMapper.Map<ItemViewModel>(item));
            }

            return list;
        }

        private JsonSerializerSettings DefaultJsonSettings => new JsonSerializerSettings {Formatting = Formatting.Indented};

        private int DefaultNumberOfItems => 5;

        private int MaxNumberOfItems => 100;

        #endregion
    }
}
