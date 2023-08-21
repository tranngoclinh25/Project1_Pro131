using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ASM.Models;
using ASM.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace WebShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DMMContext _context;

        public HomeController(ILogger<HomeController> logger, DMMContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        public IActionResult Index()
        {
            HomeViewVM model = new HomeViewVM();

            var lsProducts = _context.Products.AsNoTracking()
                .Where(x => x.Active == true && x.HomeFlag == true && x.SoLuongConLai > 0)
                .OrderByDescending(x => x.CreateDate)
                .ToList();

            List<ProductHomeVM> lsProductViews = new List<ProductHomeVM>();
            var lsCats = _context.Categories
                .AsNoTracking()
                .Where(x => x.Published == true)
                .OrderByDescending(x => x.Odersing)
                .ToList();

            foreach (var item in lsCats)
            {
                ProductHomeVM productHome = new ProductHomeVM();
                productHome.category = item;
                productHome.lstProducts = lsProducts.Where(x => x.CateId == item.CateId).ToList();
                lsProductViews.Add(productHome);
                model.products = lsProductViews;
                ViewBag.AllProducts = lsProducts;
            }
            var posts = _context.Posts
                    .AsNoTracking()
                    .Where(x => x.Published == true && x.IsHot == true)
                    .OrderByDescending(x => x.CreateData)
                    .Take(3)
                    .ToList();
            model.posts = posts;
            return View(model);
        }



        [Route("lien-he.html", Name = "Contact")]
        public IActionResult Contact()
        {
            return View();
        }
        [Route("gioi-thieu.html", Name = "About")]
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}