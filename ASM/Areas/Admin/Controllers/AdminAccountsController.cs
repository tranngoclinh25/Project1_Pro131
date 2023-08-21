using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace ASM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class AdminAccountsController : Controller
    {
        private readonly DMMContext _context;

        public AdminAccountsController(DMMContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminAccounts
        public async Task<IActionResult> Index()
        {
            
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "RoleName");

            List<SelectListItem> lsTrangThai = new List<SelectListItem>();

            lsTrangThai.Add(new SelectListItem() { Text = "Hoạt động", Value = "1" });
            lsTrangThai.Add(new SelectListItem() { Text = "Không họat động", Value = "0" });
            ViewData["lsTrangThai"] = lsTrangThai;


            var dMMContext = _context.Accounts.Include(a => a.Role);
            if (HttpContext.Session.GetString("AccountId") == null)
                return RedirectToAction("AdminLogin", "Account");
            return View(await dMMContext.ToListAsync());
        }

        // GET: Admin/AdminAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }
            if (HttpContext.Session.GetString("AccountId") == null)
                return RedirectToAction("AdminLogin", "Account");
            return View(account);
        }

        // GET: Admin/AdminAccounts/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId");
            if (HttpContext.Session.GetString("AccountId") == null)
                return RedirectToAction("AdminLogin", "Account");
            return View();
        }

        // POST: Admin/AdminAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,FullName,Email,Phone,Password,Active,CreateDate,RoleId,LastLogin")] Account account)
        {
            if (ModelState.IsValid)
            {

                account.CreateDate = DateTime.Now;
                account.LastLogin = DateTime.Now;
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", account.RoleId);

            return View(account);
        }

        // GET: Admin/AdminAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", account.RoleId);
            if (HttpContext.Session.GetString("AccountId") == null)
                return RedirectToAction("AdminLogin", "Account");
            return View(account);
        }

        // POST: Admin/AdminAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,FullName,Email,Phone,Password,Active,CreateDate,RoleId,LastLogin")] Account account)
        {
            if (id != account.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountId))
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
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", account.RoleId);
            return View(account);
        }

        // GET: Admin/AdminAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }
            if (HttpContext.Session.GetString("AccountId") == null)
                return RedirectToAction("AdminLogin", "Account");
            return View(account);
        }

        // POST: Admin/AdminAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.AccountId == id);
        }
    }
}
