using ASM.Extension;
using ASM.Helpper;
using ASM.Models;
using ASM.ModelViews;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DMMContext _context;
        public INotyfService _notyfService { get; }
        public CheckoutController(DMMContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (gh == default(List<CartItem>))
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }
        }
       
        [Route("checkout.html", Name = "Checkout")]
        public IActionResult Index(string returnUrl = null)
        {
            //Lay gio hang ra de xu ly
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            MuaHangVM model = new MuaHangVM();
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                model.CustomerId = khachhang.CustomerId;
                model.FullName = khachhang.FullName;
                model.Email = khachhang.Email;
                model.Phone = khachhang.Phone;
                model.Address = khachhang.Adress;
            }
            //ViewData["lsTinhThanh"] = new SelectList(_context.Locations.Where(x => x.Levels == 1).OrderBy(x => x.Type).ToList(), "Location", "Name");
            ViewBag.GioHang = cart;
            return View(model);
        }

     

        [HttpPost]
        [Route("checkout.html", Name = "Checkout")]
        public IActionResult Index(MuaHangVM muaHang, int? voucherId)
        {
            //Lay ra gio hang de xu ly
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");

            var taikhoanID = HttpContext.Session.GetString("CustomerId");

            MuaHangVM model = new MuaHangVM();
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                model.CustomerId = khachhang.CustomerId;
                model.FullName = khachhang.FullName;
                model.Email = khachhang.Email;
                model.Phone = khachhang.Phone;
                model.Address = khachhang.Adress;

                /* khachhang.LocationId = muaHang.TinhThanh;
                 khachhang.District = muaHang.QuanHuyen;
                 khachhang.Ward = muaHang.PhuongXa;*/
                khachhang.Adress = muaHang.Address;
                _context.Update(khachhang);
                _context.SaveChanges();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    //Khoi tao don hang
                    Oder donhang = new Oder();
                    donhang.CustomerId = model.CustomerId;
                    donhang.Address = model.Address;
                    donhang.VoucherId = voucherId;

                    donhang.OderDate = DateTime.Now;
                    donhang.TransactStatusId = 2;//Cho xac nhan
                    donhang.Delected = false;
                    donhang.Paid = false;
                    donhang.Note = Utilities.StripHTML(model.Note);
                    //Voucher
                    var voucher = _context.Vouchers.Find(voucherId);
                    var total = 0;
                    foreach (var item in cart)
                    {
                        if(item.product.Discount > 0)
                        {
                            total += Convert.ToInt32(item.product.Discount * item.soluong);
                        }
                        else
                        {
                            total += Convert.ToInt32(item.product.Price * item.soluong);
                        }
                    }

                    if (voucher != null)
                    {
                        if (voucher.VoucherType == 0)
                            donhang.TotalMoney = total -(voucher.DiscountValue);
                        else
                            donhang.TotalMoney = Convert.ToInt32(total * (1 - (voucher.DiscountValue / (double)100)));
                        voucher.Quantity--;
                        _context.Update(voucher);
                    }
                    else
                        donhang.TotalMoney = total;

                    _context.Add(donhang);
                    _context.SaveChanges();
                    //tao danh sach don hang

                    foreach (var item in cart)
                    {
                        OderDetail orderDetail = new OderDetail();
                        Product product = _context.Products.FirstOrDefault(p => p.ProductId == item.product.ProductId);
                        orderDetail.OderId = donhang.OderId;
                        orderDetail.ProductId = item.product.ProductId;
                        orderDetail.Amount = item.soluong;
                        if (product.Discount > 0)
                        {
                            orderDetail.TotalMoney = item.soluong * item.product.Discount.Value;
                            orderDetail.Price = item.product.Discount;
                        }
                        else
                        {
                            orderDetail.TotalMoney = item.soluong * item.product.Price.Value;
                            orderDetail.Price = item.product.Price;

                        }
                        orderDetail.CreateDate = DateTime.Now;
                        _context.Add(orderDetail);
                        //Tru so luong san pham khi dat hang thanh cong
                        if (product != null)
                        {
                            product.SoLuongConLai -= item.soluong;
                            _context.Update(product);
                        }
                        if (product.SoLuongConLai  <= 0)
                        {
                            product.Active = false;
                            _context.Update(product);
                        }
                    }

                    _context.SaveChanges();
                    //clear gio hang
                    HttpContext.Session.Remove("GioHang");
                    //Xuat thong bao
                    _notyfService.Success("Đặt hàng thành công");
                    //cap nhat thong tin khach hang
                    return RedirectToAction("Success");


                }
            }
            catch
            {
                //ViewData["lsTinhThanh"] = new SelectList(_context.Locations.Where(x => x.Levels == 1).OrderBy(x => x.Type).ToList(), "Location", "Name");
                ViewBag.GioHang = cart;
                return View(model);
            }
            //ViewData["lsTinhThanh"] = new SelectList(_context.Locations.Where(x => x.Levels == 1).OrderBy(x => x.Type).ToList(), "Location", "Name");
            ViewBag.GioHang = cart;
            return View(model);
        }

        [HttpPost]
        public IActionResult AddVoucher(string voucherCore)
        {
            try
            {
                VoucherViewModel _voucherViewModel = new VoucherViewModel();
                var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
                var total = cart.Sum(p => p.TotalMoney);
                var voucher = _context.Vouchers.ToList();
                foreach (var item in voucher)
                {
                    _voucherViewModel.TotalValue = total;
                    _voucherViewModel.Value = 0;
                    if (item.VoucherCode == voucherCore)
                    {
                        //Check khách hàng đã add voucher này chưa
                        var taikhoanID = HttpContext.Session.GetString("CustomerId");
                        var voucherCheck = _context.Vouchers.FirstOrDefault(p => p.VoucherCode == voucherCore);
                        var oderCustomerVoucher = _context.Oders.FirstOrDefault(p => p.CustomerId.ToString() == taikhoanID && p.VoucherId == voucherCheck.VoucherId);
                        if (taikhoanID == null)
                        {
                            _voucherViewModel.Note = "Vui lòng đăng nhập để sử dụng Voucher!";
                            return Json(_voucherViewModel);
                        }
                        if (oderCustomerVoucher != null)
                        {
                            _voucherViewModel.Note = "Tài khoản của đã dùng mã này rồi!";
                            return Json(_voucherViewModel);
                        }

                        if (item.VoucherType == 0)
                        {
                            if (item.DiscountValue > total)
                            {
                                _voucherViewModel.Note = "Mã giảm giá không thể áp dụng!";
                                return Json(_voucherViewModel);
                            }
                            _voucherViewModel.Voucher = item;
                            _voucherViewModel.TotalValue = total - item.DiscountValue;
                            _voucherViewModel.Value = item.DiscountValue;
                            _voucherViewModel.Note = $"Giảm {item.DiscountValue} VND cho toàn bộ đơn hàng";
                        }
                        else
                        {
                            if (total * (item.DiscountValue / (double)100) > total)
                            {
                                _voucherViewModel.Note = "Mã giảm giá không thể áp dụng!";
                                return Json(_voucherViewModel);
                            }
                            _voucherViewModel.Voucher = item;
                            _voucherViewModel.TotalValue = total * (1 - (item.DiscountValue / (double)100));
                            _voucherViewModel.Value = total * (item.DiscountValue / (double)100);
                            _voucherViewModel.Note = $"Giảm {item.DiscountValue}% cho toàn bộ đơn hàng";
                        }
                        return Json(_voucherViewModel);
                    }
                }
                _voucherViewModel.Note = "Mã giảm giá không tồn tại!";
                return Json(_voucherViewModel);
            }
            catch (Exception)
            {

                return Json(null);
            }
        }

        [Route("dat-hang-thanh-cong.html", Name = "Success")]
        public IActionResult Success()
        {
            try
            {
                var taikhoanID = HttpContext.Session.GetString("CustomerId");
                if (string.IsNullOrEmpty(taikhoanID))
                {
                    return RedirectToAction("Login", "Accounts", new { returnUrl = "/dat-hang-thanh-cong.html" });
                }
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                var donhang = _context.Oders
                    .Where(x => x.CustomerId == Convert.ToInt32(taikhoanID))
                    .OrderByDescending(x => x.OderDate)
                    .FirstOrDefault();
                MuaHangSuccessVM successVM = new MuaHangSuccessVM();
                successVM.FullName = khachhang.FullName;
                successVM.DonHangID = donhang.OderId;
                successVM.Phone = khachhang.Phone;
                successVM.Address = khachhang.Adress;
                /*  successVM.PhuongXa = GetNameLocation(donhang.Ward.Value);
                  successVM.TinhThanh = GetNameLocation(donhang.District.Value);
                */
                return View(successVM);
            }
            catch
            {
                return View();
            }
        }

      
        public string GetNameLocation(int idlocation)
        {
            try
            {
                var location = _context.Locations.AsNoTracking().SingleOrDefault(x => x.LocationId == idlocation);
                if (location != null)
                {
                    return location.NameWithType;
                }
            }
            catch
            {
                return string.Empty;
            }
            return string.Empty;
        }
    }
}
