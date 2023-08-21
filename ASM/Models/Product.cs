using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ASM.Models
{
    public partial class Product
    {
        public Product()
        {
            OderDetails = new HashSet<OderDetail>();
            ProductImages = new HashSet<ProductImage>();
        }

        public int ProductId { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]

        public string ProductName { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Danh mục sản phẩm là bắt buộc.")]

        public int? CateId { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc.")]
        [Range(0, int.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn hoặc bằng 0.")]
        public int? Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn hoặc bằng 0.")]
        public int? Discount { get; set; }
        public string Img { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? Modified { get; set; }
        public bool BestSellers { get; set; }
        public bool HomeFlag { get; set; }
        public bool Active { get; set; }

        [Required(ErrorMessage = "Kích thước sản phẩm là bắt buộc.")]
        public string Tags { get; set; }

        [Required(ErrorMessage = "Tên nhân vật là bắt buộc.")]
        public string Title { get; set; }
        public string Alias { get; set; }

        [Required(ErrorMessage = "Series sản phẩm là bắt buộc.")]
        public string MetaDesc { get; set; }

        [Required(ErrorMessage = "Hãng sản phẩm là bắt buộc.")]
        public string MetaKey { get; set; }

        [Required(ErrorMessage = "Hàng tồn kho sản phẩm là bắt buộc.")]
        [Range(0, int.MaxValue, ErrorMessage = "Hàng tồn kho phải lớn hơn hoặc bằng 0.")]
        public int? SoLuongConLai { get; set; }

        public virtual Category Cate { get; set; }
        public virtual ICollection<OderDetail> OderDetails { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
    }
}
