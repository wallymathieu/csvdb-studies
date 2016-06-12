using System;

namespace SomeBasicCsvApp.Core.Entities
{
    [Serializable]
    public class Product : IIdentifiableByNumber, IHasVersion
    {
        public virtual int Id { get; set; }

        public virtual float Cost { get; set; }

        public virtual string Name { get; set; }

        public virtual int Version { get; set; }
    }
}
