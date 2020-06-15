using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Aot.Hrms.Contracts
{
    public class CryptoHelper
    {
        public static string Hash(string message, string secret)
        {
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

        public static string Serialize<T>(T obj, bool? encode = null)
        {
            var objText = JsonConvert.SerializeObject(obj);

            if (encode.GetValueOrDefault(false))
                objText = Encode(objText);

            return objText;
        }

        public static T Deserialize<T>(string objText, bool? decode = null)
        {
            if (decode.GetValueOrDefault(false))
                objText = Decode(objText);

            return JsonConvert.DeserializeObject<T>(objText);
        }

        public static string Decode(string encodedString)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encodedString));
        }

        public static string Encode(string plainText)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        }

        public static string Encrypt(string input, string key)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string Decrypt(string input, string key)
        {
            byte[] inputArray = Convert.FromBase64String(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static string Compress(string inputStr)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputStr);

            using (var outputStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(outputStream, CompressionMode.Compress))
                    gZipStream.Write(inputBytes, 0, inputBytes.Length);

                var outputBytes = outputStream.ToArray();

                return Encoding.UTF8.GetString(outputBytes);
            }
        }

        public static string Decompress(string inputStr)
        {
            byte[] inputBytes = Convert.FromBase64String(inputStr);

            using (var inputStream = new MemoryStream(inputBytes))
            using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            using (var outputStream = new MemoryStream())
            {
                gZipStream.CopyTo(outputStream);
                var outputBytes = outputStream.ToArray();

                return Encoding.UTF8.GetString(outputBytes);
            }
        }
    }
}
