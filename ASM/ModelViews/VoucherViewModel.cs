using ASM.Models;

namespace ASM.ModelViews
{
    public class VoucherViewModel
    {
        public Voucher Voucher { get; set; }
        public double Value { get; set; }
        public double TotalValue { get; set; }
        public string Note { get; set; }
    }
}
