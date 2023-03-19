using FileCryptClass;
using FileCryptOperator;
using System.CommandLine;

string IVDefault = "abcdefghijklmnopqrstuvwxyz";

RootCommand Main = new RootCommand();

Option<string> Path = new Option<string>("--Path", "Specify the Path to operate")
{
    IsRequired = true
};
Option<string> Password = new Option<string>("--Password", "Enter the password that was used to encrypt")
{
    IsRequired = true
};
Option<string> IV = new Option<string>("--IV", "Enter the iv value to randomize")
{
    IsRequired = false
};
Option<Mode> Mode = new Option<Mode>("--Mode", "Set the operate Mode (File or Folder)")
{
    IsRequired = true
};
Option<AESSize> Size = new Option<AESSize>("--Size", "Enter the length of the AES Key")
{
    IsRequired = true
};

Command Decrypt = new Command("--Decrypt", "This Command decrypt files or folders");
Decrypt.AddOption(Path);
Decrypt.AddOption(Password);
Decrypt.AddOption(IV);
Decrypt.AddOption(Mode);
Decrypt.AddOption(Size);

Decrypt.SetHandler((Path, Password, IV, Mode, Size) =>
{
    if (IV == null)
    {
        IV = IVDefault;
    }
    using (FileCryptHelper _helper = new FileCryptHelper(Size, Password, IV))
    {
        if (Mode == Mode.File)
        {
            _helper.DecryptFile(Path);
        }
        else if (Mode == Mode.Folder)
        {
            string[] AllFiles = Directory.GetFiles(Path);
            foreach (string File in AllFiles)
            {
                _helper.DecryptFile(File);
            }
        }
    }
}, Path, Password, IV, Mode, Size);

Main.AddCommand(Decrypt);

Command Encrypt = new Command("--Encrypt", "This Command encrypt files or folders");
Encrypt.AddOption(Path);
Encrypt.AddOption(Password);
Encrypt.AddOption(IV);
Encrypt.AddOption(Mode);
Encrypt.AddOption(Size);

Encrypt.SetHandler((Path, Password, IV, Mode, Size) =>
{
    if(IV == null)
    {
        IV = IVDefault;
    }
    using (FileCryptHelper _helper = new FileCryptHelper(Size, Password, IV))
    {
        if (Mode == Mode.File)
        {
            _helper.EncryptFile(Path);
        }
        else if (Mode == Mode.Folder)
        {
            string[] AllFiles = Directory.GetFiles(Path);
            foreach(string File in AllFiles)
            {
                _helper.EncryptFile(File);
            }
        }
    }
}, Path, Password, IV, Mode, Size);

Main.AddCommand(Encrypt);
Main.Invoke(args);