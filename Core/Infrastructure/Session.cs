using System;
using System.Linq;
namespace SomeBasicCsvApp.Core
{
    public interface ISession:IDisposable
    {
        T Get<T>(object key);
        void Save<T>(T value);
        IQueryable<T> QueryOver<T>();
        void Close();
    }
    public class Session
    {
        public Session()
        {
        }
    }
}

