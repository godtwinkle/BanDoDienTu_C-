using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using System.Web.Security;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: Home
        public ActionResult Index()
        {
            var lstDTM = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == true && n.DaXoa == false).ToList();
            ViewBag.ListDTM=lstDTM;

            var lstLTM = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == true && n.DaXoa == false).ToList();
            ViewBag.ListLTM = lstLTM;

            var lstMTBM = db.SanPhams.Where(n => n.MaLoaiSP == 3 && n.Moi == true && n.DaXoa == false).ToList();
            ViewBag.ListMTBM = lstMTBM;

            return View();
        }

        public ActionResult MenuPartial()
        {
            var lstSP = db.SanPhams;

            return PartialView(lstSP);
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(ThanhVien tv,FormCollection f)
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());

            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                if (ModelState.IsValid)
                {
                    ViewBag.ThongBao = "Thêm thành công";
                    db.ThanhViens.Add(tv);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.ThongBao = "Thêm thất bại";
                }
                return View();

            }

            ViewBag.ThongBao = "Sai mã captcha";
            return View();
        }

        

        public List<string> LoadCauHoi()
        {
            List<string> lstCauHoi = new List<string>();
            lstCauHoi.Add("Con vật mà bạn yêu thích");
            lstCauHoi.Add("Ca sĩ mà bạn yêu thích");
            lstCauHoi.Add("Công việc hiện tại của bạn");
            lstCauHoi.Add("Người yêu bạn tên là gì");
            lstCauHoi.Add("Tên bố hoặc mẹ của bạn");
            return lstCauHoi;
        }

        public ActionResult DangNhap(FormCollection f)
        {

            //string sTaiKhoan = f["txtTenDangNhap"].ToString();
            //string sMatKhau = f["txtMatKhau"].ToString();

            //ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
            //if(tv != null)
            //{
            //    Session["TaiKhoan"] = tv;
            //    return RedirectToAction("Index","Home");

            //}

            //return Content("Tài khoản hoặc mật khẩu không đúng");

            string taikhoan = f["txtTenDangNhap"].ToString();
            string matkhau = f["txtMatKhau"].ToString();
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == taikhoan && n.MatKhau == matkhau);
            if(tv != null)
            {
                var lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
                string Quyen = "";
                if (lstQuyen.Count() != 0)
                {
                    foreach (var item in lstQuyen)
                    {
                        Quyen += item.Quyen.MaQuyen + ",";
                    }
                    Quyen = Quyen.Substring(0, Quyen.Length - 1);
                    PhanQuyen(tv.TaiKhoan.ToString(), Quyen);
                    Session["TaiKhoan"] = tv;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Session["TaiKhoan"] = tv;
                    return RedirectToAction("Index", "Home");
                }
            }
            return Content("Tài khoản hoặc mật khẩu không đúng");

        }

        public void PhanQuyen(string TaiKhoan,string Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1, TaiKhoan, DateTime.Now, DateTime.Now.AddHours(3), false, Quyen,FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));

            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }

        public ActionResult LoiPhanQuyen()
        {
            return View();
        }

        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}