using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenGameListWebApp.ViewModels;

namespace OpenGameListWebApp.Controllers
{
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
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
            return new JsonResult(GetSampleItems().FirstOrDefault(item => item.Id == id), DefaultJsonSettings);
        }

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
            var items = GetSampleItems().OrderByDescending(item => item.CreatedDate).Take(n);
            return new JsonResult(items, DefaultJsonSettings);
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
            var items = GetSampleItems().OrderByDescending(item => item.ViewCount).Take(n);
            return new JsonResult(items, DefaultJsonSettings);
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
            var items = GetSampleItems().OrderBy(item => Guid.NewGuid()).Take(n);
            return new JsonResult(items, DefaultJsonSettings);
        }

        #region Private members

        /// <summary>
        /// Generate a sample list for source Items to emulate a database (for testing purposes only)
        /// </summary>
        /// <param name="num">The number of items to generate: default is 999</param>
        /// <returns>A defined number of mock items (for testing purpose only)</returns>
        private List<ItemViewModel> GetSampleItems(int num = 999)
        {
            var list = new List<ItemViewModel>();
            DateTime date = DateTime.Today.AddDays(-num);

            for (int id = 1; id <= num; id++)
            {
                list.Add(new ItemViewModel
                         {
                             Id = id,
                             Title = $"Item {id} Title",
                             Description = $"This is a sample description for item {id}: Lorem ipsum dolor sit amet.",
                             CreatedDate = date.AddDays(id - 1),
                             LastModifiedDate = date.AddDays(id),
                             ViewCount = num - id
                         });
            }

            return list;
        }

        private JsonSerializerSettings DefaultJsonSettings => new JsonSerializerSettings {Formatting = Formatting.Indented};

        private int DefaultNumberOfItems => 5;

        private int MaxNumberOfItems => 100;

        #endregion
    }
}
