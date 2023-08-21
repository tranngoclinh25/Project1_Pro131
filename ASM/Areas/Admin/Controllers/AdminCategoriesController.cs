using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM.Models;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;
using PagedList.Core;
using System.IO;
using ASM.Helpper;
using Microsoft.AspNetCore.Http;

namespace ASM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class AdminCategoriesController : Controller
    {
        private readonly DMMContext _context;
        public INotyfService _notyfService { get; }
        public AdminCategoriesController(DMMContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminCategories
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsCategorys = _context.Categories
                .AsNoTracking()
                .OrderBy(x => x.CateName);
            PagedList<Category> models = new PagedList<Category>(lsCategorys, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
           
            return View(models);
        }

        // GET: Admin/AdminCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CateId == id);
            if (category == null)
            {
                return NotFound();
            }
           
            return View(category);
        }

        // GET: Admin/AdminCategories/Create
        public IActionResult Create()
        {
          
            return View();
        }

        // POST: Admin/AdminCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CateId,CateName,Title,MetaDesc,MetaKey,Img,Published,Odersing,Parents,Levels,Icon,Cover,Description,Alias")] Category category, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {  //Xu ly Thumb
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string imageName = Utilities.SEOUrl(category.CateName) + extension;
                    category.Img = await Utilities.UploadFile(fThumb, @"category", imageName.ToLower());
                }
                if (category.Alias == null)
                {
                    category.Alias = Utilities.SEOUrl(category.CateName);
                }
                if (string.IsNullOrEmpty(category.Img)) category.Img = "default.jpg";
                _context.Add(category);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công");

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/AdminCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
         
            return View(category);
        }

        // POST: Admin/AdminCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CateId,CateName,Title,MetaDesc,MetaKey,Img,Published,Odersing,Parents,Levels,Icon,Cover,Description,Alias")] Category category, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (id != category.CateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string imageName = Utilities.SEOUrl(category.CateName) + extension;
                        category.Img = await Utilities.UploadFile(fThumb, @"category", imageName.ToLower());
                    }
                    if (string.IsNullOrEmpty(category.Img)) category.Img = "default.jpg";
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Chỉnh sửa thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CateId))
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
            return View(category);
        }

        // GET: Admin/AdminCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CateId == id);
            if (category == null)
            {
                return NotFound();
            }
            
            return View(category);
        }

        // POST: Admin/AdminCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CateId == id);
        }
    }
}
