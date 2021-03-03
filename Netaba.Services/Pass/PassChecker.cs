using System.Security.Cryptography;
using Netaba.Data.Enteties;
using System.Text;
using System.Linq;

namespace Netaba.Services.Pass
{
    public class PassChecker
    {
        static public bool Check(Post post, string ip, string password) =>
            Enumerable.SequenceEqual(MD5.HashData(Encoding.UTF8.GetBytes(ip + password)), post.IpAndPasswordHash);
    }
}