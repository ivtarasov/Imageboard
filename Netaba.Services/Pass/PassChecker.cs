using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace Netaba.Services.Pass
{
    public class HashGenerator
    {
        static public byte[] GetHash(string ip, string password) =>
            MD5.HashData(Encoding.UTF8.GetBytes(ip + password));
    }

    public class PassChecker
    {
        static public bool Check(byte[] passHash, string ip, string password) =>
            Enumerable.SequenceEqual(MD5.HashData(Encoding.UTF8.GetBytes(ip + password)), passHash);
    }
}