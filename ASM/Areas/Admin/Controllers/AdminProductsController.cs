using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM.Models;
using PagedList.Core;
using ASM.Helpper;
using System.IO;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;

namespace ASM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminProductsController : Controller
    {
        private readonly DMMContext _context;
        public INotyfService _notyfService { get; }


        public AdminProductsController(DMMContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;

        }

        // GET: Admin/AdminProducts
        public IActionResult Index(int page = 1, int CateId = 0)
        {
            var pageNumber = page; /*== null || page <= 0 ? 1 : page.Value*/;
            var pageSize = 20;

            List<Product> lsProducts = new List<Product>();
            if (CateId != 0)
            {
                lsProducts = _context.Products
                 .AsNoTracking()
                 .Where(x => x.CateId == CateId)
                 .Include(x => x.Cate)
                 .OrderBy(x => x.ProductId).ToList();
            }
            else
            {
                lsProducts = _context.Products
                 .AsNoTracking()
                 .Include(x => x.Cate)
                 .OrderByDescending(x => x.CreateDate).ToList();
            }
          
            PagedList<Product> models = new PagedList<Product>(lsProducts.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentCateId = CateId;
            ViewBag.CurrentPage = pageNumber;


            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CateId", "CateName", CateId);
            if (HttpContext.Session.GetString("AccountId") == null)
                return RedirectToAction("AdminLogin", "Account");
            return View(models);
        }
        public IActionResult Filtter(int CateID = 0)
        {
            var url = $"/Admin/AdminProducts?CateId={CateID}";
            if (CateID == 0)
            {
                url = $"/Admin/AdminProducts";
            }
            return Json(new { status = "success", redirectUrl = url });
        }
        // GET: Admin/AdminProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cate)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            if (HttpContext.Session.GetString("AccountId") == null)
                return RedirectToAction("AdminLogin", "Account");
            return View(product);
        }

        // GET: Admin/AdminProducts/Create
        public IActionResult Create()
        {
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CateId", "CateName");
            if (HttpContext.Session.GetString("AccountId") == null)
                return RedirectToAction("AdminLogin", "Account");
            return View();
        }

        // POST: Admin/AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ShortDesc,Description,CateId,Price,Discount,Img,Video,CreateDate,Modified,BestSellers,HomeFlag,Active,Tags,Title,Alias,MetaDesc,MetaKey,SoLuongConLai")] Product product, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                product.ProductName = Utilities.ToTitleCase(product.ProductName);
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(product.ProductName) + extension;
                    product.Img = await Utilities.UploadFile(fThumb, @"products", image.ToLower());
                }
                if (product.SoLuongConLai == null)
                {
                    product.SoLuongConLai = 10;
                }
                if (product.CateId == null)
                {
                    product.CateId = 11;
                }
                if (product.Price == null)
                {
                    product.Price = 10000;
                }
                if (product.Discount > 0)
                {
                    product.BestSellers = true;
                }
                if (product.Discount >= product.Price)
                {
                    ModelState.AddModelError("Discount", "Giá giảm phải nhỏ hơn giá bán!");
                    return View(product);
                }
                if (product.Description == null)
                {
                    product.Description = product.ProductName;
                }
                if (string.IsNullOrEmpty(product.Img)) product.Img = "default.jpg";
                product.Alias = Utilities.SEOUrl(product.ProductName);
                product.Modified = DateTime.Now;
                product.CreateDate = DateTime.Now;

                _context.Add(product);
                
                await _context.SaveChangesAsync();
                _context.ProductImages.Add(new ProductImage() { ProductId = product.ProductId, ImageUrl = product.Img, IsImage = true });
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CateId", "CateName");
            return View();
            /*if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CateId", "CateName", product.CateId);
            return View(product);*/
        }

        // GET: Admin/AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CateId", "CateName", product.CateId);
            if (HttpContext.Session.GetString("AccountId") == null)
                return RedirectToAction("AdminLogin", "Account");
            return View(product);
        }

        // POST: Admin/AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ShortDesc,Description,CateId,Price,Discount,Img,Video,CreateDate,Modified,BestSellers,HomeFlag,Active,Tags,Title,Alias,MetaDesc,MetaKey,SoLuongConLai")] Product product, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.ProductName = Utilities.ToTitleCase(product.ProductName);
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string image = Utilities.SEOUrl(product.ProductName) + extension;
                        product.Img = await Utilities.UploadFile(fThumb, @"products", image.ToLower());
                    }
                    if (string.IsNullOrEmpty(product.Img)) product.Img = "default.jpg";
                    product.Alias = Utilities.SEOUrl(product.ProductName);
                    product.Modified = DateTime.Now;
                    if (product.Discount >= product.Price)
                    {
                        ModelState.AddModelError("Discount", "Giá giảm phải nhỏ hơn giá bán!");
                        return View(product);
                    }
                    if (product.Discount > 0)
                    {
                        product.BestSellers = true;
                    }
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    var productImage = await _context.ProductImages.FirstOrDefaultAsync(p =>
                        p.ProductId == product.ProductId && p.IsImage == true);
                    if (productImage != null)
                    {
                        productImage.ImageUrl = product.Img;
                        _context.ProductImages.Update(productImage);
                        await _context.SaveChangesAsync();
                    }
                    _notyfService.Success("Chỉnh sửa thành công");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CateId", "CateName", product.CateId);
            return View(product);
        }

        // GET: Admin/AdminProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cate)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            if (HttpContext.Session.GetString("AccountId") == null)
                return RedirectToAction("AdminLogin", "Account");
            return View(product);
        }

        // POST: Admin/AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
