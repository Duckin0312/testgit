using System;
using System.Collections.Generic;
using System.Linq;
using EventTicket.Event;
using EventTicket.Exception;
using EventTicket.Interfaces;
using EventTicket.Model.Entity;
using EventTicket.Model.Enums;
using EventTicket.Repository;

namespace EventTicket.Service
{
    public class VeService : IQuanLy<Ve>
    {
        private readonly VeRepository _veRepo;

        public VeService()
        {
            _veRepo = new VeRepository();
        }

        // Nghiep vu: Ban ve cho khach hang co phat Event
        public void BanVe(Ve veMoi, KhachHang khachHang)
        {
            if (veMoi == null)
            {
                throw new VeException("Loi: Thong tin ve khong duoc de trong!");
            }

            // Mac dinh khi ban la trang thai SOLD
            veMoi.TrangThaiVe = TrangThaiVe.SOLD;

            if (!veMoi.Validate())
            {
                EventBus.Instance.PhatSuKien(
                    new VeMuaThatBaiEvent("Thong tin ve khong hop le",
                        veMoi.MaKhachHang, veMoi.MaSuKien));
                throw new VeException("Loi: Thong tin ve khong hop le!");
            }

            if (_veRepo.KiemTraMaDaTonTai(veMoi.MaVe))
            {
                EventBus.Instance.PhatSuKien(
                    new VeMuaThatBaiEvent("Ma ve da ton tai trong he thong",
                        veMoi.MaKhachHang, veMoi.MaSuKien));
                throw new VeException("Loi: Ma ve '" + veMoi.MaVe + "' da ton tai!");
            }

            _veRepo.Them(veMoi);
            Console.WriteLine("Ban ve thanh cong cho khach hang: " + veMoi.MaKhachHang);

            // Phat su kien de cac Listener (Email, ThongBao) tu dong xu ly
            EventBus.Instance.PhatSuKien(
                new VeDaBanEvent(veMoi, khachHang, DateTime.Now));
        }

        public void BanVe(Ve veMoi)
        {
            BanVe(veMoi, null);
        }

        // Nghiep vu: Huy ve / Hoan ve
        public void HuyVe(string maVe)
        {
            Ve ve = _veRepo.TimById(maVe);
            if (ve == null)
            {
                throw new VeException("Loi: Khong tim thay ve co ma: " + maVe);
            }

            if (ve.TrangThaiVe == TrangThaiVe.CANCELLED)
            {
                throw new VeException("Loi: Ve nay da bi huy truoc do!");
            }

            if (ve.TrangThaiVe == TrangThaiVe.EXPIRED)
            {
                throw new VeException("Loi: Ve da het han, khong the huy hoan tien!");
            }

            ve.TrangThaiVe = TrangThaiVe.CANCELLED;
            _veRepo.Sua(ve);
            Console.WriteLine("Huy ve thanh cong: " + maVe);
        }

        // Lay danh sach ve theo su kien
        public List<Ve> LayVeTheoSuKien(string maSuKien)
        {
            return _veRepo.TimTheoSuKien(maSuKien);
        }

        // Lay danh sach ve da mua cua khach hang
        public List<Ve> LayVeTheoKhachHang(string maKhachHang)
        {
            return _veRepo.TimTheoKhachHang(maKhachHang);
        }

        // Tinh tong doanh thu tu cac ve da ban
        public decimal TinhTongDoanhThu()
        {
            return _veRepo.LayTatCa()
                .Where(v => v.TrangThaiVe == TrangThaiVe.SOLD && v.GiaVe != null)
                .Sum(v => v.GiaVe.Value);
        }

        // Tinh tong doanh thu theo mot su kien cu the
        public decimal TinhDoanhThuTheoSuKien(string maSuKien)
        {
            return _veRepo.TimTheoSuKien(maSuKien)
                .Where(v => v.TrangThaiVe == TrangThaiVe.SOLD && v.GiaVe != null)
                .Sum(v => v.GiaVe.Value);
        }

        public void Them(Ve entity)
        {
            BanVe(entity);
        }

        public void Sua(Ve entity)
        {
            if (entity == null || !entity.Validate())
            {
                throw new VeException("Loi: Du lieu ve khong hop le!");
            }

            if (!_veRepo.KiemTraMaDaTonTai(entity.MaVe))
            {
                throw new VeException("Loi: Khong tim thay ve can cap nhat!");
            }

            _veRepo.Sua(entity);
            Console.WriteLine("Cap nhat ve thanh cong!");
        }

        public void Xoa(string maVe)
        {
            Ve ve = _veRepo.TimById(maVe);
            if (ve == null)
            {
                throw new VeException("Loi: Khong tim thay ve can xoa!");
            }

            // Khong xoa ve da ban ma chi nen doi trang thai thanh CANCELLED
            if (ve.TrangThaiVe == TrangThaiVe.SOLD)
            {
                throw new VeException("Loi: Ve da ban khong the xoa khoi he thong, hay su dung chuc nang huy ve!");
            }

            _veRepo.Xoa(maVe);
            Console.WriteLine("Xoa ve thanh cong!");
        }

        public Ve TimById(string maVe)
        {
            return _veRepo.TimById(maVe);
        }

        public List<Ve> LayTatCa()
        {
            return _veRepo.LayTatCa();
        }
    }
}
