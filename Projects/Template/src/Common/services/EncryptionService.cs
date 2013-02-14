using asi.asicentral.interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.services
{
    public class EncryptionService : IEncryptionService
    {
        private static byte[] _salt = Encoding.ASCII.GetBytes("What happens at ASI stays at ASI");

        public string Encrypt(string key, string text)
        {
            RijndaelManaged aesAlgorithm = null;
            string encryptedText = null;
            try
            {
                if (string.IsNullOrEmpty(text)) throw new Exception("You need to provide a valid text to be encrypted");
                Rfc2898DeriveBytes modifiedKey = new Rfc2898DeriveBytes(key, _salt);
                aesAlgorithm = new RijndaelManaged();
                aesAlgorithm.Key = modifiedKey.GetBytes(aesAlgorithm.KeySize / 8);
                ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor(aesAlgorithm.Key, aesAlgorithm.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // prepend the IV
                    msEncrypt.Write(BitConverter.GetBytes(aesAlgorithm.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlgorithm.IV, 0, aesAlgorithm.IV.Length);
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(text);
                        }
                    }
                    encryptedText = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                if (aesAlgorithm != null) aesAlgorithm.Clear();
            }

            return encryptedText;
        }


        public string Decrypt(string key, string encrytpedText)
        {
            if (string.IsNullOrEmpty(encrytpedText)) throw new ArgumentNullException("encrytpedText");

            RijndaelManaged aesAlgorithm = null;
            string plaintext = null;

            try
            {
                Rfc2898DeriveBytes modifiedKey = new Rfc2898DeriveBytes(key, _salt);
                byte[] bytes = Convert.FromBase64String(encrytpedText);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    aesAlgorithm = new RijndaelManaged();
                    aesAlgorithm.Key = modifiedKey.GetBytes(aesAlgorithm.KeySize / 8);
                    aesAlgorithm.IV = ReadByteArray(msDecrypt);

                    ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor(aesAlgorithm.Key, aesAlgorithm.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                if (aesAlgorithm != null) aesAlgorithm.Clear();
            }

            return plaintext;
        }

        /// <summary>
        /// Reads a byte array from a stream
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }
    }
}
