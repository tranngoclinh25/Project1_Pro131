using System;
using System.Collections.Generic;

#nullable disable

namespace ASM.Models
{
    public partial class ProductImage
    {
        public int ProductImageId { get; set; }
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsImage { get; set; }

        public virtual Product Product { get; set; }
    }
}
