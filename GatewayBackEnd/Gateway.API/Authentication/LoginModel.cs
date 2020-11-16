using System;
using System.ComponentModel.DataAnnotations;

namespace Gateway.API.Authentication
{
    /// <summary>
    /// A class used to define the login credentials 
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// A property that sets the gateway user name ( not to be confused with the merchantID )
        /// </summary>
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        /// <summary>
        /// A property that sets the password of the gateway user
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
