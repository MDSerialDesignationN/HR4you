using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace HR4you.Security.Handler;

public class PasswordHandler
{
    private readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();
    private const int SaltSize = 128 /8;

    public string HashPassword(string pwd)
    {
        var salt = new byte[SaltSize];
        _rng.GetBytes(salt);
        return HashPassword(pwd, salt);
    }
    
    //https://datatracker.ietf.org/doc/html/rfc2898
    //https://datatracker.ietf.org/doc/html/rfc2104
    private string HashPassword(string pwd, byte[] salt)
    {

        var key = KeyDerivation.Pbkdf2(pwd, salt, KeyDerivationPrf.HMACSHA256, 100000, 256 / 8);

        var outputBytes = new byte[13 + salt.Length + key.Length];
        Buffer.BlockCopy(salt, 0, outputBytes, 0, salt.Length);
        Buffer.BlockCopy(key, 0, outputBytes,  SaltSize, key.Length);
        return Convert.ToBase64String(outputBytes);
    }

    public bool IsPasswordOk(string password, string hash)
    {
        var salt = GetSaltFromHash(hash);
        var computed = HashPassword(password, salt);
        
        return computed == hash;
    }

    private byte[] GetSaltFromHash(string hash)
    {
        var hashedPassword = Convert.FromBase64String(hash);
        var salt = new byte[SaltSize];
        Buffer.BlockCopy(hashedPassword, 0, salt, 0, salt.Length);
        return salt;
    }
}