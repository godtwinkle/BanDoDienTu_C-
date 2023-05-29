using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ThongKeController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: ThongKe
        public ActionResult Index()
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();
            ViewBag.SoNguoiDangOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();
            ViewBag.TongDoanhThu = ThongKeTongDoanhThu();
            ViewBag.TongDDH = ThongKeDonHang();
            ViewBag.TongThanhVien = ThongKeThanhVien();
            return View();
        }

        public double ThongKeDonHang()
        {
            double ddh = db.DonDatHangs.Count();
            return ddh;

        }


        public double ThongKeThanhVien()
        {
            double tv = db.DonDatHangs.Count();
            return tv;

        }

        public decimal ThongKeTongDoanhThu()
        {
            decimal TongDoanhThu =  db.ChiTietDonDatHangs.Sum(n => n.SoLuong * n.DonGia).Value;
            return TongDoanhThu;
        }

        public decimal ThongKeTongDoanhThuThang(int Thang, int Nam)
        {
            var lstDDH = db.DonDatHangs.Where(n => n.NgayDat.Value.Month == Thang && n.NgayDat.Value.Year == Nam);
            decimal TongTien = 0;
            foreach(var item in lstDDH)
            {
                TongTien += decimal.Parse(item.ChiTietDonDatHangs.Sum(n => n.SoLuong * n.DonGia).Value.ToString());
            }

            return TongTien;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                }
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}