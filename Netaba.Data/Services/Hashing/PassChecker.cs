using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace Netaba.Data.Services.Hashing
{
    public class PassChecker
    {
        public static bool Check(byte[] passHash, string ip, string password)
        {
            Enumerable.SequenceEqual(MD5.HashData(Encoding.UTF8.GetBytes(ip + password)), passHash);
        }

        public static bool Check(byte[] passHash, string password)
        {
            Enumerable.SequenceEqual(MD5.HashData(Encoding.UTF8.GetBytes(password)), passHash);
        }
    }
}