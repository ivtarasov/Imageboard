using System.Security.Cryptography;
using System.Text;

namespace Netaba.Services.Pass
{
    public class HashGenerator
    {
        public static byte[] GetHash(string ip, string password) =>
            MD5.HashData(Encoding.UTF8.GetBytes(ip + password));

        public static byte[] GetHash(string password) =>
            MD5.HashData(Encoding.UTF8.GetBytes(password));
    }
}
