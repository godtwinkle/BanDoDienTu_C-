using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "QuanTri,QuanLySanPham")]

    public class KhachHangController : Controller
    {

        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
       [Authorize(Roles = "QuanLySanPham")]

        // GET: KhachHang
        public ActionResult Index()
        {
            var lstKH = db.KhachHangs;
            return View(lstKH);
        }

        [Authorize(Roles = "QuanTri")]

        public ActionResult TruyVanDoiTuong()
        {
            var lstKH = from kh in db.KhachHangs where kh.MaKH == 1 select kh;
            KhachHang KH = lstKH.FirstOrDefault();
            return View(KH);
        }

        public ActionResult SortDuLieu()
        {
            List<KhachHang> lstKH = db.KhachHangs.OrderBy(n => n.TenKH).ToList();
            return View(lstKH);
        }

        public ActionResult GroupDuLieu()
        {
            List<ThanhVien> lstTV = db.ThanhViens.OrderByDescending(n => n.TaiKhoan).ToList();
            return View(lstTV);
        }
    }
}