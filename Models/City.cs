using System;
using System.Collections.Generic;

namespace WebAppluisChaves.Models
{
    public partial class City
    {
        public City()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string CityName { get; set; } = null!;
        public int StateId { get; set; }

        public virtual State State { get; set; } = null!;
        public virtual ICollection<User> Users { get; set; }
    }
}
