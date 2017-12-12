using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpenGameListWebApp.Data.Comments;
using OpenGameListWebApp.Data.Users;

namespace OpenGameListWebApp.Data.Items
{
    public class Item
    {
        public Item()
        {
        }

        #region Properties

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public string Text { get; set; }

        public string Notes { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public int Flags { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ViewCount { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }

        #endregion

        #region Related Properties

        /// <summary>
        /// Current Item's Author
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ApplicationUser Author { get; set; }

        /// <summary>
        /// Current Item's Comment list
        /// </summary>
        public virtual List<Comment> Comments { get; set; }

        #endregion
    }
}