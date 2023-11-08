using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shared
{
    public class UserChangePasswordModel
    {
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Compare(nameof(Password), ErrorMessage = "Passwords does not match!")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}