using System.ComponentModel.DataAnnotations;
using System;

namespace qlthucung.Models
{
    public class AccountViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Ten Tai Khoan")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Mat Khau")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        [Display(Name = "Nhap Lai Mat Khau")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Ho Ten")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "Ngay sinh")]
        public DateTime BirthDate { get; set; }
    }

}
