using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class QuanLyDonHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: QuanLyDonHang
        public ActionResult ChuaThanhToan()
        {
            var lst = db.DonDatHangs.Where(n => n.DaThanhToan == false).OrderBy(n => n.NgayDat);
            return View(lst);            
        }

        public ActionResult ChuaGiao()
        {
            var lstDHCG = db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == false&&n.DaThanhToan==true).OrderBy(n => n.NgayGiao);
            
            return View(lstDHCG);
        }

        public ActionResult DaGiaoDaThanhToan()
        {
            var lstDHCG=db.DonDatHangs.Where(n=>n.TinhTrangGiaoHang==true&&n.DaThanhToan==true);
            return View(lstDHCG) ;
        }
        [HttpGet]
        public ActionResult DuyetDonDatHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            DonDatHang model = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            if(model==null)
            {
                return HttpNotFound();
            }

            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(model);
        }

        [HttpPost]
        public ActionResult DuyetDonDatHang(DonDatHang ddh)
        {
            DonDatHang ddhUpdate = db.DonDatHangs.Single(n => n.MaDDH == ddh.MaDDH);
            ddhUpdate.DaThanhToan = ddh.DaThanhToan;
            ddhUpdate.TinhTrangGiaoHang = ddh.TinhTrangGiaoHang;
            db.SaveChanges();

            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == ddh.MaDDH);
            ViewBag.ListChiTietDH = lstChiTietDH;
            GuiEmail("Xác nhận đơn hàng của hệ thống", "tnl04122001@gmail.com", "czsir3623@iperfectmail.com", "", "Đơn hàng của bạn đã đặt thành công");
            return View(ddhUpdate);
        }

        public void GuiEmail(string Title,string ToEmail,string FromEmail,string PassWord,string Content)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail);
            mail.From = new MailAddress(ToEmail);
            mail.Subject = Title;
            mail.Body = Content;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials=new System.Net.NetworkCredential(FromEmail,PassWord);
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
}