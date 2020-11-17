using System;
using System.IO;
using System.Text;

namespace FilePackerSharp
{
    public class Unpacker : IDisposable
    {
        private FileStream _input;

        /// <summary>
        /// Unpack data file into multiple files
        /// </summary>
        public void UnpackFile(FileStream _input, string _directory)
        {
            this._input = _input;

            while (_input.Position < _input.Length)
            {
                byte[] buffer;

                //read file name length
                buffer = new byte[4];
                _input.Read(buffer, 0, 4);

                int _fileNameLength = BitConverter.ToInt32(buffer, 0);

                //read file name
                buffer = new byte[_fileNameLength];
                _input.Read(buffer, 0, _fileNameLength);

                string _fileName = Encoding.UTF8.GetString(buffer);

                //read file size
                buffer = new byte[4];
                _input.Read(buffer, 0, 4);

                long _fileSize = BitConverter.ToInt32(buffer, 0);

                //read file data
                buffer = new byte[(int)_fileSize];
                _input.Read(buffer, 0, (int)_fileSize);

                //write to file
                File.WriteAllBytes(_directory + $"/{_fileName}", buffer);
            }
        }

        //free unmanaged memory
        public void Dispose()
        {
            _input.Dispose();
        }
    }
}
