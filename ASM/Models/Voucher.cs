using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ASM.Models
{
    public partial class Voucher
    {
        public Voucher()
        {
            Oders = new HashSet<Oder>();
        }
        public int VoucherId { get; set; }

        [Required(ErrorMessage = "Mã giảm giá là bắt buộc.")]
        [StringLength(20, ErrorMessage = "Mã giảm giá không vượt quá 20 ký tự.")]
        public string VoucherCode { get; set; }

        [Display(Name = "Ngày áp dụng")]
        [Required(ErrorMessage = "Ngày áp dụng là bắt buộc.")]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Ngày hết hạn")]
        [Required(ErrorMessage = "Ngày hết hạn là bắt buộc.")]
        [DataType(DataType.Date)]
        [EndDateMustBeGreaterThanStartDate(ErrorMessage = "Ngày hết hạn phải lớn hơn ngày áp dụng.")]
        public DateTime ExpirationDate { get; set; }

        [Required(ErrorMessage = "Giá trị giảm giá là bắt buộc.")]
        public int DiscountValue { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "Tên chương trình giảm giá là bắt buộc.")]
        public string VoucherName { get; set; }

        [Required(ErrorMessage = "Kiểu giảm giá là bắt buộc.")]
        public int? VoucherType { get; set; }

        [Required(ErrorMessage = "Số lượng giảm giá là bắt buộc.")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng giảm giá phải lớn hơn hoặc bằng 0.")]
        public int? Quantity { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<Oder> Oders { get; set; }
    }
    // Custom validation attributes
    public class EndDateMustBeGreaterThanStartDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var voucher = (Voucher)validationContext.ObjectInstance;

            if (voucher.ExpirationDate <= voucher.CreateDate)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }

   
}
