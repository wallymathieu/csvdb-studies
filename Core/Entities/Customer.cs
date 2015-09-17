using System.Collections.Generic;

namespace SomeBasicCsvApp.Core.Entities
{
    public class Customer : IIdentifiableByNumber
    {
        public int GetId()
        {
            return Id;
        }

        public virtual int Id { get; set; }

        public virtual string Firstname { get; set; }

        public virtual string Lastname { get; set; }

        public virtual IList<Order> Orders { get; set; }

        public virtual int Version { get; set; }

    }
}
