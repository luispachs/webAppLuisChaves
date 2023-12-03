using System;
using System.Collections.Generic;

namespace WebAppluisChaves.Models
{
    public partial class User
    {
        public User()
        {
            Bills = new HashSet<Bill>();
        }

        public long Id { get; set; }
        public long Nit { get; set; }
        public string BusinessName { get; set; } = null!;
        public string LocalAddress { get; set; } = null!;
        public int City { get; set; }
        public string Phone { get; set; } = null!;

        public virtual City CityNavigation { get; set; } = null!;
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
