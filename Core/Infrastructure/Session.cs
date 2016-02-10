using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;


namespace SomeBasicCsvApp.Core
{
    public interface ISession : IDisposable
    {
        T Get<T>(int key) where T : IIdentifiableByNumber;
        void Save<T>(T value);
        IQueryable<T> QueryOver<T>();
        void Commit();
    }
    public class Session : ISession
    {
        private readonly string basepath;
        public Session(string basepath)
        {
            this.basepath = basepath;
        }

        private string FileName<T>()
        {
            return Path.Combine(basepath, typeof(T).Name.ToLower());
        }
        public T Get<T>(int key) where T : IIdentifiableByNumber
        {
            var r = this.QueryOver<T>().SingleOrDefault(v => v.GetId() == key);
            if (ReferenceEquals(r, null))
            {
                throw new Exception("Could not find " + key + " in " + typeof(T).Name);
            }
            return r;
        }

        private List<Command> commands = new List<Command>();
        abstract class Command
        {
            public abstract void Apply();// todo: should not know how to apply itself, it's a command, should be able to rewrite the command stream
        }
        class SaveIt<T> : Command
        {
            private readonly T value;
            private readonly Session session;//todo: Should not know of session
            public SaveIt(T value, Session session)
            {
                this.value = value;
                this.session = session;
            }
            public override void Apply()
            {
                if (!File.Exists(session.FileName<T>()))
                {
                    using (var s = Streams.OpenCreate(session.FileName<T>()))
                    {
                        CsvFile.Write<T>(s, new[] { value });
                    }
                }
                else
                {
                    using (var s = Streams.OpenReadWrite(session.FileName<T>()))
                    {
                        CsvFile.Append<T>(s, new[] { value });
                    }
                }
            }
        }

        public void Save<T>(T value)
        {
            commands.Add(new SaveIt<T>(value, this));
        }

        public IQueryable<T> QueryOver<T>()
        {
            using (var s = Streams.OpenReadOnly(FileName<T>()))
            {
                var content = CsvFile.Read<T>(s).ToArray();
                return content.AsQueryable();
            }
        }

        public void Commit()
        {
            foreach (var item in commands)
            {
                item.Apply();
            }
            commands.Clear();
        }

        public void Dispose()
        {
        }

    }
}

