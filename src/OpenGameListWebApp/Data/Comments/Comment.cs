using System;
using System.ComponentModel.DataAnnotations;

namespace OpenGameListWebApp.Data.Comments
{
    public class Comment
    {
        public Comment()
        {
        }

        #region Properties

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public int Flags { get; set; }

        [Required]
        public string UserId { get; set; }

        public int? ParentId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }

        #endregion
    }
}