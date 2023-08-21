using ASM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.ModelViews
{
    public class ProductHomeVM
    {
        public Category category { get; set; }
        public List<Product> lstProducts { get; set; }
    }
}
