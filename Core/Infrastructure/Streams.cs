using System;
using System.IO;

namespace SomeBasicCsvApp.Core
{
    public class Streams
    {
        public static Stream ToStream(string data)
        {
            var m = new MemoryStream();
            var w = new StreamWriter(m);
            w.Write(data);
            w.Flush();
            m.Seek(0, SeekOrigin.Begin);
            return m;
        }

        public static string ToString(Stream m)
        {
            m.Seek(0, SeekOrigin.Begin);
            using (var r = new StreamReader(m))
            {
                return r.ReadToEnd();
            }
        }

        public static FileStream OpenCreate(string fileName)
        {
            return new FileStream(fileName, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
        }
        public static FileStream OpenReadOnly(string fileName)
        {
            return new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }
        public static FileStream OpenReadWrite(string fileName)
        {
            return new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
        }

    }
}

