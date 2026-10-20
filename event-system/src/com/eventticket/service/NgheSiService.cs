using System;
using System.Collections.Generic;
using System.Linq;
using EventTicket.Interfaces;
using EventTicket.Model.Entity;
using EventTicket.Repository;

namespace EventTicket.Service
{
    public class NgheSiService : IQuanLy<NgheSi>
    {
        private readonly NgheSiRepository _ngheSiRepo;

        public NgheSiService()
        {
            _ngheSiRepo = new NgheSiRepository();
        }

        // Tim kiem nghe si theo ten
        public List<NgheSi> TimKiemTheoTen(string tenCanTim)
        {
            if (string.IsNullOrWhiteSpace(tenCanTim))
            {
                return new List<NgheSi>();
            }
            string tenLower = tenCanTim.Trim().ToLower();
            return _ngheSiRepo.LayTatCa()
                .Where(ns => ns.TenNgheSi != null && ns.TenNgheSi.ToLower().Contains(tenLower))
                .ToList();
        }

        // Loc nghe si theo the loai
        public List<NgheSi> LocTheoTheLoai(string theLoai)
        {
            return _ngheSiRepo.TimTheoTheLoai(theLoai);
        }

        // Them lich bieu dien cho nghe si
        public void ThemLichBieuDien(string maNgheSi, LichBieuDien lich)
        {
            NgheSi ns = _ngheSiRepo.TimById(maNgheSi);
            if (ns == null)
            {
                throw new ArgumentException("Loi: Khong tim thay nghe si co ma: " + maNgheSi);
            }
            if (lich == null)
            {
                throw new ArgumentException("Loi: Thong tin lich bieu dien khong duoc de trong!");
            }

            ns.ThemLich(lich);
            _ngheSiRepo.Sua(ns);
            Console.WriteLine("Them lich bieu dien thanh cong cho nghe si: " + ns.TenNgheSi);
        }

        // Lay danh sach lich bieu dien sap toi cua nghe si
        public List<LichBieuDien> LayLichBieuDienSapToi(string maNgheSi)
        {
            NgheSi ns = _ngheSiRepo.TimById(maNgheSi);
            if (ns == null)
            {
                Console.WriteLine("Loi: Khong tim thay nghe si co ma: " + maNgheSi);
                return new List<LichBieuDien>();
            }
            return ns.GetSuKienSapToi();
        }

        public void Them(NgheSi nsMoi)
        {
            if (nsMoi == null)
            {
                throw new ArgumentException("Loi: Du lieu nghe si khong duoc de trong!");
            }

            if (string.IsNullOrWhiteSpace(nsMoi.TenNgheSi))
            {
                throw new ArgumentException("Loi: Ten nghe si khong duoc de trong!");
            }

            if (_ngheSiRepo.KiemTraMaDaTonTai(nsMoi.MaNgheSi))
            {
                throw new ArgumentException("Loi: Ma nghe si '" + nsMoi.MaNgheSi + "' da ton tai!");
            }

            _ngheSiRepo.Them(nsMoi);
            Console.WriteLine("Them nghe si moi thanh cong: " + nsMoi.TenNgheSi);
        }

        public void Sua(NgheSi nsCanSua)
        {
            if (nsCanSua == null)
            {
                throw new ArgumentException("Loi: Du lieu cap nhat khong hop le!");
            }

            if (!_ngheSiRepo.KiemTraMaDaTonTai(nsCanSua.MaNgheSi))
            {
                throw new ArgumentException("Loi: Khong tim thay nghe si can cap nhat!");
            }

            _ngheSiRepo.Sua(nsCanSua);
            Console.WriteLine("Cap nhat thong tin nghe si thanh cong!");
        }

        public void Xoa(string maNgheSi)
        {
            NgheSi ns = _ngheSiRepo.TimById(maNgheSi);
            if (ns == null)
            {
                throw new ArgumentException("Loi: Khong tim thay nghe si can xoa!");
            }

            _ngheSiRepo.Xoa(maNgheSi);
            Console.WriteLine("Xoa nghe si thanh cong!");
        }

        public NgheSi TimById(string maNgheSi)
        {
            return _ngheSiRepo.TimById(maNgheSi);
        }

        public List<NgheSi> LayTatCa()
        {
            return _ngheSiRepo.LayTatCa();
        }
    }
}
