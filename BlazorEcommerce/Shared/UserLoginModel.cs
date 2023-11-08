using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shared
{
    public class UserLoginModel
    {

        [Required]

        public string Email { get; set; } = string.Empty;

        [Required]

        public string Password { get; set; } = string.Empty;
    }
}