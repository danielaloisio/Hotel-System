using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace KobraSoftware.Filters
{
    public class Cryptography
    {
        public string SetMD5(string password)
        {
            MD5 md5Hasher = MD5.Create();

            byte[] value = md5Hasher.ComputeHash(Encoding.Default.GetBytes(password));

            StringBuilder strBuilder = new StringBuilder();

            for (int i = 0; i < value.Length; i++)
            {
                strBuilder.Append(value[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

    }
}