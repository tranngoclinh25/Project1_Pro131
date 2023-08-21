using ASM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.ModelViews
{
    public class HomeViewVM
    {
        public List<Post> posts { get; set; }
        public List<ProductHomeVM> products { get; set; }
    }
}
