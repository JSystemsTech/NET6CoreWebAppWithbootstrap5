using ApplicationInfoServiceAPI;
using System.Security.Cryptography;
using System.Text;

namespace ApplicationInfoServiceAPI.Extensions
{
    internal class SaltManager
    {
        private (DateTime key, string salt) CurrentSaltKV { get; set; }
        private (DateTime key, string salt) LastSaltKV { get; set; }
        public SaltManager()
        {
            CurrentSaltKV = (DateTime.UtcNow.Date, CreateSaltValue());
            LastSaltKV = (DateTime.UtcNow.AddDays(-1).Date, CreateSaltValue());
        }
        private string CreateSaltValue()
        => $"salt{Guid.NewGuid().ToString().Replace("-", "")}";
        public void Update()
        {
            if (DateTime.UtcNow.Date != CurrentSaltKV.key)
            {
                LastSaltKV = CurrentSaltKV;
                CurrentSaltKV = (DateTime.UtcNow.Date, CreateSaltValue());
            }
        }
        public string CurrentSalt => CurrentSaltKV.salt;
        public string LastSalt => LastSaltKV.salt;
    }
    public static class StringEncryptionExtensions
    {
        private static SaltManager SaltManager = new SaltManager();
        public static string Encrypt(this string data)
        {
            SaltManager.Update();
            return EncryptCore(data, SaltManager.CurrentSalt);
        }
        public static string Decrypt(this string data)
        {
            SaltManager.Update();
            string value = DecryptCore(data, SaltManager.CurrentSalt);
            if (value == null)
            {
                value = DecryptCore(data, SaltManager.LastSalt);
            }
            return value;
        }
        private static (byte[] Key, byte[] IV) GetKeyIV(string salt)
        {
            byte[][] keys = GetHashKeys($"{ApplicationConfiguration.ApplicationSettings.EncryptionKey}{salt}");
            return (keys[0], keys[1]);
        }
        private static string EncryptCore(string data, string salt)
        {
            string encData = null;

            try
            {
                encData = EncryptStringToBytes_Aes(data, salt);
            }
            catch (CryptographicException) { }
            catch (ArgumentNullException) { }

            return encData;
        }
        private static string DecryptCore(string data, string salt)
        {
            string decData = null;

            try
            {
                decData = DecryptStringFromBytes_Aes(data, salt);
            }
            catch (Exception) { }

            return decData;
        }

        private static byte[][] GetHashKeys(string key)
        {
            byte[][] result = new byte[2][];
            Encoding enc = Encoding.UTF8;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] rawKey = enc.GetBytes(key);
                byte[] rawIV = enc.GetBytes(key);

                byte[] hashKey = sha256.ComputeHash(rawKey);
                byte[] hashIV = sha256.ComputeHash(rawIV);

                Array.Resize(ref hashIV, 16);

                result[0] = hashKey;
                result[1] = hashIV;
            }

            return result;
        }
        private static string EncryptStringToBytes_Aes(string plainText, string salt)
        {
            var config = GetKeyIV(salt);
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (config.Key == null || config.Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (config.IV == null || config.IV.Length <= 0)
                throw new ArgumentNullException("IV");

            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = config.Key;
                aesAlg.IV = config.IV;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt =
                            new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }
        private static string DecryptStringFromBytes_Aes(string cipherTextString, string salt)
        {
            var config = GetKeyIV(salt);
            byte[] cipherText = Convert.FromBase64String(cipherTextString);

            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (config.Key == null || config.Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (config.IV == null || config.IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = config.Key;
                aesAlg.IV = config.IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt =
                            new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}
