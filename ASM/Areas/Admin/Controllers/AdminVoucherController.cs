using System;
using ASM.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ASM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminVoucherController : Controller
    {
        private readonly DMMContext _context;
        public INotyfService _notyfService { get; }
        public AdminVoucherController(DMMContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: AdminVoucherController
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("AccountId") == null)
                return RedirectToAction("AdminLogin", "Account");
            return View(await _context.Vouchers.ToListAsync());
        }

        // GET: AdminVoucherController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var voucher = await _context.Vouchers.FirstOrDefaultAsync(p=>p.VoucherId == id);
            if (voucher == null)
            {
                return NotFound();
            }
            if (HttpContext.Session.GetString("AccountId") == null)
                return RedirectToAction("AdminLogin", "Account");
            return View(voucher);
        }

        // GET: AdminVoucherController/Create
        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetString("AccountId") == null)
                return RedirectToAction("AdminLogin", "Account");
            return View();
        }

        // POST: AdminVoucherController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (voucher.VoucherType == 1)
                        voucher.DiscountValue = Math.Min(100, voucher.DiscountValue);
                    else
                        voucher.DiscountValue = Math.Max(0, voucher.DiscountValue);
                    if (voucher.Description == null)
                        voucher.Description = "ABC";
                    _context.Add(voucher);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Tạo mới thành công!");
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return BadRequest();
                }
            }
            return View();
        }

        // GET: AdminVoucherController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var voucher = await _context.Vouchers.FindAsync(id);
                if (voucher == null)
                {
                    return NotFound();
                }
                if (HttpContext.Session.GetString("AccountId") == null)
                    return RedirectToAction("AdminLogin", "Account");
                return View(voucher);
            }
            catch
            {
                return BadRequest();
            }
        }

        // POST: AdminVoucherController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Voucher voucher)  
        {
            if (id != voucher.VoucherId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (voucher.VoucherType == 1)
                        voucher.DiscountValue = Math.Min(100, voucher.DiscountValue);
                    else
                        voucher.DiscountValue = Math.Max(0, voucher.DiscountValue);
                    _context.Update(voucher);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Chỉnh sửa thành công!");
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return BadRequest();
                }
            }
            return View(voucher);
        } 
    }
}
