using System.Security.Cryptography;
using Imageboard.Data.Enteties;
using System.Text;
using System.Linq;

namespace Imageboard.Services.Pass
{
    public class Checker
    {
        static public bool Check(Post post, string ip, string password) =>
            Enumerable.SequenceEqual(MD5.HashData(Encoding.ASCII.GetBytes(ip + password)), post.IpAndPasswordHash);
    }
}