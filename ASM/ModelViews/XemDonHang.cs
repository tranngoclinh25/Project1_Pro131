using ASM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.ModelViews
{
    public class XemDonHang
    {
        public Oder DonHang { get; set; }
        public List<OderDetail> ChiTietDonHang { get; set; }
    }
}
