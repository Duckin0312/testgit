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
    public class SuKienService : IQuanLy<SuKien>
    {
        private readonly SuKienRepository _suKienRepo;

        public SuKienService()
        {
            _suKienRepo = new SuKienRepository();
        }

        // Tim kiem su kien theo ten (tim kiem gan dung)
        public List<SuKien> TimKiemTheoTen(string tenCanTim)
        {
            if (string.IsNullOrWhiteSpace(tenCanTim))
            {
                return new List<SuKien>();
            }
            string tenLower = tenCanTim.Trim().ToLower();
            return _suKienRepo.LayTatCa()
                .Where(sk => sk.TenSuKien != null && sk.TenSuKien.ToLower().Contains(tenLower))
                .ToList();
        }

        // Loc danh sach su kien dang dien ra / dang ban ve
        public List<SuKien> LaySuKienDangDienRa()
        {
            return _suKienRepo.LocTheoTrangThai(TrangThaiSuKien.ONGOING);
        }

        // Loc danh sach su kien sap dien ra
        public List<SuKien> LaySuKienSapDienRa()
        {
            return _suKienRepo.LocTheoTrangThai(TrangThaiSuKien.UPCOMING);
        }

        // Nghiep vu: Huy su kien
        public void HuySuKien(string maSuKien)
        {
            SuKien sk = _suKienRepo.TimById(maSuKien);
            if (sk == null)
            {
                throw new SuKienException("Loi: Khong tim thay su kien co ma: " + maSuKien);
            }

            if (sk.TrangThaiSuKien == TrangThaiSuKien.COMPLETED)
            {
                throw new SuKienException("Loi: Su kien da ket thuc, khong the huy!");
            }

            sk.TrangThaiSuKien = TrangThaiSuKien.CANCELLED;
            _suKienRepo.Sua(sk);
            Console.WriteLine("Da huy su kien: " + sk.TenSuKien);

            // Phat su kien de LogListener tu dong ghi log canh bao
            EventBus.Instance.PhatSuKien(
                new SuKienHuyEvent(sk, "Huy theo yeu cau cua quan tri vien"));
        }

        // Nghiep vu: Bat dau su kien / Chuyen sang dang dien ra
        public void BatDauSuKien(string maSuKien)
        {
            SuKien sk = _suKienRepo.TimById(maSuKien);
            if (sk == null)
            {
                throw new SuKienException("Loi: Khong tim thay su kien co ma: " + maSuKien);
            }

            if (sk.TrangThaiSuKien == TrangThaiSuKien.CANCELLED)
            {
                throw new SuKienException("Loi: Su kien da bi huy, khong the bat dau!");
            }

            sk.TrangThaiSuKien = TrangThaiSuKien.ONGOING;
            _suKienRepo.Sua(sk);
            Console.WriteLine("Su kien da chuyen sang trang thai DANG DIEN RA: " + sk.TenSuKien);
        }

        public void Them(SuKien skMoi)
        {
            if (skMoi == null)
            {
                throw new SuKienException("Loi: Du lieu su kien khong duoc de trong!");
            }

            // Mac dinh khi tao moi la UPCOMING neu chua co trang thai
            // (Neu TrangThaiSuKien la enum khong nullable, hay doi thanh TrangThaiSuKien? trong entity)
            if (skMoi.TrangThaiSuKien == null)
            {
                skMoi.TrangThaiSuKien = TrangThaiSuKien.UPCOMING;
            }

            if (!skMoi.Validate())
            {
                throw new SuKienException("Loi: Thong tin su kien khong hop le!");
            }

            // Kiem tra ma da ton tai
            if (_suKienRepo.KiemTraMaDaTonTai(skMoi.MaSuKien))
            {
                throw new SuKienException("Loi: Ma su kien '" + skMoi.MaSuKien + "' da ton tai!");
            }

            // Kiem tra thoi gian dien ra phai o tuong lai
            if (skMoi.ThoiGianDienRa != null && skMoi.ThoiGianDienRa < DateTime.Now)
            {
                throw new SuKienException("Loi: Thoi gian dien ra su kien phai o tuong lai!");
            }

            _suKienRepo.Them(skMoi);
            Console.WriteLine("Them su kien moi thanh cong: " + skMoi.TenSuKien);
        }

        public void Sua(SuKien skCanSua)
        {
            if (skCanSua == null || !skCanSua.Validate())
            {
                throw new SuKienException("Loi: Du lieu cap nhat khong hop le!");
            }

            if (!_suKienRepo.KiemTraMaDaTonTai(skCanSua.MaSuKien))
            {
                throw new SuKienException("Loi: Khong tim thay su kien can cap nhat!");
            }

            _suKienRepo.Sua(skCanSua);
            Console.WriteLine("Cap nhat thong tin su kien thanh cong!");
        }

        public void Xoa(string maSuKien)
        {
            SuKien sk = _suKienRepo.TimById(maSuKien);
            if (sk == null)
            {
                throw new SuKienException("Loi: Khong tim thay su kien can xoa!");
            }

            // Khong cho phep xoa su kien dang dien ra
            if (sk.TrangThaiSuKien == TrangThaiSuKien.ONGOING)
            {
                throw new SuKienException("Loi: Su kien dang dien ra, khong the xoa truc tiep!");
            }

            _suKienRepo.Xoa(maSuKien);
            Console.WriteLine("Xoa su kien thanh cong!");
        }

        public SuKien TimById(string maSuKien)
        {
            return _suKienRepo.TimById(maSuKien);
        }

        public List<SuKien> LayTatCa()
        {
            return _suKienRepo.LayTatCa();
        }
    }
}
