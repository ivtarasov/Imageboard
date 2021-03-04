using System.Security.Cryptography;
using Netaba.Data.Enteties;
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
        static public bool Check(Post post, string ip, string password) =>
            Enumerable.SequenceEqual(MD5.HashData(Encoding.UTF8.GetBytes(ip + password)), post.PassHash);
    }
}