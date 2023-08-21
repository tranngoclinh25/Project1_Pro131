using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ASM.Helpper;
using ASM.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ASM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class AdminProductImagesController : Controller
    {
        private readonly DMMContext _context;
        public INotyfService _notyfService { get; }
        public AdminProductImagesController(DMMContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.Product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            var items = await _context.ProductImages.Where(x => x.ProductId == id).ToListAsync();
            if (HttpContext.Session.GetString("AccountId") == null)
                return RedirectToAction("AdminLogin", "Account");
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(int productId, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            var pI = await _context.ProductImages.OrderByDescending(p => p.ProductImageId).FirstOrDefaultAsync();
            var i = 0;
            if (pI != null)
            {
                i = pI.ProductImageId;
            }
            var product = await _context.Products.FindAsync(productId);
            var productImages = _context.ProductImages.Where(p => p.ProductId == productId).ToList();
            var pICheck = _context.ProductImages.Where(p => p.ProductId == productId && p.IsImage == false).ToList();
            if (pICheck.Count >= 3)
            {
                _notyfService.Warning("Giới hạn 3 ảnh phụ cho 1 sản phẩm!");
                return RedirectToAction(nameof(Index), new { id = productId });
            }
            if (fThumb != null)
            {
                string extension = Path.GetExtension(fThumb.FileName);
                string image = Utilities.SEOUrl(product.ProductName) + i++ + extension;
                string newImg = await Utilities.UploadFile(fThumb, @"products", image);
                if (_context.ProductImages.Where(p => p.ProductId == productId).Count() == 0)
                {
                    _context.ProductImages.Add(new ProductImage
                    {
                        ProductId = productId,
                        ImageUrl = product.Img,
                        IsImage = true
                    });
                }
                else
                {
                    _context.ProductImages.Add(new ProductImage
                    {
                        ProductId = productId,
                        ImageUrl = newImg,
                        IsImage = false
                    });
                }
            }
            else _notyfService.Warning("Vui lòng chọn ảnh!");
            await _context.SaveChangesAsync();
            _notyfService.Success("Thêm mới thành công!");
            return RedirectToAction(nameof(Index), new { id = productId });
        }
        public async Task<IActionResult> Delete(int id, int productId)
        {
            try
            {
                var productImages = await _context.ProductImages.FindAsync(id);
                _context.ProductImages.Remove(productImages);
                await _context.SaveChangesAsync();
                _notyfService.Error("Xóa thành công!");
                return RedirectToAction(nameof(Index), new { id = productId });
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
