using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.ModelViews
{
    public class RegisterViewModel
    {
        [Key]
        public int CustomerId { get; set; }

        [Display(Name = "Họ và Tên")]
        [RegularExpression(@"^[\p{L} ]+$", ErrorMessage = "Tên người dùng không hợp lệ.")]
        [Required(ErrorMessage = "Vui lòng nhập Họ Tên")]
        public string FullName { get; set; }

        [MaxLength(150)]
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "IsEmailExists", controller: "Accounts")]
        public string Email { get; set; }

        [MaxLength(10)]
        [Display(Name = "Điện thoại")]
        [RegularExpression(@"^(0|\+84)[3|5|7|8|9]\d{8}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [Remote(action: "IsPhoneExists", controller: "Accounts")]

        public string Phone { get; set; }

        //
        //[Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        //[Display(Name = "Điện thoại")]
        //[DataType(DataType.PhoneNumber)]
        //public string Phone { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [Display(Name = "Địa chỉ")]
/*        [RegularExpression("^[a-zA-Z0-9_]*$", ErrorMessage = "Địa chỉ không được chứa ký tự đặc biệt")]
*/
        public string Adress { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]

        public string Password { get; set; }

        [MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Nhập lại mật khẩu không đúng")]
        public string ConfirmPassword { get; set; }
    }
}
