using System;
using System.Linq;
using System.IO;


namespace SomeBasicCsvApp.Core
{
    public interface ISession:IDisposable
    {
        T Get<T>(int key) where T: IIdentifiableByNumber;
        void Save<T>(T value) ;
        IQueryable<T> QueryOver<T>();
        void Close();
    }
    public class Session:ISession
    {
        private readonly string basepath;
        public Session(string basepath)
        {
            this.basepath = basepath;
        }

        private string FileName<T>(){
            return Path.Combine(basepath, typeof(T).Name.ToLower());
        }
        public T Get<T>(int key) where T: IIdentifiableByNumber
        {
            using (var s = Streams.OpenReadOnly( FileName<T>())){
                var content = CsvFile.Read<T>(s);
                var r = content.SingleOrDefault(v => v.Id == key);
                if (Equals(r,null))
                {
                    throw new Exception("Could not find "+key+" in "+typeof(T).Name);
                }
                return r;
            }
        }

        public void Save<T>(T value)
        {
            if (!File.Exists(FileName<T>()))
            {
                using (var s = Streams.OpenCreate(FileName<T>()))
                {
                    CsvFile.Write<T>(s,new []{ value });
                }
            }
            else
            {
                using (var s = Streams.OpenReadWrite(FileName<T>()))
                {
                    CsvFile.Append<T>(s,new []{ value });
                }
            }
        }

        public IQueryable<T> QueryOver<T>()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
        }
            
        public void Dispose()
        {
        }

    }
}

