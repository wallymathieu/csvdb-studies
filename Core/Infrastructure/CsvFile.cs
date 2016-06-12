using System;
using System.IO;
using CsvHelper.Configuration;
using System.Collections.Generic;
using CsvHelper;
using System.Text.RegularExpressions;
using System.Collections;

namespace SomeBasicCsvApp.Core
{
    public class CsvFile
    {
        private static CsvConfiguration conf = new CsvConfiguration
        {
            Delimiter = ";"
        };
        public static Stream Write(Stream stream, Type type, IEnumerable data)
        {
            var w = new StreamWriter(stream);
            var writer = new CsvWriter(w, conf);
            var map = writer.Configuration.AutoMap(type);
            writer.WriteHeader(type);
            foreach (var item in data)
            {
                writer.WriteRecord(type, item);
            }
            w.Flush();
            return stream;
        }
        public static Stream Write<T>(Stream stream, IEnumerable<T> data)
        {
            return Write(stream, typeof(T), data);
        }

        public static IEnumerable<T> Read<T>(Stream data)
        {
            using (var reader = new CsvReader(new StreamReader(data), conf))
            {
                while (reader.Read())
                {
                    yield return reader.GetRecord<T>();
                }
            }
        }
        public static Stream Append<T>(Stream m, IEnumerable<T> data)
        {
            return Append(m, typeof(T), data);
        }
        public static Stream Append(Stream m, Type t, IEnumerable data)
        {
            m.Seek(-3, SeekOrigin.End);
            //read 
            var r = new StreamReader(m);
            var last = r.ReadToEnd();

            var w = new StreamWriter(m);
            var writer = new CsvWriter(w, conf);
            writer.Configuration.AutoMap(t);
            if (!Regex.Match(last, "[\n\r]+$").Success)
            {
                w.WriteLine();
            }
            foreach (var period in data)
            {
                writer.WriteRecord(t, period);
            }
            w.Flush();
            return m;
        }
    }
}
