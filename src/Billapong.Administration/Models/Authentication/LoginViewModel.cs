namespace Billapong.Administration.Models.Authentication
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Login view model.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [Display(Name = "UsernameLabel", ResourceType = typeof(Resources.Global))]
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Display(Name = "PasswordLabel", ResourceType = typeof(Resources.Global))]
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current request has errors or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if request has errors; otherwise, <c>false</c>.
        /// </value>
        public bool HasErrors { get; set; }
    }
}