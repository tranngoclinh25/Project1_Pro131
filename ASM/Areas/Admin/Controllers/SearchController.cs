using ASM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class SearchController : Controller
    {
        private readonly DMMContext _context;

        public SearchController(DMMContext context)
        {
            _context = context;
        }
        //Scaffold-DbContext "Data Source=DESKTOP-D08A7VN;Initial Catalog=DMM;Integrated Security=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -force -verbose

        [HttpPost]
        public IActionResult FindProduct(string keyword)
        {
            List<Product> ls = new List<Product>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListProductsSearchPartial", null);
            }
            ls = _context.Products.AsNoTracking()
                                  .Include(a => a.Cate)
                                  .Where(x => x.ProductName.Contains(keyword))
                                  .OrderByDescending(x => x.ProductName)
                                  .Take(10)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("ListProductsSearchPartial", null);
            }
            else
            {
                return PartialView("ListProductsSearchPartial", ls);
            }
        }
    }
}
