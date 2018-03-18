using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZalApiGateway.ApiTools
{
    internal static class ApiSecurity
    {
        private static string password = "3sc3RLrpd17";
        private static Aes encryptor;
        private static Aes Encryptor {
            get { return encryptor ?? CreateAesEncryptor(); }
        }

        private static Aes CreateAesEncryptor() {
            encryptor = Aes.Create();
            encryptor.Mode = CipherMode.CBC;
            encryptor.Key = SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(password));
            encryptor.IV = new byte[16] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
            return encryptor;
        }

        public static string Encrypt(this string text) {
            MemoryStream memoryStream = new MemoryStream();
            ICryptoTransform aesEncryptor = Encryptor.CreateEncryptor();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            cryptoStream.Write(textBytes, 0, textBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] encryptedBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            string encryptedText = Convert.ToBase64String(encryptedBytes, 0, encryptedBytes.Length);
            return encryptedText;
        }

        public static string Decrypt(this string encryptedText) {
            MemoryStream memoryStream = new MemoryStream();
            ICryptoTransform aesDecryptor = Encryptor.CreateDecryptor();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);
            string text = String.Empty;
            byte[] cipherBytes = Convert.FromBase64String(encryptedText);
            try {
                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] plainBytes = memoryStream.ToArray();
                text = Encoding.UTF8.GetString(plainBytes, 0, plainBytes.Length);
            }
            finally {
                memoryStream.Close();
                cryptoStream.Close();
            }
            return text;
        }
    }
}
