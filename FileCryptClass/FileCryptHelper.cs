
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
            _key = key;
            _iv = iv;

            _crypt = Aes.Create();
            _crypt.GenerateKey();
            _crypt.GenerateIV();
        }

        private string ReplaceFileExtension(string Path, string Extension)
        {
            string[] SplittedPath = Path.Split(".");

            return Path.Replace(SplittedPath[SplittedPath.Length -1], Extension);
        }

        private byte[] CreateByteFileType(string FileName, byte[] Content)
        {
            string[] Splitted = FileName.Split(".");
            string FileExtension = Splitted[Splitted.Length - 1];
            byte[] FileType = Encoding.UTF8.GetBytes(FileExtension);

            byte[] NewContent = new byte[Content.Length + FileType.Length + 1];
            Content.CopyTo(NewContent, 0);
            FileType.CopyTo(NewContent, Content.Length +1);

            return NewContent;
        }

        private KeyValuePair<byte[], string> ExtractFileType(byte[] Content)
        {
            List<byte> rawContent = new List<byte>();
            List<byte> type = new List<byte>();

            bool next = false;
            foreach(byte by in Content)
            {
                if(by != 0 && next == false)
                {
                    rawContent.Add(by);
                }else if(by != 0 && next)
                {
                    type.Add(by);
                }
            }
            return new KeyValuePair<byte[], string>(rawContent.ToArray(), Encoding.UTF8.GetString(type.ToArray()));
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