using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System;

namespace Coffee.Controllers
{
    public class VnPayLibrary
    {
        private SortedList<string, string> requestData = new SortedList<string, string>();

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                requestData.Add(key, value);
            }
        }

        public string CreateRequestUrl(string baseUrl, string hashSecret)
        {
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in requestData)
            {
                if (data.Length > 0)
                {
                    data.Append("&");
                }
                data.Append(kvp.Key).Append("=").Append(HttpUtility.UrlEncode(kvp.Value));
            }

            string rawData = data.ToString();
            string hash = ComputeHmacSha512(hashSecret, rawData);
            return $"{baseUrl}?{rawData}&vnp_SecureHash={hash}";
        }

        private static string ComputeHmacSha512(string key, string input)
        {
            using (HMACSHA512 hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
            }
        }
    }
}