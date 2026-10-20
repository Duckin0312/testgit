using System;
using System.Collections.Generic;
using System.Linq;
using EventTicket.Interfaces;
using EventTicket.Model.Entity;
using EventTicket.Repository;

namespace EventTicket.Service
{
    public class DiaDiemService : IQuanLy<DiaDiem>
    {
        private readonly DiaDiemRepository _diaDiemRepo;

        public DiaDiemService()
        {
            _diaDiemRepo = new DiaDiemRepository();
        }

        // Tim kiem dia diem theo ten
        public List<DiaDiem> TimKiemTheoTen(string tenCanTim)
        {
            if (string.IsNullOrWhiteSpace(tenCanTim))
            {
                return new List<DiaDiem>();
            }
            string tenLower = tenCanTim.Trim().ToLower();
            return _diaDiemRepo.LayTatCa()
                .Where(dd => dd.TenDiaDiem != null && dd.TenDiaDiem.ToLower().Contains(tenLower))
                .ToList();
        }

        // Loc dia diem theo thanh pho hoac khu vuc dia ly trong dia chi
        public List<DiaDiem> LocTheoThanhPho(string tenThanhPho)
        {
            if (string.IsNullOrWhiteSpace(tenThanhPho))
            {
                return new List<DiaDiem>();
            }
            string tpLower = tenThanhPho.Trim().ToLower();
            return _diaDiemRepo.LayTatCa()
                .Where(dd => dd.DiaChi != null && dd.DiaChi.ToLower().Contains(tpLower))
                .ToList();
        }

        // Sap xep danh sach dia diem theo tong suc chua giam dan (LINQ)
        public List<DiaDiem> SapXepTheoSucChuaGiamDan()
        {
            return _diaDiemRepo.LayTatCa()
                .OrderByDescending(dd => dd.SucChuaTong)
                .ToList();
        }

        // Them khu vuc moi vao dia diem
        public void ThemKhuVucChoDiaDiem(string maDiaDiem, KhuVuc khuVucMoi)
        {
            DiaDiem dd = _diaDiemRepo.TimById(maDiaDiem);
            if (dd == null)
            {
                throw new ArgumentException("Loi: Khong tim thay dia diem co ma: " + maDiaDiem);
            }
            if (khuVucMoi == null)
            {
                throw new ArgumentException("Loi: Thong tin khu vuc khong duoc de trong!");
            }

            dd.ThemKhuVuc(khuVucMoi);
            _diaDiemRepo.Sua(dd);
            Console.WriteLine("Them khu vuc thanh cong vao dia diem: " + dd.TenDiaDiem);
        }

        public void Them(DiaDiem ddMoi)
        {
            if (ddMoi == null)
            {
                throw new ArgumentException("Loi: Du lieu dia diem khong duoc de trong!");
            }

            if (!ddMoi.Validate())
            {
                throw new ArgumentException("Loi: Thong tin dia diem khong hop le!");
            }

            if (_diaDiemRepo.KiemTraMaDaTonTai(ddMoi.MaDiaDiem))
            {
                throw new ArgumentException("Loi: Ma dia diem '" + ddMoi.MaDiaDiem + "' da ton tai!");
            }

            _diaDiemRepo.Them(ddMoi);
            Console.WriteLine("Them dia diem moi thanh cong: " + ddMoi.TenDiaDiem);
        }

        public void Sua(DiaDiem ddCanSua)
        {
            if (ddCanSua == null || !ddCanSua.Validate())
            {
                throw new ArgumentException("Loi: Du lieu cap nhat khong hop le!");
            }

            if (!_diaDiemRepo.KiemTraMaDaTonTai(ddCanSua.MaDiaDiem))
            {
                throw new ArgumentException("Loi: Khong tim thay dia diem can cap nhat!");
            }

            _diaDiemRepo.Sua(ddCanSua);
            Console.WriteLine("Cap nhat dia diem thanh cong!");
        }

        public void Xoa(string maDiaDiem)
        {
            DiaDiem dd = _diaDiemRepo.TimById(maDiaDiem);
            if (dd == null)
            {
                throw new ArgumentException("Loi: Khong tim thay dia diem can xoa!");
            }

            _diaDiemRepo.Xoa(maDiaDiem);
            Console.WriteLine("Xoa dia diem thanh cong!");
        }

        public DiaDiem TimById(string maDiaDiem)
        {
            return _diaDiemRepo.TimById(maDiaDiem);
        }

        public List<DiaDiem> LayTatCa()
        {
            return _diaDiemRepo.LayTatCa();
        }
    }
}
