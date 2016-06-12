using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using SomeBasicCsvApp.Core.Infrastructure.Internal;

namespace SomeBasicCsvApp.Core
{
    public interface ISession : IDisposable
    {
        T Get<T>(int key) where T : IIdentifiableByNumber;
        void Save<T>(T value) where T : IIdentifiableByNumber;
        IEnumerable<T> Where<T>(Func<T, bool> predicate) where T : IIdentifiableByNumber;
        bool Any<T>(Func<T, bool> predicate) where T : IIdentifiableByNumber;
        void Commit();
    }
    public class Session : ISession
    {
        private readonly string _basepath;
        private readonly Dictionary<Tuple<Type, int>, Entity> _sessionObjects;

        public Session(string basepath)
        {
            _basepath = basepath;
            _sessionObjects = new Dictionary<Tuple<Type, int>, Entity>();
        }

        private string FileName<T>()
        {
            return FileName(typeof(T));
        }
        private string FileName(Type t)
        {
            return Path.Combine(_basepath, t.Name.ToLower());
        }

        public T Get<T>(int key) where T : IIdentifiableByNumber
        {
            var r = this.Where<T>(v => v.Id == key).FirstOrDefault();
            if (ReferenceEquals(r, null))
            {
                throw new Exception("Could not find " + key + " in " + typeof(T).Name);
            }
            return r;
        }

        public void Save<T>(T value) where T : IIdentifiableByNumber
        {
            var key = Tuple.Create(value.GetType(), value.Id);
            if (!_sessionObjects.ContainsKey(key))
            {
                _sessionObjects.Add(key, Entity.New<T>(value));
            }
        }

        public IEnumerable<T> Where<T>(Func<T, bool> predicate) where T : IIdentifiableByNumber
        {
            using (var s = Streams.OpenReadOnly(FileName<T>()))
            {
                var content = CsvFile.Read<T>(s)
                    .Reverse()
                    .GroupBy(v=>v.Id)
                    .Select(v=>v.First());
                return TapCollection(content.Where(predicate).ToArray());
            }
        }

        public bool Any<T>(Func<T, bool> predicate) where T : IIdentifiableByNumber
        {
            return Where(predicate).Any();
        }

        private IEnumerable<T> TapCollection<T>(T[] collection) where T : IIdentifiableByNumber
        {
            foreach (var item in collection)
            {
                var key = Tuple.Create(item.GetType(), item.Id);
                Entity value;
                if (_sessionObjects.TryGetValue(key, out value))
                {
                    yield return (T)value.Value;
                }
                else
                {
                    _sessionObjects.Add(key, Entity.Existing(item));
                    yield return item;
                }
            }
        }

        public void Commit()
        {
            var groupedByType = _sessionObjects.GroupBy(so => so.Key.Item1);
            foreach (var typeGroup in groupedByType)
            {
                var type = typeGroup.Key;
                if (!File.Exists(FileName(type)))
                {
                    using (var s = Streams.OpenCreate(FileName(type)))
                    {
                        CsvFile.Write(s, type, typeGroup
                            .Select(e => e.Value.Value));
                    }
                }
                else
                {
                    using (var s = Streams.OpenReadWrite(FileName(type)))
                    {
                        CsvFile.Append(s, type, typeGroup
                            .Where(e=>e.Value.IsChanged())
                            .Select(e => e.Value.GetChanged()));
                    }
                }
            }
        }

        public void Dispose()
        {
            _sessionObjects.Clear();
        }
    }
}

