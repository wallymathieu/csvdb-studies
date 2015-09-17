using System;

namespace SomeBasicCsvApp.Core
{
    public class OrderProduct
    {
        public OrderProduct()
        {
        }
        public virtual int OrderId { get; set; }
        public virtual int ProductId { get; set; }
    }
}

