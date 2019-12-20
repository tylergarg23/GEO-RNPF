using System.Text;
using System.Security.Cryptography;

namespace SERFOR.Component.Tools.Cryptography
{
    public class HashCrypter
    {

        public static string Sha1Encrypter(string strHashing)
        {
            return Encrypter(new SHA1CryptoServiceProvider(), strHashing);
        }

        public static string Md5Encrypter(string strHashing)
        {
            return Encrypter(new MD5CryptoServiceProvider(), strHashing);
        }

        public static string Encrypter(HashAlgorithm algorimt, string strHashing)
        {
            var result = algorimt.ComputeHash(Encoding.UTF8.GetBytes(strHashing));
            var builder = new StringBuilder();
            foreach (var b in result)
            {
                builder.AppendFormat("{0:x2}", b);
            }
            return builder.ToString();
        }
    }
}
