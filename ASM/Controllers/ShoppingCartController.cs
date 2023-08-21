using ASM.Extension;
using ASM.Models;
using ASM.ModelViews;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly DMMContext _context;
        public INotyfService _notyfService { get; }
        public ShoppingCartController(DMMContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public List<CartItem> GioHang //dinh nghia gio hang
        {
            get
            {
                var giohang = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (giohang == default(List<CartItem>))
                {
                    giohang = new List<CartItem>();
                }
                return giohang;
            }
        }

        /*
        1.Thêm mới sản phẩm vào giỏ
        2.Cập nhật lại số lượng sản phẩm trong giỏ
        3.Xóa sản phẩm khỏi giỏ hàng
        4.Xóa luôn cả giỏ hàng
         
         */

        /* [HttpPost]
         [Route("api/cart/add")]
         public IActionResult AddToCart(int productID, int? soluong)
         {
             List<CartItem> gioHang = GioHang;
             try
             {
                 //Them san pham vao gio hang
                 CartItem item = GioHang.SingleOrDefault(p => p.product.ProductId == productID);
                 if (item != null) // da co => cap nhat so luong
                 {
                     if (soluong.HasValue)
                     {
                         item.soluong = soluong.Value;
                     }
                     else
                     {
                         item.soluong++; 
                     }

                 }
                 else
                 {
                     Product sp = _context.Products.SingleOrDefault(p => p.ProductId == productID);
                     item = new CartItem
                     {
                         soluong = soluong.HasValue ? soluong.Value : 1,
                         product = sp
                     };
                     GioHang.Add(item);//Them vao gio
                 }

                 //Luu lai Session
                 HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
                 _notyfService.Success("Thêm sản phẩm thành công");
                 return Json(new { success = true });
             }
             catch
             {
                 return Json(new { success = false });
             }
         }
 */
        [HttpPost]
        [Route("api/cart/add")]
        public IActionResult AddToCart(int productID, int? amount)
        {
            List<CartItem> cart = GioHang;
            Product product = _context.Find<Product>(productID);

            try
            {
                //Them san pham vao gio hang
                CartItem item = cart.SingleOrDefault(p => p.product.ProductId == productID);

                if (product.SoLuongConLai < amount )
                {
                    _notyfService.Error("Số lượng sản phẩm không đủ");
                    return Json(new { success = false });

                }
                if (product.Active == false)
                {
                    _notyfService.Error("Sản phẩm đã ngừng bán");
                    return Json(new { success = false });
                }
                if (item != null) // da co => cap nhat so luong
                {
                    /* if (amount.HasValue)
                     {
                         item.soluong = amount.Value;
                     }
                     else
                     {
                         item.soluong++;
                     }*/
                    
                    item.soluong = item.soluong + amount.Value;
                    //luu lai session
                    _notyfService.Success("Thêm sản phẩm thành công");
                    HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
                    _notyfService.Success("Thêm sản phẩm thành công");

                }
                else
                {
                    Product hh = _context.Products.SingleOrDefault(p => p.ProductId == productID);
                    item = new CartItem
                    {
                        soluong = amount.HasValue ? amount.Value : 1,
                        product = hh
                    };
                    _notyfService.Success("Thêm sản phẩm thành công");
                    cart.Add(item);//Them vao gio
                    _notyfService.Success("Thêm sản phẩm thành công");

                }

                //Luu lai Session
                _notyfService.Success("Thêm sản phẩm thành công");
                HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
                _notyfService.Success("Thêm sản phẩm thành công");
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        [Route("api/cart/update")]
        public IActionResult UpdateCart(int productID, int? soLuong)
        {
            //Lay san pham trong gio hang ra de xu ly
            var gioHang = HttpContext.Session.Get<List<CartItem>>("GioHang");
            try
            {
                if (gioHang != null)
                {
                    CartItem item = gioHang.SingleOrDefault(p => p.product.ProductId == productID);
                    if (item != null && soLuong.HasValue) // da co -> cap nhat so luong
                    {
                        item.soluong = soLuong.Value;
                    }
                    //Luu lai session
                    HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
                }
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [Route("api/cart/remove")]
        public ActionResult Remove(int productID)
        {
            try
            {
                List<CartItem> gioHang = GioHang;
                CartItem item = gioHang.SingleOrDefault(p => p.product.ProductId == productID);
                if (item != null)
                {
                    gioHang.Remove(item);
                }
                //luu lai session
                _notyfService.Success("Xoá sản phẩm thành công");
                HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [Route("cart.html", Name = "Cart")]
        public IActionResult Index()
        {

            return View(GioHang);
        }

    }
}
