
# FileCryptOperator

This tool is an command line program that provides an impletation of the aes algorithm

## Features

- Encrypt single File
- Encrypt whole Folder
- Decrypt single File
- Decrypt whole Folder


## Usage/Examples

To Decrypt an Single File (You need to change the Path)
```bash
FileCryptOperator.exe --Decrypt --Path SomePath --Password "SomeSecure" --Mode File --Size Strong
```

To Decrypt an whole Folder (You need to change the Path)
```bash
FileCryptOperator.exe --Decrypt --Path SomePath --Password "SomeSecure" --Mode Folder --Size Strong
```

To Encrypt an single File (You need to change the Path)
```bash
FileCryptOperator.exe --Encrypt --Path SomePath --Password "SomeSecure" --Mode File --Size Strong
```

To Encrypt an whole Folder (You need to change the Path)
```bash
FileCryptOperator.exe --Encrypt --Path SomePath --Password "SomeSecure" --Mode Folder --Size Strong
```

## Tech Stack

This tool is Build on .Net 6 so it can be runned on Linux/Windows
