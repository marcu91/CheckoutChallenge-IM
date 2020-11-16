using System;

namespace Gateway.API.Authentication
{
    /// <summary>
    /// A class used to define the response from the authentication controller in the case of a failed operation
    /// </summary>
    public class Response
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
