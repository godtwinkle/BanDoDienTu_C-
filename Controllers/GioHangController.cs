using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: GioHang

        public List<ItemGioHang> LayGioHang()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
               lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
               
            }
            return lstGioHang;
        }

        public ActionResult ThemGioHang(int MaSP,string strURL)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if(sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<ItemGioHang> lstGioHang = LayGioHang();
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return View("ThongBao");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }
            
            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            lstGioHang.Add(itemGH);
            return Redirect(strURL);
        }

        public double TinhTongSoLuong()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);
        }

        public decimal TinhTongTien()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);

        }


        public ActionResult GioHangPartial()
        {
            if (TinhTongSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0 ;

                return PartialView();

            }

            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }

        public ActionResult XemGioHang()
        {
            List<ItemGioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }

        [HttpGet]
        public ActionResult SuaGioHang(int MaSP)
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index","Home");
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<ItemGioHang> lstGioHang = LayGioHang();

            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.GioHang = lstGioHang;
            return View(spCheck);
        }

        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        {
            SanPham spCheck = db.SanPhams.Single(n => n.MaSP == itemGH.MaSP);
            if (spCheck.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            List<ItemGioHang> lstGH = LayGioHang();
            ItemGioHang itemGHUpdate = lstGH.Find(n => n.MaSP == itemGH.MaSP);
            itemGHUpdate.SoLuong = itemGH.SoLuong;
            itemGHUpdate.ThanhTien = itemGHUpdate.SoLuong * itemGHUpdate.DonGia;
            return RedirectToAction("XemGioHang"); 
        }

        public ActionResult XoaGioHang(int MaSP)
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<ItemGioHang> lstGioHang = LayGioHang();

            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }

            lstGioHang.Remove(spCheck);

            return RedirectToAction("XemGioHang");
        }

        public ActionResult DatHang(KhachHang kh)
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            KhachHang khachHang = new KhachHang();
            ;
            if (Session["TaiKhoan"] == null)
            {
                khachHang = kh;
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
            }
            else
            {
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                khachHang.TenKH = tv.HoTen;
                khachHang.DiaChi = tv.DiaChi;
                khachHang.Email = tv.Email;
                khachHang.SoDienThoai = tv.SoDienThoai;
                khachHang.MaThanhVien = tv.MaLoaiTV;
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
            }
            DonDatHang ddh=new DonDatHang();
            ddh.MaKH = khachHang.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            ddh.DaHuy = false;
            ddh.DaXoa = false;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();

            List<ItemGioHang> lstGH = LayGioHang();
            foreach(var item in lstGH)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                db.ChiTietDonDatHangs.Add(ctdh);
            }

            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");
        }

        public ActionResult ThemGioHangAjax(int MaSP,string strURL)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            List<ItemGioHang> lstGioHang = LayGioHang();

            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return Content("<script> alert(\"Sản phẩm đã hết hàng!\")</script>");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                ViewBag.TongSoLuong = TinhTongSoLuong();
                ViewBag.TongTien = TinhTongTien();
                return PartialView("GioHangPartial");
            }
            return View();
        }
    }
}