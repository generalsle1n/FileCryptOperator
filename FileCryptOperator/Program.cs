using FileCryptClass;
using System.Text;

string Password = "Secure";
string Salt = "Cool";
byte[] key = Encoding.UTF8.GetBytes(Password.ToCharArray());
byte[] iv = Encoding.UTF8.GetBytes(Salt.ToCharArray());

FileCryptHelper fileCryptHelper = new FileCryptHelper(key, iv);

string FilePath = @"C:\temp\text.txt";

fileCryptHelper.EncryptFile(FilePath);

string EncryptedFile = @"C:\temp\text.dec";

fileCryptHelper.DecryptFile(EncryptedFile);
