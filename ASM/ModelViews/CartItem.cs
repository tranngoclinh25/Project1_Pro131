using ASM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.ModelViews
{
    public class CartItem
    {
        public Product product { get; set; }
        public int soluong { get; set; }
        public double TotalMoney => soluong * (product.Discount > 0 ? product.Discount.Value : product.Price.Value);
            

    }
}
