using System;
using System.Collections.Generic;

namespace WebAppluisChaves.Models
{
    public partial class State
    {
        public State()
        {
            Cities = new HashSet<City>();
        }

        public int Id { get; set; }
        public string StateName { get; set; } = null!;
        public int Code { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}
