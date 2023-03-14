
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;

namespace FileCryptClass
{
    public class FileCryptHelper : IDisposable
    {
        private readonly int _keyBitSize;
        private Aes _crypt;
        private readonly ICryptoTransform _encryptor;
        private readonly ICryptoTransform _decryptor;
        private InfoHelper _infoHelper = new InfoHelper();

        public FileCryptHelper(AESSize Size, string key, string iv)
        {
            _keyBitSize = (int)Size;

            _crypt = Aes.Create();
            _crypt.KeySize = _keyBitSize;
            
            _crypt.Key = GetByteArrayFromString(key, _crypt.KeySize / 8);
            _crypt.IV = GetByteArrayFromString(iv, _crypt.BlockSize / 8);

            _encryptor = _crypt.CreateEncryptor();
            _decryptor = _crypt.CreateDecryptor();

        }

        private byte[] GetByteArrayFromString(string Text, int Size)
        {
            byte[] bytesRaw = Encoding.UTF8.GetBytes(Text);
            byte[] bytes = new byte[Size];
            if(bytesRaw.Length >= Size)
            {
                bytes = bytesRaw[0..Size];
            }
            else
            {
                int count = 0;
                foreach(byte b in bytesRaw)
                {
                    bytes[count] = b;
                    count++;
                }
            }

            return bytes;
        }

        public void EncryptFile(string FilePath)
        {
            byte[] sourceContent = File.ReadAllBytes(FilePath);
            File.Delete(FilePath);

            byte[] encrypted;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, _encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(sourceContent);
                }

                encrypted = ms.ToArray();
                encrypted = _infoHelper.InsertInfoIntoByte(FilePath, encrypted);

                string newName = Path.Combine(Path.GetDirectoryName(FilePath), Guid.NewGuid().ToString());
                File.WriteAllBytes(newName, encrypted);
            }
        }

        public void DecryptFile(string FilePath)
        {
            KeyValuePair<string, byte[]> sourceContent = _infoHelper.GetInfoAndContent(File.ReadAllBytes(FilePath));
            byte[] decrypted;
            File.Delete(FilePath);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, _decryptor, CryptoStreamMode.Write))
                {
                    cs.Write(sourceContent.Value);
                }

                decrypted = ms.ToArray();

                string[] fileSplit = FilePath.Split(".");


                File.WriteAllBytes(sourceContent.Key, decrypted);
            }
        }

        public void Dispose()
        {
            _encryptor.Dispose();
            _decryptor.Dispose();
            _crypt.Dispose();
        }
    }
}