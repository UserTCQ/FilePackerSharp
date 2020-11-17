# FilePackerSharp
A simple file packing and unpacking library for C#.

## Usage
### Packing
```CSharp
using System.IO;
using FilePackerSharp;

using (Packer _packer = new Packer()) 
{
  FileStream[] files = Packer.GetFileStreams("/path/to/files"); //get all filestreams for all files
  _packer.PackFiles(files, File.Open("/path/to/new_file.dat", FileMode.Create)); //pack into one file
}
```
### Unpacking
```CSharp
using System.IO;
using FilePackerSharp;

using (Unpacker _unpacker = new Unpacker()) 
{
  _unpacker.UnpackFile(File.Open("/path/to/new_file.dat", FileMode.Open), "/path/to/files"); //unpack one file to multiple files
}
```

## License
[MIT](https://choosealicense.com/licenses/mit/)
