using System;
using System.Collections.Generic;

#nullable disable

namespace ASM.Models
{
    public partial class Oder
    {
        public Oder()
        {
            OderDetails = new HashSet<OderDetail>();
        }

        public int OderId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? OderDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public int TransactStatusId { get; set; }
        public bool Delected { get; set; }
        public bool Paid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string OderCode { get; set; }
        public string Note { get; set; }
        public int? TotalMoney { get; set; }
        public string Address { get; set; }
        public int? VoucherId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual TransactStatus TransactStatus { get; set; }
        public virtual Voucher Voucher { get; set; }
        public virtual ICollection<OderDetail> OderDetails { get; set; }
    }
}
