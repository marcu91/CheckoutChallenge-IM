using System;

namespace Gateway.API.Authentication
{
    /// <summary>
    /// A static class defining the available gateway user roles 
    /// </summary>
    public static class UserRoles
    {
        public const string GatewayAdministrator = "GatewayAdministrator"; 
        public const string GatewayMerchantUser = "GatewayMerchantUser"; 
    }
}
