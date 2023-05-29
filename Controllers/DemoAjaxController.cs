using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class DemoAjaxController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: DemoAjax
        public ActionResult DemoAjax()
        {
            return View();
        }

        public ActionResult LoadAjaxActionLink()
        {      
            return Content("Hello Ajax");
        }

        public ActionResult LoadAjaxBeginForm(FormCollection f)
        {
            string KQ = f["txt1"].ToString();
            return Content(KQ);
        }

        public ActionResult LoadAjaxJQuery(int a,int b)
        {
            int v = a + b;
            string kq = v.ToString();
            return Content(kq);
        }

        public ActionResult LoadSanPhamAjax()
        {
            var lstSanPham = db.SanPhams;
            return PartialView(lstSanPham);
        }
    }
}