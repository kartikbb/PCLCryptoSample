using System.Text;
using PCLCrypto;

namespace Security
{
    public class CryptoAesEncryptionProvider
    {
        public string Encrypt(string dataString, string password, string saltString)
        {
            byte[] data = ConvertToBytesArray(dataString);
            byte[] key = CreateDerivedKey(password, saltString);

            ISymmetricKeyAlgorithmProvider aes =
                WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            ICryptographicKey symetricKey = aes.CreateSymmetricKey(key);
            byte[] bytes = WinRTCrypto.CryptographicEngine.Encrypt(symetricKey, data);
            return ConvertToString(bytes);
        }

        public string Decrypt(string dataString, string password, string saltString)
        {
            byte[] data = ConvertToBytesArray(dataString);
            byte[] key = CreateDerivedKey(password, saltString);

            ISymmetricKeyAlgorithmProvider aes =
                WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            ICryptographicKey symetricKey = aes.CreateSymmetricKey(key);
            byte[] bytes = WinRTCrypto.CryptographicEngine.Decrypt(symetricKey, data);
            return ConvertToString(bytes);
        }

        private byte[] CreateDerivedKey(string password, string saltString, int keyLengthInBytes = 32,
            int iterations = 1000)
        {
            byte[] salt = ConvertToBytesArray(saltString);
            byte[] key = NetFxCrypto.DeriveBytes.GetBytes(password, salt, iterations, keyLengthInBytes);
            return key;
        }

        private byte[] ConvertToBytesArray(string input)
        {
            return Encoding.Unicode.GetBytes(input);
        }

        private string ConvertToString(byte[] input)
        {
            return Encoding.Unicode.GetString(input, 0, input.Length);
        }
    }
}