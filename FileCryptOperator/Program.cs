using FileCryptClass;
using System.Text;

string Password = "Secure";
string IV = "Cool";

using (FileCryptHelper fileCryptHelper = new FileCryptHelper(AESSize.Strong, Password, IV))
{
    string folder = @"C:\temp\a\";
    string[] Allfiles = Directory.GetFiles(folder);

    foreach(string File in Allfiles)
    {
        fileCryptHelper.EncryptFile(File);
    }

    string[] AllNewfiles = Directory.GetFiles(folder);
    foreach (string File in AllNewfiles)
    {
        fileCryptHelper.DecryptFile(File);
    }
}