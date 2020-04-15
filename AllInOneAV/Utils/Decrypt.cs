using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class DecryptHelper
    {
        public static byte[] AES_IV = Encoding.UTF8.GetBytes("ABCDEF1G34123412");

        public static byte[] GetKey(string pwd)
        {
            while (pwd.Length < 32)
            {
                pwd += '0';
            }
            pwd = pwd.Substring(0, 32);
            return Encoding.UTF8.GetBytes(pwd);
        }

        public static string Decrypt(string pwd, string data)
        {
            byte[] inputBytes = Convert.FromBase64String(data);

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(pwd);
                aesAlg.IV = AES_IV;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream(inputBytes))
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srEncrypt = new StreamReader(csEncrypt))
                        {
                            return srEncrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
