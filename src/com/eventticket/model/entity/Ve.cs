using System;
using EventTicket.Model.Base;
using EventTicket.Model.Enums;
using TrangThaiVeEnum = EventTicket.Model.Enums.TrangThaiVe;

namespace EventTicket.Model.Entity
{
    public class Ve : BaseEntity
    {
        private string maVe;

        public string MaKhachHang { get; set; }
        public string MaSuKien { get; set; }
        public string MaKhuVuc { get; set; }
        public LoaiVe? LoaiVe { get; set; }
        public TrangThaiVeEnum? TrangThaiVe { get; set; }
        public decimal? GiaVe { get; set; }
        public DateTime? ThoiGianMua { get; set; }

        public Ve() : base()
        {
        }

        public Ve(string maVe, string maKhachHang, string maSuKien, string maKhuVuc, LoaiVe? loaiVe,
                  TrangThaiVeEnum? trangThaiVe, decimal? giaVe, DateTime? thoiGianMua)
        {
            MaVe = maVe; // đồng thời gán Id
            MaKhachHang = maKhachHang;
            MaSuKien = maSuKien;
            MaKhuVuc = maKhuVuc;
            LoaiVe = loaiVe;
            TrangThaiVe = trangThaiVe;
            GiaVe = giaVe;
            ThoiGianMua = thoiGianMua ?? DateTime.Now;
        }

        public string MaVe
        {
            get => maVe;
            set
            {
                maVe = value;
                Id = value;
            }
        }

        public string XuatVe()
        {
            return $"Xuất vé thành công! Mã vé: {MaVe}, Giá: {GiaVe}, " +
                   $"Loại vé: {LoaiVe}, Mã khách hàng: {MaKhachHang}, Thời gian: {ThoiGianMua}. " +
                   $"Vé có hiệu lực với sự kiện {MaSuKien} tại khu vực {MaKhuVuc}";
        }

        public bool HuyVe()
        {
            if (TrangThaiVe == TrangThaiVeEnum.EXPIRED)
            {
                Console.WriteLine("Vé đã hết hạn, không thể hủy!");
                return false;
            }
            if (TrangThaiVe == TrangThaiVeEnum.CANCELLED)
            {
                Console.WriteLine("Vé đã được hủy trước đó!");
                return false;
            }
            TrangThaiVe = TrangThaiVeEnum.CANCELLED;
            Console.WriteLine("Đã hủy vé " + MaVe);
            return true;
        }

        /*
            Hàm Validate(): Kiểm tra dữ liệu input đầu vào có hợp lệ không
            Nếu không sẽ in thông báo lỗi và trả về false thay vì làm sập chương trình
         */
        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(maVe))
            {
                Console.WriteLine("Lỗi: Mã vẽ không được để trống!");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(MaKhachHang))
            {
                Console.WriteLine("Lỗi: Mã khách hàng không được để trống!");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(MaKhuVuc))
            {
                Console.WriteLine("Lỗi: Mã khu vực không được để trống!");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(MaSuKien))
            {
                Console.WriteLine("Lỗi: Mã sự kiện không được để trống!");
                return false;
            }
            else if (LoaiVe == null)
            {
                Console.WriteLine("Lỗi: Loại vé không hợp lệ (null)!");
                return false;
            }
            else if (TrangThaiVe == null)
            {
                Console.WriteLine("Lỗi: Trạng thái vé không hợp lệ (null)!");
                return false;
            }
            else if (GiaVe == null || GiaVe.Value < 0m)
            {
                Console.WriteLine("Lỗi: Giá vẽ không được null hoặc bé hơn 0.0!");
                return false;
            }
            else if (ThoiGianMua == null || ThoiGianMua > DateTime.Now)
            {
                Console.WriteLine("Lỗi: Thời gian mua không được null hoặc trễ hơn thời gian thực!");
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return maVe + " [" + MaSuKien + ", " + MaKhachHang + ", " + TrangThaiVe + ", " + GiaVe + "]";
        }
    }
}
