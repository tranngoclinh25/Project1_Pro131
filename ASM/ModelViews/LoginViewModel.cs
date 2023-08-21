using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.ModelViews
{
    public class LoginViewModel
    {
        [Key]
        [MinLength(6, ErrorMessage = "Bạn cần nhập username lớn hơn 6 ký tự")]
        [Required(ErrorMessage = ("Vui lòng nhập Email"))]
        [Display(Name = "Địa chỉ Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Sai định dạng Email")]

        public string UserName { get; set; }

        public string Username { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Text & DataType.PhoneNumber)]
        [MinLength(6, ErrorMessage = "Bạn cần nhập mật khẩu tối thiểu 6 ký tự")]
        [RegularExpression("^[a-zA-Z0-9_]*$", ErrorMessage = "Username không được chứa ký tự đặc biệt")]

        public string Password { get; set; }
    }
}
