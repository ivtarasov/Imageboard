using System.Security.Cryptography;
using System.Text;

namespace Netaba.Services.Pass
{
    public class HashGenerator
    {
        static public byte[] GetHash(string ip, string password) =>
            MD5.HashData(Encoding.UTF8.GetBytes(ip + password));
    }
}
