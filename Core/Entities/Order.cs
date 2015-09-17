using System;
using System.Collections.Generic;

namespace SomeBasicCsvApp.Core.Entities
{
    public class Order : IIdentifiableByNumber
    {
        public int GetId()
        {
            return Id;
        }

        public virtual Customer Customer { get; set; }

        public virtual DateTime OrderDate { get; set; }

        public virtual int Id { get; set; }

        public virtual IList<Product> Products { get; set; }

        public virtual int Version { get; set; }

    }
}
