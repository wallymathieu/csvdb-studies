using System;

namespace SomeBasicCsvApp.Core
{
    public interface ISessionFactory:IDisposable
    {
        ISession OpenSession();
    }
    public class SessionFactory
    {
        public SessionFactory()
        {
        }
        public static ISessionFactory CreateTestSessionFactory(string path){
            throw new NotImplementedException();
        }
    }
}

