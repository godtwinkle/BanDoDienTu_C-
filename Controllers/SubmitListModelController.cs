using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class SubmitListModelController : Controller
    {
        // GET: SubmitListModel
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(PhieuNhap Model,IEnumerable<ChiTietPhieuNhap> ModelList)
        {
            return View();
        }
    }
}