
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;

namespace FileCryptClass
{
    public class FileCryptHelper
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;
        private Aes _crypt;
        private const string _enCryptExtension = "dec";

        public FileCryptHelper(byte[] key, byte[] iv)
        {
            _key = FillUpBytes(key);
            _iv = FillUpBytes(iv);
        }

        private byte[] FillUpBytes(byte[] bytes)
        {

        }

        public void EncryptFile(string FilePath)
        {
            ICryptoTransform cryptTrans = _crypt.CreateEncryptor();
            byte[] sourceContent = File.ReadAllBytes(FilePath);
            sourceContent = CreateByteFileType(FilePath, sourceContent);
            File.Delete(FilePath);

            byte[] encrypted;

            using(MemoryStream ms = new MemoryStream())
            {
                using(CryptoStream cs = new CryptoStream(ms, cryptTrans, CryptoStreamMode.Write))
                {
                    cs.Write(sourceContent);
                }

                encrypted = ms.ToArray();

                string[] fileSplit = FilePath.Split(".");


                File.WriteAllBytes(ReplaceFileExtension(FilePath, _enCryptExtension), encrypted);
            }
        }

        public void DecryptFile(string FilePath)
        {
            ICryptoTransform cryptTrans = _crypt.CreateDecryptor();
            byte[] sourceContent = File.ReadAllBytes(FilePath);
            var a = ExtractFileType(sourceContent);
            File.Delete(FilePath);
            byte[] encrypted;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, cryptTrans, CryptoStreamMode.Write))
                {
                    cs.Write(sourceContent);
                }

                encrypted = ms.ToArray();

                string[] fileSplit = FilePath.Split(".");


                File.WriteAllBytes(ReplaceFileExtension(FilePath, "txt"), encrypted);
            }
        }
    }
}