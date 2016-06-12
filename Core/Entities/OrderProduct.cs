using System;

namespace SomeBasicCsvApp.Core
{
    [Serializable]
    public class OrderProduct:IIdentifiableByNumber
    {
        public OrderProduct()
        {
        }

        public virtual int Id { get; set; }
        public virtual int OrderId { get; set; }
        public virtual int ProductId { get; set; }
    }
}

