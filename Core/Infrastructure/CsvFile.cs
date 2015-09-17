using System;
using System.IO;
using CsvHelper.Configuration;
using System.Collections.Generic;
using CsvHelper;
using System.Text.RegularExpressions;

namespace SomeBasicCsvApp.Core
{
    public class CsvFile
    {
        private static CsvConfiguration conf = new CsvConfiguration{
            Delimiter=";"
        };
        public static Stream Write<T>(Stream stream, IEnumerable<T> data)
        {
            var w = new StreamWriter(stream);
            var writer = new CsvWriter(w,conf);
            writer.WriteRecords(data);
            w.Flush();
            return stream;
        }

        public static IEnumerable<T> Read<T>(Stream data)
        {
            using (var reader = new CsvReader(new StreamReader(data),conf))
            {
                while (reader.Read())
                {
                    yield return reader.GetRecord<T>();
                }
            }
        }

        public static Stream Append<T>(Stream m, IEnumerable<T> data)
        {
            m.Seek(-3, SeekOrigin.End);
            //read 
            var r = new StreamReader(m);
            var last= r.ReadToEnd();

            var w = new StreamWriter(m);
            var writer = new CsvWriter(w,conf);
            if (!Regex.Match(last, "[\n\r]$").Success)
            {
                w.WriteLine();
            }
            foreach (var period in data)
            {
                writer.WriteRecord(period);
            }
            w.Flush();
            return m;
        }
    }
}
