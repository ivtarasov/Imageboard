using System.Linq;

namespace Netaba.Data.Services.Hashing
{
    public class PassChecker
    {
        public static bool Check(byte[] passHash, string ip, string password)
        {
            return Enumerable.SequenceEqual(HashGenerator.GetHash(ip, password), passHash);
        }

        public static bool Check(byte[] passHash, string password)
        {
            return Enumerable.SequenceEqual(HashGenerator.GetHash(password), passHash);
        }
    }
}