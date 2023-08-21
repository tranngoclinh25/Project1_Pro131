using System;
using System.Collections.Generic;

#nullable disable

namespace ASM.Models
{
    public partial class Category
    {
        public Category()
        {
            Posts = new HashSet<Post>();
            Products = new HashSet<Product>();
        }

        public int CateId { get; set; }
        public string CateName { get; set; }
        public string Title { get; set; }
        public string Img { get; set; }
        public bool Published { get; set; }
        public int? Odersing { get; set; }
        public int? Parents { get; set; }
        public string Icon { get; set; }
        public string Cover { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
