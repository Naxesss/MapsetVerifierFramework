using System;
using System.IO;

namespace MapsetVerifierFramework.objects.resources
{
    public class FileAbstraction : TagLib.File.IFileAbstraction
    {
        public string error;

        public FileAbstraction(string aFilePath)
        {
            error = null;

            ReadStream = aFilePath != null ? new FileStream(aFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite) : null;
            Name = aFilePath;
        }

        public string Name { get; }

        public Stream ReadStream { get; }

        public Stream WriteStream
        {
            get { return ReadStream; }
        }

        public void CloseStream(Stream aStream)
        {
            aStream.Position = 0;
        }

        public TagLib.File GetTagFile()
        {
            if (Name == null)
            {
                error = "Name cannot be null.";
                return null;
            }

            if (ReadStream == null)
            {
                error = "Could not open file for reading.";
                return null;
            }

            return TagLib.File.Create(this);
        }
    }
}
