using System;
using System.Collections.Generic;

namespace SomeBasicCsvApp.Core.Entities
{
    [Serializable]
    public class Customer : IIdentifiableByNumber, IHasVersion
    {
        public virtual int Id { get; set; }

        public virtual string Firstname { get; set; }

        public virtual string Lastname { get; set; }

        public virtual int Version { get; set; }
    }
}
