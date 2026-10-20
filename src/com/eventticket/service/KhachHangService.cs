using System;
using System.Collections.Generic;
using System.Linq;
using EventTicket.Interfaces;
using EventTicket.Model.Entity;
using EventTicket.Repository;

namespace EventTicket.Service
{
    public class KhachHangService : IQuanLy<KhachHang>
    {
        private readonly KhachHangRepository _khachHangRepo;

        public KhachHangService()
        {
            _khachHangRepo = new KhachHangRepository();
        }

        // Tim khach hang theo so dien thoai
        public KhachHang TimBySdt(string sdt)
        {
            if (string.IsNullOrWhiteSpace(sdt))
            {
                return null;
            }
            return _khachHangRepo.TimBySdt(sdt);
        }

        // Tim kiem danh sach khach hang theo ten
        public List<KhachHang> TimKiemTheoTen(string tenCanTim)
        {
            if (string.IsNullOrWhiteSpace(tenCanTim))
            {
                return new List<KhachHang>();
            }
            string tenLower = tenCanTim.Trim().ToLower();
            return _khachHangRepo.LayTatCa()
                .Where(kh => kh.HoTen != null && kh.HoTen.ToLower().Contains(tenLower))
                .ToList();
        }

        // Xem lich su mua ve cua mot khach hang
        public List<Ve> LayLichSuMuaVe(string maKhachHang)
        {
            KhachHang kh = _khachHangRepo.TimById(maKhachHang);
            if (kh == null)
            {
                Console.WriteLine("Loi: Khong tim thay khach hang co ma: " + maKhachHang);
                return new List<Ve>();
            }
            return kh.GetLichSuMuaVe();
        }

        public void Them(KhachHang khMoi)
        {
            if (khMoi == null)
            {
                throw new ArgumentException("Loi: Du lieu khach hang khong duoc de trong!");
            }

            // Kiem tra hop le du lieu thuc the
            if (!khMoi.Validate())
            {
                throw new ArgumentException("Loi: Thong tin khach hang khong hop le!");
            }

            // Kiem tra trung ma khach hang
            if (_khachHangRepo.KiemTraMaDaTonTai(khMoi.MaKhachHang))
            {
                throw new ArgumentException("Loi: Ma khach hang '" + khMoi.MaKhachHang + "' da ton tai!");
            }

            // Kiem tra trung so dien thoai
            if (khMoi.Sdt != null && _khachHangRepo.TimBySdt(khMoi.Sdt) != null)
            {
                throw new ArgumentException("Loi: So dien thoai '" + khMoi.Sdt + "' da duoc dang ky boi khach hang khac!");
            }

            _khachHangRepo.Them(khMoi);
            Console.WriteLine("Them khach hang moi thanh cong!");
        }

        public void Sua(KhachHang khCanSua)
        {
            if (khCanSua == null || !khCanSua.Validate())
            {
                throw new ArgumentException("Loi: Du lieu cap nhat khong hop le!");
            }

            if (!_khachHangRepo.KiemTraMaDaTonTai(khCanSua.MaKhachHang))
            {
                throw new ArgumentException("Loi: Khong tim thay khach hang can cap nhat!");
            }

            _khachHangRepo.Sua(khCanSua);
            Console.WriteLine("Cap nhat thong tin khach hang thanh cong!");
        }

        public void Xoa(string maKhachHang)
        {
            KhachHang kh = _khachHangRepo.TimById(maKhachHang);
            if (kh == null)
            {
                throw new ArgumentException("Loi: Khong tim thay khach hang can xoa!");
            }

            // Neu khach hang da co ve da mua thi khong cho xoa de bao toan du lieu ve
            if (kh.GetLichSuMuaVe().Count > 0)
            {
                throw new ArgumentException("Loi: Khong the xoa khach hang da co lich su mua ve!");
            }

            _khachHangRepo.Xoa(maKhachHang);
            Console.WriteLine("Xoa khach hang thanh cong!");
        }

        public KhachHang TimById(string maKhachHang)
        {
            return _khachHangRepo.TimById(maKhachHang);
        }

        public List<KhachHang> LayTatCa()
        {
            return _khachHangRepo.LayTatCa();
        }
    }
}
