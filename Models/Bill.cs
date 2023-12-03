using System;
using System.Collections.Generic;

namespace WebAppluisChaves.Models
{
    public partial class Bill
    {
        public long Id { get; set; }
        public long IdUser { get; set; }
        public string ListObject { get; set; } = null!;
        public long? Amount { get; set; }

        public virtual User IdUserNavigation { get; set; } = null!;
    }
}
