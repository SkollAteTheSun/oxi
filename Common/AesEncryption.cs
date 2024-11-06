using System.Security.Cryptography;
using System.Text;

namespace Kp.Ms.Oxi.Common;

public static class AesEncryption
{
    public static byte[] Encrypt(string plaintext, byte[] key, byte[] iv)
    {
        using Aes aesAlg = Aes.Create();
        aesAlg.Key = key;
        aesAlg.IV = iv;
        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
        using var msEncrypt = new MemoryStream();
        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plaintext);
            csEncrypt.Write(plainBytes, 0, plainBytes.Length);
        }
        var encryptedBytes = msEncrypt.ToArray();
        return encryptedBytes;
    }

    public static string Decrypt(byte[] ciphertext, byte[] key, byte[] iv)
    {
        using Aes aesAlg = Aes.Create();
        aesAlg.Key = key;
        aesAlg.IV = iv;
        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
        byte[] decryptedBytes;
        using (var msDecrypt = new MemoryStream(ciphertext))
        {
            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            {
                using (var msPlain = new MemoryStream())
                {
                    csDecrypt.CopyTo(msPlain);
                    decryptedBytes = msPlain.ToArray();
                }
            }
        }
        return Encoding.UTF8.GetString(decryptedBytes);
    }
}