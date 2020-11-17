using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FilePackerSharp
{
    public class Packer : IDisposable
    {
        private FileStream[] _inputs;
        private FileStream _output;

        /// <summary>
        /// Get all files as array of FileStream for reading in top directory
        /// </summary>
        public static FileStream[] GetFileStreams(string _directory)
        {
            string[] _filePaths = Directory.GetFiles(_directory, "*.*", SearchOption.TopDirectoryOnly);

            FileStream[] _streams = new FileStream[_filePaths.Length];

            for (int i = 0; i < _filePaths.Length; i++)
                _streams[i] = File.OpenRead(_filePaths[i]);

            return _streams;
        }

        /// <summary>
        /// Pack multiple files into one data file
        /// </summary>
        public void PackFiles(FileStream[] _inputs, FileStream _output)
        {
            this._inputs = _inputs;
            this._output = _output;

            if (!_output.CanWrite)
                throw new Exception("Output FileStream not writable!");

            //list of bytes
            List<byte> bytes = new List<byte>();

            for (int i = 0; i < _inputs.Length; i++)
            {
                //checks
                if (!_inputs[i].CanRead)
                    throw new Exception($"Input FileStream at index {i} not readable!");

                if (_inputs[i].Length > int.MaxValue)
                    throw new Exception($"File larger than maximum allowed of 2GB!");

                string _fileName = Path.GetFileName(_inputs[i].Name);

                //write file name length
                bytes.AddRange(BitConverter.GetBytes(_fileName.Length));

                //write file name
                bytes.AddRange(Encoding.UTF8.GetBytes(_fileName));

                //write file size
                bytes.AddRange(BitConverter.GetBytes((int)_inputs[i].Length));

                //write file data
                byte[] buffer = new byte[(int)_inputs[i].Length];
                _inputs[i].Read(buffer, 0, buffer.Length);

                bytes.AddRange(buffer);
            }

            //write to output stream
            _output.Write(bytes.ToArray(), 0, bytes.Count);
        }

        //free unmanaged resources
        public void Dispose()
        {
            foreach (var _input in _inputs)
                _input.Dispose();

            _output.Dispose();
        }
    }
}
