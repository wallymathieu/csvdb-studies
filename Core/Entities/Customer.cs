﻿using System.Collections.Generic;

namespace SomeBasicCsvApp.Core.Entities
{
    public class Customer : IIdentifiableByNumber
    {
        public virtual int Id { get; set; }

        public virtual string Firstname { get; set; }

        public virtual string Lastname { get; set; }

        public virtual int Version { get; set; }
    }
}
