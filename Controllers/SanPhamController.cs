using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class SanPhamController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: SanPham
        [ChildActionOnly]
        public ActionResult SanPhamStylePartial()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult SanPhamStyle2Partial()
        {
            return PartialView();
        }

        public ActionResult XemChiTiet(int? id,string tensp)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id && n.DaXoa == false);
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }


        public ActionResult SanPhamTheoLoai(int? page, int? MaLoaiSP, int? MaNSX)
        {
            if (MaLoaiSP == null || MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX).OrderBy(n=>n.DonGia);
            if (lstSP.Count() == 0)
            {
                return HttpNotFound();
            }

            ViewBag.MaLoai = MaLoaiSP;
            ViewBag.MaLoai = MaNSX;

            ViewBag.lstSP = db.SanPhams.ToList();

            return View(lstSP.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult SanPham(int? MaLoaiSP, int? MaNSX)
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("Index","Home");
            }
            if (MaLoaiSP == null || MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX);
            if (lstSP.Count() == 0)
            {
                return HttpNotFound();
            }
            return View(lstSP);
        }


    }
}