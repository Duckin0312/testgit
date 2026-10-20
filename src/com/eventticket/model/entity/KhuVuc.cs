using System;
using EventTicket.Model.Base;
using EventTicket.Model.Enums;

namespace EventTicket.Model.Entity
{
    public class KhuVuc : BaseEntity
    {
        public string TenKhuVuc { get; set; }
        public LoaiKhuVuc? LoaiKhuVuc { get; set; }
        public int SucChua { get; set; }

        public KhuVuc() : base()
        {
        }

        public KhuVuc(string maKhuVuc, string tenKhuVuc, LoaiKhuVuc? loaiKhuVuc, int sucChua)
        {
            Id = maKhuVuc;
            TenKhuVuc = tenKhuVuc;
            LoaiKhuVuc = loaiKhuVuc;
            SucChua = sucChua;
        }

        // chỉ có getter
        public string MaKhuVuc => Id;

        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                Console.WriteLine("Lỗi: Mã khu vực không tồn tại!");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(TenKhuVuc))
            {
                Console.WriteLine("Lỗi: Tên khu vực không được trống!");
                return false;
            }
            else if (LoaiKhuVuc == null)
            {
                Console.WriteLine("Lỗi: Loại khu vực không hợp lệ!");
                return false;
            }
            else if (SucChua <= 0)
            {
                Console.WriteLine("Lỗi: Sức chứa phải lớn hơn 0!");
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return TenKhuVuc + " [" + LoaiKhuVuc + ", " + SucChua + " chỗ]";
        }
    }
}
