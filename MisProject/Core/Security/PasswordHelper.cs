using System.Security.Cryptography;
using System.Text;

namespace Core.Security;

public static class PasswordHelper
{
    public static string EncodePasswordMd5(string pass) //Encrypt using MD5   
    {
        var provider = MD5.Create();
        var salt = "MySalt";
        var bytes = provider.ComputeHash(Encoding.UTF32.GetBytes(salt + pass));
        return BitConverter.ToString(bytes).Replace("-", "").ToLower();
    }
}