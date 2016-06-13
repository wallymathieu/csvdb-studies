using System;
using System.IO;
using CsvHelper.Configuration;
using System.Collections.Generic;
using CsvHelper;
using System.Text.RegularExpressions;
using System.Collections;

namespace SomeBasicCsvApp.Core.Infrastructure
{
    public class CsvFile
    {
        private static CsvConfiguration GetConf()
        {
            return new CsvConfiguration
            {
                Delimiter = ";"
            };
        }
        public static Stream Write(Stream stream, Type type, IEnumerable data)
        {
            var conf = GetConf();
            var w = new StreamWriter(stream);
            var writer = new CsvWriter(w, conf);
            writer.Configuration.AutoMap(type);
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
            var conf = GetConf();
            conf.AutoMap<T>();
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
            var conf = GetConf();
            conf.AutoMap(t);
            var writer = new CsvWriter(w, conf);
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
