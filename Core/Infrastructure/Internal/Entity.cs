using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace SomeBasicCsvApp.Core.Infrastructure.Internal
{
    internal abstract class Entity
    {
        private class ExistingEntity : Entity
        {
            private readonly IIdentifiableByNumber _existingValue;

            public ExistingEntity(IIdentifiableByNumber item)
            {
                this.Value = item;
                this._existingValue = Clone(item);
            }

            /// <summary>
            /// in order to clone the object we use BinaryFormatter
            /// </summary>
            private IIdentifiableByNumber Clone(IIdentifiableByNumber item)
            {
                var formatter = new BinaryFormatter();
                using (var stream = new MemoryStream())
                {
                    formatter.Serialize(stream, item);
                    stream.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    return (IIdentifiableByNumber)formatter.Deserialize(stream);
                }
            }

            public override Type GetType()
            {
                return Type.Existing;
            }

            public override bool IsChanged()
            {
                var t = this.Value.GetType();
                var writeableProperties = t.GetProperties().Where(p => p.CanWrite).ToArray();
                foreach (var property in writeableProperties)
                {
                    if (!property.GetValue(this.Value)
                            .Equals(property.GetValue(this._existingValue)))
                    {
                        return true;
                    }
                }
                return false;
            }

            public override IIdentifiableByNumber GetChanged()
            {
                var hasVersion = Value as IHasVersion;
                if (hasVersion!=null)
                {
                    hasVersion.Version += 1;
                }
                return this.Value;
            }
        }

        private class NewEntity : Entity
        {
            public NewEntity(IIdentifiableByNumber item)
            {
                this.Value = item;
            }

            public override IIdentifiableByNumber GetChanged()
            {
                return Value;
            }

            public override Type GetType()
            {
                return Type.New;
            }

            public override bool IsChanged()
            {
                return true;
            }
        }

        public IIdentifiableByNumber Value { get; protected set; }
        public new abstract Type GetType();
        public abstract bool IsChanged();

        public static Entity Existing<T>(T item) where T : IIdentifiableByNumber
        {
            return new ExistingEntity(item);
        }
        public static Entity New<T>(T item) where T : IIdentifiableByNumber
        {
            return new NewEntity(item);
        }
        public enum Type
        {
            Existing,
            New
        }

        public abstract IIdentifiableByNumber GetChanged();
    }
}
