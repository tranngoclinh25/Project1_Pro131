using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM.Models;
using PagedList.Core;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.IO;
using ASM.Helpper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace ASM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class AdminPostsController : Controller
    {
        private readonly DMMContext _context;
        public INotyfService _notyfService { get; }

        public AdminPostsController(DMMContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;

        }

        // GET: Admin/AdminPosts
        public IActionResult Index(int? page)
        {

            var collection = _context.Posts.AsNoTracking().ToList();
            foreach (var item in collection)
            {
                if (item.CreateData == null)
                {
                    item.CreateData = DateTime.Now;
                    _context.Update(item);
                    _context.SaveChanges();
                }
            }

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsTinDangs = _context.Posts
                .AsNoTracking()
                .OrderBy(x => x.PostId);
            PagedList<Post> models = new PagedList<Post>(lsTinDangs, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            if (HttpContext.Session.GetString("AccountId") == null)
                return RedirectToAction("AdminLogin", "Account");
            return View(models);
        }

        // GET: Admin/AdminPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Account)
                .Include(p => p.Cate)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }
            if (HttpContext.Session.GetString("AccountId") == null)
                return RedirectToAction("AdminLogin", "Account");
            return View(post);
        }

        // GET: Admin/AdminPosts/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "FullName");
            ViewData["CateId"] = new SelectList(_context.Categories, "CateId", "CateId");
            if (HttpContext.Session.GetString("AccountId") == null)
                return RedirectToAction("AdminLogin", "Account");
            return View();
        }

        // POST: Admin/AdminPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Title,Contents,Img,Published,Alias,CreateData,Author,AccountId,ShortContents,Tags,CateId,IsHot,IsNewFeed")] Post post, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string imageName = Utilities.SEOUrl(post.Title) + extension;
                    post.Img = await Utilities.UploadFile(fThumb, @"news", imageName.ToLower());
                }
                if (string.IsNullOrEmpty(post.Img)) post.Img = "default.jpg";
                post.Alias = Utilities.SEOUrl(post.Title);
                post.CreateData = DateTime.Now;

                _context.Add(post);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công");

                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "FullName", post.AccountId);
            ViewData["CateId"] = new SelectList(_context.Categories, "CateId", "CateId", post.CateId);
            return View(post);
        }

        // GET: Admin/AdminPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "FullName", post.AccountId);
            ViewData["CateId"] = new SelectList(_context.Categories, "CateId", "CateId", post.CateId);
            if (HttpContext.Session.GetString("AccountId") == null)
                return RedirectToAction("AdminLogin", "Account");
            return View(post);
        }

        // POST: Admin/AdminPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,Contents,Img,Published,Alias,CreateData,Author,AccountId,ShortContents,Tags,CateId,IsHot,IsNewFeed")] Post post, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (id != post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {   //Xu ly Thumb
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string imageName = Utilities.SEOUrl(post.Title) + extension;
                        post.Img = await Utilities.UploadFile(fThumb, @"news", imageName.ToLower());
                    }
                    if (string.IsNullOrEmpty(post.Img)) post.Img = "default.jpg";
                    post.Alias = Utilities.SEOUrl(post.Title);
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Chỉnh sửa thành công");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "FullName", post.AccountId);
            ViewData["CateId"] = new SelectList(_context.Categories, "CateId", "CateId", post.CateId);
            return View(post);
        }

        // GET: Admin/AdminPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Account)
                .Include(p => p.Cate)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }
            if (HttpContext.Session.GetString("AccountId") == null)
                return RedirectToAction("AdminLogin", "Account");
            return View(post);
        }

        // POST: Admin/AdminPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");

            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
