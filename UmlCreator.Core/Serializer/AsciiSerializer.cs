using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UmlCreator.Core.Serializer
{
    internal class AsciiSerializer : ISerializer<string>
    {
        public void Serialize(string obj, FileInfo fileInfo)
        {
            if (!fileInfo.Exists)
            {
                throw new IOException($"{fileInfo.FullName} is not found.");
            }

            using StreamWriter streamWriter = fileInfo.AppendText();
            streamWriter.WriteLine(obj);
        }
    }
}
