using ASM.Models;
using ASM.ModelViews;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.Controllers
{
    public class DonHangController : Controller
    {
        private readonly DMMContext _context;
        public INotyfService _notyfService { get; }
        public DonHangController(DMMContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        [HttpPost]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var taikhoanID = HttpContext.Session.GetString("CustomerId");
                if (string.IsNullOrEmpty(taikhoanID)) return RedirectToAction("Login", "Accounts");
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                if (khachhang == null) return NotFound();
                var donhang = await _context.Oders
                    .Include(x => x.TransactStatus)
                    .FirstOrDefaultAsync(m => m.OderId == id && Convert.ToInt32(taikhoanID) == m.CustomerId);
                if (donhang == null) return NotFound();

                var chitietdonhang = _context.OderDetails
                    .Include(x => x.Product)
                    .AsNoTracking()
                    .Where(x => x.OderId == id)
                    .OrderBy(x => x.OderDetailId)
                    .ToList();
                XemDonHang donHang = new XemDonHang();
                donHang.DonHang = donhang;
                donHang.ChiTietDonHang = chitietdonhang;
                return PartialView("Details", donHang);

            }
            catch
            {
                return NotFound();
            }
        }
    }
}
