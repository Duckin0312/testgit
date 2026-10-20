using System;
using System.Collections.Generic;
using System.Linq;
using EventTicket.Model.Entity;
using EventTicket.Model.Enums;

namespace EventTicket.Repository
{
    public class KhachHangRepository : GenericRepository<KhachHang>
    {
        private readonly List<KhachHang> danhSachKhachHang = new List<KhachHang>();

        public KhachHangRepository()
        {
            // Du lieu mau ban dau de test chuong trinh
            KhachHang kh1 = new KhachHang("KH001", "Nguyen Van An", "0912345678", "an.nguyen@gmail.com",
                new DateTime(2000, 1, 15), "Ha Noi", LoaiKhachHang.THUONG);
            KhachHang kh2 = new KhachHang("KH002", "Tran Thi Binh", "0987654321", "binh.tran@gmail.com",
                new DateTime(1998, 5, 20), "TP Ho Chi Minh", LoaiKhachHang.VIP);

            danhSachKhachHang.Add(kh1);
            danhSachKhachHang.Add(kh2);
        }

        public override void Them(KhachHang entity)
        {
            danhSachKhachHang.Add(entity);
            Console.WriteLine("[Repo] Da them khach hang: " + entity.MaKhachHang);
        }

        public override void Sua(KhachHang entity)
        {
            for (int i = 0; i < danhSachKhachHang.Count; i++)
            {
                if (string.Equals(danhSachKhachHang[i].MaKhachHang, entity.MaKhachHang, StringComparison.OrdinalIgnoreCase))
                {
                    danhSachKhachHang[i] = entity;
                    Console.WriteLine("[Repo] Da cap nhat khach hang: " + entity.MaKhachHang);
                    return;
                }
            }
        }

        public override void Xoa(string maKhachHang)
        {
            danhSachKhachHang.RemoveAll(kh => string.Equals(kh.MaKhachHang, maKhachHang, StringComparison.OrdinalIgnoreCase));
            Console.WriteLine("[Repo] Da xoa khach hang: " + maKhachHang);
        }

        public override KhachHang TimById(string maKhachHang)
        {
            if (maKhachHang == null) return null;
            return danhSachKhachHang
                .FirstOrDefault(kh => kh.MaKhachHang != null && kh.MaKhachHang.Equals(maKhachHang, StringComparison.OrdinalIgnoreCase));
        }

        public override List<KhachHang> LayTatCa()
        {
            return new List<KhachHang>(danhSachKhachHang);
        }

        // Tim kiem khach hang theo so dien thoai
        public KhachHang TimBySdt(string sdt)
        {
            if (sdt == null) return null;
            return danhSachKhachHang
                .FirstOrDefault(kh => kh.Sdt != null && kh.Sdt.Equals(sdt.Trim()));
        }

        // Kiem tra ma khach hang da ton tai chua
        public bool KiemTraMaDaTonTai(string maKhachHang)
        {
            if (maKhachHang == null) return false;
            return danhSachKhachHang
                .Any(kh => kh.MaKhachHang != null && kh.MaKhachHang.Equals(maKhachHang, StringComparison.OrdinalIgnoreCase));
        }

        // =========================================================================
        // CÁC TRUY VẤN NGHIỆP VỤ (LINQ)
        // =========================================================================

        /// <summary>
        /// CÂU 16: Tìm những khách hàng đang sở hữu vé VIP nhưng vé chưa được sử dụng.
        /// </summary>
        public List<KhachHang> Cau16_TimKhachHangSoHuuVeVipChuaDung(VeRepository veRepo)
        {
            HashSet<string> khachVipChuaDung = veRepo.LayTatCa()
                .Where(v => v.LoaiVe == LoaiVe.VIP)
                .Where(v => v.TrangThaiVe == TrangThaiVe.SOLD || v.TrangThaiVe == TrangThaiVe.RESERVED)
                .Select(v => v.MaKhachHang)
                .ToHashSet();

            return danhSachKhachHang
                .Where(kh => khachVipChuaDung.Contains(kh.MaKhachHang))
                .ToList();
        }
    }
}
