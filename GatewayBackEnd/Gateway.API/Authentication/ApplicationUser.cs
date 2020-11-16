using Microsoft.AspNetCore.Identity;
using System;

namespace Gateway.API.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// The default constructor is used for runtime setup
        /// </summary>
        public ApplicationUser()
        {

        }

        /// <summary>
        /// A constructor used when registering a new user to the gateway 
        /// </summary>
        /// <param name="model">Provides an instance of RegistrationModel</param>
        public ApplicationUser(RegisterModel model)
        {
            if (model == null) return;
            this.Email = model.Email;
            this.SecurityStamp = Guid.NewGuid().ToString();
            this.UserName = model.Username;
        }
    }
}
