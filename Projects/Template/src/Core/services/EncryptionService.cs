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
		
		public string ECBEncrypt(string key, string text)
		{
			RijndaelManaged aes = new RijndaelManaged();
			aes.BlockSize = 128;
			aes.KeySize = 256;

			/// In Java, Same with below code
			/// Cipher _Cipher = Cipher.getInstance("AES");  // Java Code
			aes.Mode = CipherMode.ECB;

			aes.Key = ASCIIEncoding.UTF8.GetBytes(key);

			ICryptoTransform encrypto = aes.CreateEncryptor();

			byte[] plainTextByte = ASCIIEncoding.UTF8.GetBytes(text);
			byte[] CipherText = encrypto.TransformFinalBlock(plainTextByte, 0, plainTextByte.Length);
			return Convert.ToBase64String(CipherText);
		}

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

        public string LegacyDecrypt(string key, string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText)) throw new ArgumentNullException("encryptedText");
            string plainText = encryptedText;
            try
            {
                string initVector = "#h9E0JKkwe0p@3j8";
                string saltValue = "6e9rja8@4";
                int keySize = 256;
                int passwordIterations = 2;

                // Convert strings defining encryption key characteristics into byte arrays. 
                // Let us assume that strings only contain ASCII codes.
                // If strings include Unicode characters, use Unicode, UTF7, or UTF8 encoding.
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

                // Convert our ciphertext into a byte array.
                byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);

                // First, we must create a password, from which the key will be derived. 
                // This password will be generated from the specified passphrase and salt value. 
                // The password will be created using the specified hash algorithm. 
                // Password creation can be done in several iterations.
                PasswordDeriveBytes password = new PasswordDeriveBytes(
                    key,
                    saltValueBytes,
                    "SHA1",
                    passwordIterations);

                // Use the password to generate pseudo-random bytes for the encryption
                // key. Specify the size of the key in bytes (instead of bits).
                byte[] keyBytes = password.GetBytes(keySize / 8);

                // Create uninitialized Rijndael encryption object.
                RijndaelManaged symmetricKey = new RijndaelManaged();

                // It is reasonable to set encryption mode to Cipher Block Chaining (CBC). 
                // Use default options for other symmetric key parameters.
                symmetricKey.Mode = CipherMode.CBC;
                symmetricKey.Padding = PaddingMode.PKCS7;

                // Generate decryptor from the existing key bytes and initialization vector. 
                // Key size will be defined based on the number of the key bytes.
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

                // Define memory stream which will be used to hold encrypted data.
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

                // Define cryptographic stream (always use Read mode for encryption).
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                // Since at this point we don't know what the size of decrypted data will be, allocate the buffer long enough to hold ciphertext;
                // plaintext is never longer than ciphertext.
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                // Start decrypting.
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                // Close both streams.
                memoryStream.Close();
                cryptoStream.Close();

                // Convert decrypted data into a string. 
                // Let us assume that the original plaintext string was UTF8-encoded.
                plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            catch (Exception)
            {
                //ignore errors, return the original text if cannot decode
            }
            return plainText;
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
