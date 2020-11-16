using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.API.Helpers
{
    /// <summary>
    /// A class used for encrypting 
    /// </summary>
    public static class EncryptionHelper
    {
        /// <summary>
        /// Generate private key from SHA512 hash
        /// </summary>
        /// <param name="concatenatedString"></param>
        /// <returns></returns>
        public static string GeneratePrivateKey(string concatenatedString)
        {
            // Generate Encrypted Data
            using (SHA512 hashCreator = SHA512.Create())
            {
                byte[] encryptedData = hashCreator.ComputeHash(Encoding.UTF8.GetBytes(concatenatedString));

                var generatedConsistent = new StringBuilder();
                for (int i = 0; i < encryptedData.Length; i++)
                    generatedConsistent.Append(encryptedData[i].ToString("X2", CultureInfo.InvariantCulture));

                return generatedConsistent.ToString().ToUpper(CultureInfo.InvariantCulture);
            }
        }
    }
}