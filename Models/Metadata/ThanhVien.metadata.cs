using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApplication1.Models
{
    [MetadataTypeAttribute(typeof(ThanhVienMetadata))]
    public partial class ThanhVien
    {
        internal sealed class ThanhVienMetadata
        {
            [DisplayName("Mã thành viên")]
            public int MaThanhVien { get; set; }
            [DisplayName("Tài khoản")]
            [Required(ErrorMessage = "Nhập vào {0}")]
            public string TaiKhoan { get; set; }
            [DisplayName("Mật khẩu")]
            [Required(ErrorMessage = "Nhập vào {0}")]
            public string MatKhau { get; set; }
            [DisplayName("Họ tên")]
            [Required(ErrorMessage = "Nhập vào {0}")]
            public string HoTen { get; set; }
            [DisplayName("Địa chỉ")]
            public string DiaChi { get; set; }
            [DisplayName("Email")]
            [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = " {0} không hợp lệ")]
            public string Email { get; set; }
            [DisplayName("Số điện thoại")]
            [Required(ErrorMessage = "Nhập vào {0}")]
            public string SoDienThoai { get; set; }
            [DisplayName("Câu hỏi")]
            public string CauHoi { get; set; }
            [DisplayName("Câu trả lời")]
            public string CauTraLoi { get; set; }
            [DisplayName("Mã loại thành viên")]
            public Nullable<int> MaLoaiTV { get; set; }
            
        }
    }
}