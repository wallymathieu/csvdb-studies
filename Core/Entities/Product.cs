using System.Collections.Generic;

namespace SomeBasicCsvApp.Core.Entities
{
    public class Product : IIdentifiableByNumber
    {
        public int GetId()
        {
            return Id;
        }

        public virtual float Cost { get; set; }

        public virtual string Name { get; set; }

        public virtual IList<Order> Orders { get; set; }

        public virtual int Id { get; set; }

        public virtual int Version { get; set; }


    }
}
