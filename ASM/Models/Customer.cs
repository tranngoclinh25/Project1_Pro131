using System;
using System.Collections.Generic;

#nullable disable

namespace ASM.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Oders = new HashSet<Oder>();
        }

        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Avatar { get; set; }
        public string Adress { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Password { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool Active { get; set; }
        public string Salt { get; set; }

        public virtual ICollection<Oder> Oders { get; set; }
    }
}
