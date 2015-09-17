using System;
using System.IO;

namespace SomeBasicCsvApp.Core
{
    public interface ISessionFactory:IDisposable
    {
        ISession OpenSession();
    }
    public class SessionFactory:ISessionFactory
    {
        private readonly string path;
        public SessionFactory(string path)
        {
            this.path = path;
        }
        public static ISessionFactory CreateTestSessionFactory(string path){
            if (!Directory.Exists(path)){
                Directory.CreateDirectory(path);
            }
            return new SessionFactory(path);
        }

        public ISession OpenSession()
        {
            return new Session(path);
        }
            
        public void Dispose()
        {
            //TODO
        }

    }
}

