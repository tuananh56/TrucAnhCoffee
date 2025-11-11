using Coffee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;
using PagedList;

namespace Coffee.Controllers
{
    public class OrderController : Controller
    {
        private readonly CafeDBEntities db = DbContextSingleton.Instance.DbContext;

        // 1️⃣ Hiển thị danh sách đơn hàng
        /*public ActionResult Index(string search, string status)
        {
            var orders = db.Orders.Include(o => o.User).OrderByDescending(o => o.OrderDate).AsQueryable();

            // Lọc theo trạng thái
            if (!string.IsNullOrEmpty(status))
            {
                orders = orders.Where(o => o.Status.Trim().ToLower() == status.Trim().ToLower());
            }

            // Tìm kiếm theo OrderID, Tên user, SĐT
            if (!string.IsNullOrEmpty(search))
            {
                orders = orders.Where(o =>
                    o.OrderID.ToString().Contains(search) ||
                    o.User.FullName.Contains(search) ||
                    o.User.Phone.Contains(search));
            }

            ViewBag.Search = search;
            ViewBag.Status = status;

            return View(orders.ToList());
        }*/

        public ActionResult Index(string search, string status)
        {
            var command = new GetOrdersCommand(db)
                            .WithSearch(search)
                            .WithStatus(status);

            var orders = command.Execute().ToList();

            ViewBag.Search = search;
            ViewBag.Status = status;

            return View(orders);
        }



        // 2️⃣ Xem chi tiết đơn hàng
        public ActionResult Details(int id)
        {
            var order = db.Orders
                          .Include(o => o.User)
                          .Include(o => o.OrderDetails.Select(od => od.Product)) // Load thông tin sản phẩm
                          .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return HttpNotFound(); // Trả về lỗi nếu không tìm thấy đơn hàng
            }

            ViewBag.OrderDetails = order.OrderDetails?.ToList(); // Gán danh sách sản phẩm vào ViewBag

            return View(order); // Truyền order làm Model cho View
        }
        public ActionResult XemDH()
        {
            if (Session["UserID"] == null)
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để xem đơn hàng!";
                return RedirectToAction("Login", "Account"); // Chuyển hướng đến trang đăng nhập
            }

            // Kiểm tra xem UserID có thể ép kiểu int không
            if (!int.TryParse(Session["UserID"].ToString(), out int userId))
            {
                TempData["ErrorMessage"] = "Lỗi xác thực người dùng, vui lòng đăng nhập lại!";
                return RedirectToAction("Login", "Account");
            }

            var orders = db.Orders
                           .Where(o => o.UserID == userId)
                           .Include(o => o.OrderDetails.Select(od => od.Product))
                           .ToList();

            return View(orders);
        }



        public ActionResult XemHoaDon(int orderId)
        {
            int userId = (int)Session["UserID"]; // Lấy UserID từ session

            var order = db.Orders
                          .Where(o => o.OrderID == orderId && o.UserID == userId) // Chỉ lấy hóa đơn của chính user
                          .Include(o => o.OrderDetails.Select(od => od.Product))
                          .FirstOrDefault();

            if (order == null)
            {
                return HttpNotFound(); // Trả về 404 nếu đơn hàng không tồn tại hoặc không thuộc về user
            }

            var invoice = new InvoiceViewModel
            {
                OrderID = order.OrderID,
                CustomerName = order.User.FullName,
                Email = order.User.Email,
                OrderDate = order.OrderDate ?? DateTime.MinValue,
                TotalAmount = order.TotalAmount,
                PaymentMethod = order.PaymentMethod,
                Status = order.Status,
                Items = order.OrderDetails.Select(od => new InvoiceItemViewModel
                {
                    ProductName = od.Product.Name,
                    Quantity = od.Quantity,
                    Price = od.Price
                }).ToList()
            };

            return View(invoice);
        }








        // 3️⃣ Cập nhật trạng thái đơn hàng
        /*[HttpPost]
        public ActionResult UpdateStatus(int id, string status)
        {
            Console.WriteLine("Trạng thái nhận được: " + status);

            var validStatuses = new List<string> { "Chua giao", "Dang giao", "Da giao", "Giao hang that bai" };

            if (!validStatuses.Contains(status))
            {
                return Content("Lỗi: Trạng thái không hợp lệ");
            }

            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            order.Status = status;
            db.SaveChanges();

            return RedirectToAction("Details", new { id = id });
        }*/
        [HttpPost]
        public ActionResult UpdateStatus(int id, string status)
        {
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            Console.WriteLine("Trạng thái cũ: " + order.Status);

            IOrderState newState;
            switch (status)
            {
                case "Chua giao":
                    newState = new ChuaGiaoState();
                    break;
                case "Dang giao":
                    newState = new DangGiaoState();
                    break;
                case "Da giao":
                    newState = new DaGiaoState();
                    break;
                case "Giao hang that bai":
                    newState = new GiaoHangThatBaiState();
                    break;
                default:
                    return Content("<script>alert('Lỗi: Trạng thái không hợp lệ'); window.history.back();</script>", "text/html");
            }

            try
            {
                order.SetState(newState);
                db.SaveChanges();
            }
            catch (InvalidOperationException ex)
            {
                return Content($"<script>alert('{ex.Message}'); window.history.back();</script>", "text/html");
            }

            Console.WriteLine("Trạng thái mới: " + order.Status);
            return RedirectToAction("Details", new { id = id });
        }

        // 4️⃣ Xóa đơn hàng (tùy chọn)
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var order = db.Orders.Find(id);
            if (order != null)
            {
                db.OrderDetails.RemoveRange(db.OrderDetails.Where(od => od.OrderID == id)); // Xóa chi tiết đơn hàng
                db.Orders.Remove(order);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Checkout(string paymentMethod)
        {
            int userId = GetLoggedInUserId();
            var cartItems = db.Carts.Where(c => c.UserID == userId).Include(c => c.Product).ToList();

            if (!cartItems.Any())
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index", "Cart");
            }

            decimal totalAmount = cartItems.Sum(c => c.Quantity * c.Product.Price);

            if (paymentMethod == "VNPay")
            {
                string vnpayUrl = GetVNPayUrl(totalAmount, userId);
                return Redirect(vnpayUrl); // Chuyển hướng trực tiếp đến VNPay
            }

            // Nếu chọn COD
            var order = new Order
            {
                UserID = userId,
                OrderDate = DateTime.Now,
                PaymentMethod = "COD",
                Status = "Chờ xác nhận",
                TotalAmount = totalAmount
            };

            db.Orders.Add(order);
            db.SaveChanges();

            // Lưu chi tiết đơn hàng
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    OrderID = order.OrderID,  // Gán OrderID vừa tạo
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                };
                db.OrderDetails.Add(orderDetail);
            }
            db.SaveChanges();

            db.Carts.RemoveRange(cartItems);
            db.SaveChanges();

            TempData["Success"] = "Đặt hàng thành công! Bạn sẽ thanh toán khi nhận hàng.";
            return RedirectToAction("OrderSuccess", new { orderId = order.OrderID });
        }





        public ActionResult OrderSuccess(int orderId)
        {
            var order = db.Orders.FirstOrDefault(o => o.OrderID == orderId);
            if (order == null)
            {
                return HttpNotFound();
            }

            ViewBag.OrderID = orderId;
            ViewBag.PaymentMethod = order.PaymentMethod; // Lấy phương thức thanh toán từ DB

            return View();
        }


        public ActionResult ListOrders()
        {
            int userId = GetLoggedInUserId();
            var orders = db.Orders.Where(o => o.UserID == userId).ToList();
            return View(orders);
        }


        private int GetLoggedInUserId()
        {
            if (Session["UserID"] == null)
            {
                throw new UnauthorizedAccessException("Người dùng chưa đăng nhập.");
            }
            return (int)Session["UserID"];
        }

        private string GetVNPayUrl(decimal amount, int userId)
        {
            string vnp_ReturnUrl = "http://localhost:5000/Cart/VNPayReturn"; // Cập nhật đúng URL callback
            string vnp_TmnCode = "YOUR_VNPAY_TMNCODE"; // Thay bằng mã TMN thực tế
            string vnp_HashSecret = "YOUR_VNPAY_SECRET"; // Thay bằng secret thực tế

            var pay = new VnPayLibrary();
            pay.AddRequestData("vnp_Version", "2.1.0");
            pay.AddRequestData("vnp_Command", "pay");
            pay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            pay.AddRequestData("vnp_Amount", ((int)(amount * 100)).ToString()); // VNPay yêu cầu nhân 100 và làm tròn
            pay.AddRequestData("vnp_CurrCode", "VND");
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); // Mã đơn hàng duy nhất
            pay.AddRequestData("vnp_OrderInfo", $"Thanh toán đơn hàng của User {userId}");
            pay.AddRequestData("vnp_Locale", "vn");
            pay.AddRequestData("vnp_ReturnUrl", vnp_ReturnUrl);
            pay.AddRequestData("vnp_IpAddr", Request.UserHostAddress ?? "127.0.0.1");

            // Bật chế độ quét mã QR
            pay.AddRequestData("vnp_Bill_Mobile", "0123456789");
            pay.AddRequestData("vnp_Bill_Email", "example@email.com");
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss"));

            // Tạo URL thanh toán QR Code
            string paymentUrl = pay.CreateRequestUrl("https://sandbox.vnpayment.vn/paymentv2/vpcpay.html", vnp_HashSecret);

            return paymentUrl;
        }

        public ActionResult ShowQRCode(string qrUrl)
        {
            ViewBag.QrUrl = qrUrl;
            return View();
        }
        // Lịch sử mua hàng của người dùng
        public ActionResult OrderHistory(string search, string status)
        {
            int userId = GetLoggedInUserId();
            var orders = db.Orders
                           .Where(o => o.UserID == userId)
                           .OrderByDescending(o => o.OrderDate)
                           .ToList();

            // Kiểm tra giá trị status nhận được
            Console.WriteLine($"Status Filter: {status}");

            // Nếu có từ khóa tìm kiếm, lọc đơn hàng
            if (!string.IsNullOrEmpty(search))
            {
                orders = orders.Where(o =>
                    o.OrderID.ToString().Contains(search) ||
                    o.OrderDetails.Any(od => od.Product.Name.Contains(search))
                ).ToList();
            }

            // Lọc theo trạng thái nếu có giá trị
            if (!string.IsNullOrEmpty(status))
            {
                orders = orders.Where(o => o.Status.Trim().ToLower() == status.Trim().ToLower()).ToList();
            }

            ViewBag.Search = search;
            ViewBag.Status = status;

            return View(orders);
        }
        public ActionResult CheckoutNow(int productId)
        {
            var product = db.Products.Find(productId);
            if (product == null || product.Stock <= 0)
            {
                return RedirectToAction("Index", "Product");
            }

            // Tạo đơn hàng mới
            var order = new Order
            {
                UserID = Session["UserID"] as int?,
                OrderDate = DateTime.Now,
                TotalAmount = product.Price,
                Status = "Chờ thanh toán"
            };
            db.Orders.Add(order);
            db.SaveChanges();

            // Thêm sản phẩm vào chi tiết đơn hàng
            var orderDetail = new OrderDetail
            {
                OrderID = order.OrderID,
                ProductID = productId,
                Quantity = 1,
                Price = product.Price
            };
            db.OrderDetails.Add(orderDetail);
            db.SaveChanges();

            // Chuyển đến trang thanh toán
            return RedirectToAction("Checkout", "Order", new { orderId = order.OrderID });
        }
        [HttpPost]
        public ActionResult DeleteSelected(int[] selectedIds)
        {
            if (selectedIds != null && selectedIds.Any())
            {
                foreach (var id in selectedIds)
                {
                    var order = db.Orders.Find(id);
                    if (order != null)
                    {
                        db.Orders.Remove(order);
                    }
                }
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}