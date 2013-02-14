using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.interfaces
{
    /// <summary>
    /// Used to encrypt data
    /// </summary>
    public interface IEncryptionService
    {
        /// <summary>
        /// Used to encrypt a string using key for the encryption
        /// </summary>
        /// <param name="passKey">The value of the key to encrypt the data</param>
        /// <param name="text">The text to be encrypted</param>
        /// <returns>The text encrypted</returns>
        string Encrypt(string key, string text);

        /// <summary>
        /// Used to decrypt a string using the key for the decryption
        /// </summary>
        /// <param name="key">The vlaue of the key which was used to encrypt the data</param>
        /// <param name="text">The encrypted text to be decrypted</param>
        /// <returns>The decrypted text</returns>
        string Decrypt(string key, string text);
    }
}
