using System;
using System.Collections.Generic;

namespace WebAppluisChaves.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public long? Price { get; set; }
        public int? Amount { get; set; }
        public int? Discount { get; set; }
    }
}
