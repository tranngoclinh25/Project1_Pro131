using System;
using System.Collections.Generic;

#nullable disable

namespace ASM.Models
{
    public partial class TransactStatus
    {
        public TransactStatus()
        {
            Oders = new HashSet<Oder>();
        }

        public int TransactStatusId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Oder> Oders { get; set; }
    }
}
