using System;
using System.Collections.Generic;

#nullable disable

namespace ASM.Models
{
    public partial class OderDetail
    {
        public int OderDetailId { get; set; }
        public int? OderId { get; set; }
        public int? ProductId { get; set; }
        public int? OderNumber { get; set; }
        public int? Quantity { get; set; }
        public int? Discount { get; set; }
        public int Total { get; set; }
        public DateTime? ShipDate { get; set; }
        public int? Amount { get; set; }
        public int? TotalMoney { get; set; }
        public int? Price { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Oder Oder { get; set; }
        public virtual Product Product { get; set; }
    }
}
