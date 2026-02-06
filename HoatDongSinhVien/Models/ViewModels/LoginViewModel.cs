using System.ComponentModel.DataAnnotations;

namespace HoatDongSinhVien.Models.ViewModels
{
    
        public class LoginViewModel
        {
            [Required(ErrorMessage = "Mẹ mày béo")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Mật khẩu không được để trống.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
    
}
