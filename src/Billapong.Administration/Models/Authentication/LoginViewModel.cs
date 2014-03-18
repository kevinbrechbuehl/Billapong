using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billapong.Administration.Models.Authentication
{
    using System.ComponentModel.DataAnnotations;

    public class LoginViewModel
    {
        [Display(Name = "UsernameLabel", ResourceType = typeof(Resources.Global))]
        [Required]
        public string Username { get; set; }

        [Display(Name = "PasswordLabel", ResourceType = typeof(Resources.Global))]
        [Required]
        public string Password { get; set; }

        public bool HasErrors { get; set; }
    }
}