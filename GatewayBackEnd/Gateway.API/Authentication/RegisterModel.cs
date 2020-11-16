using System;
using System.ComponentModel.DataAnnotations;

namespace Gateway.API.Authentication
{
    /// <summary>
    /// A class used to define the registration credentials 
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// A property that sets the name of the gateway user
        /// </summary>
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        /// <summary>
        /// A property that sets the email of the gateway user
        /// </summary>
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        /// <summary>
        /// A property that sets the password of a gateway user. A complex password of at least 10 characters is recommended.
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
