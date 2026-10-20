using System;
using System.Collections.Generic;
using System.Linq;
using EventTicket.Interfaces;
using EventTicket.Model.Entity;

namespace EventTicket.Service
{
    public class TaiTroService : IQuanLy<TaiTro>
    {
        // Danh sach luu tru cac hop dong tai tro
        private readonly List<TaiTro> _danhSachTaiTro = new List<TaiTro>();

        public TaiTroService()
        {
            // Du lieu mau ban dau
            var ntt1 = new NhaTaiTro("Cong ty Nuoc Giai Khat RedBull", "0909123456", "contact@redbull.vn",
                    "Nuoc uong tang luc", "Nguyen Van Dai Dien");
            ntt1.Id = "NTT001";

            var sk1 = new SuKien();
            sk1.Id = "SK001";
            sk1.TenSuKien = "Live Concert mua thu";

            var tt1 = new TaiTro(ntt1, sk1, 100000000m, "Kim Cuong");
            tt1.Id = "TT001";

            _danhSachTaiTro.Add(tt1);
        }

        // Nghiep vu: Tao hop dong tai tro moi
        public void KyKetTaiTro(TaiTro taiTroMoi)
        {
            if (taiTroMoi == null)
            {
                throw new ArgumentException("Loi: Thong tin tai tro khong duoc de trong!");
            }

            if (!taiTroMoi.Validate())
            {
                throw new ArgumentException("Loi: Du lieu tai tro khong hop le!");
            }

            if (string.IsNullOrWhiteSpace(taiTroMoi.Id))
            {
                taiTroMoi.Id = "TT" + (_danhSachTaiTro.Count + 1).ToString("D3");
            }

            _danhSachTaiTro.Add(taiTroMoi);
            Console.WriteLine("Ky ket tai tro thanh cong: " + taiTroMoi);
        }

        // Lay danh sach tai tro theo ma su kien
        public List<TaiTro> LayTaiTroTheoSuKien(string maSuKien)
        {
            if (maSuKien == null) return new List<TaiTro>();
            return _danhSachTaiTro
                .Where(tt => tt.SuKien != null &&
                             string.Equals(maSuKien, tt.SuKien.Id, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // Lay danh sach tai tro theo nha tai tro
        public List<TaiTro> LayTaiTroTheoNhaTaiTro(string maNhaTaiTro)
        {
            if (maNhaTaiTro == null) return new List<TaiTro>();
            return _danhSachTaiTro
                .Where(tt => tt.NhaTaiTro != null &&
                             string.Equals(maNhaTaiTro, tt.NhaTaiTro.Id, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // Tinh tong so tien tai tro cua mot su kien
        public decimal TinhTongTaiTroSuKien(string maSuKien)
        {
            return LayTaiTroTheoSuKien(maSuKien)
                .Where(tt => tt.SoTienTaiTro != null)
                .Sum(tt => tt.SoTienTaiTro.Value);
        }

        // Tinh tong tat ca tien tai tro trong he thong
        public decimal TinhTongTatCaTaiTro()
        {
            return _danhSachTaiTro
                .Where(tt => tt.SoTienTaiTro != null)
                .Sum(tt => tt.SoTienTaiTro.Value);
        }

        public void Them(TaiTro entity)
        {
            KyKetTaiTro(entity);
        }

        public void Sua(TaiTro entity)
        {
            if (entity == null || !entity.Validate())
            {
                throw new ArgumentException("Loi: Du lieu tai tro khong hop le!");
            }

            for (int i = 0; i < _danhSachTaiTro.Count; i++)
            {
                if (_danhSachTaiTro[i].Id != null &&
                    string.Equals(_danhSachTaiTro[i].Id, entity.Id, StringComparison.OrdinalIgnoreCase))
                {
                    _danhSachTaiTro[i] = entity;
                    Console.WriteLine("Cap nhat hop dong tai tro thanh cong: " + entity.Id);
                    return;
                }
            }
            throw new ArgumentException("Loi: Khong tim thay hop dong tai tro can cap nhat!");
        }

        public void Xoa(string id)
        {
            if (id == null) return;
            int removed = _danhSachTaiTro.RemoveAll(tt =>
                tt.Id != null && string.Equals(tt.Id, id, StringComparison.OrdinalIgnoreCase));
            if (removed > 0)
            {
                Console.WriteLine("Xoa hop dong tai tro thanh cong: " + id);
            }
            else
            {
                throw new ArgumentException("Loi: Khong tim thay hop dong tai tro co ma: " + id);
            }
        }

        public TaiTro TimById(string id)
        {
            if (id == null) return null;
            return _danhSachTaiTro.FirstOrDefault(tt =>
                tt.Id != null && string.Equals(tt.Id, id, StringComparison.OrdinalIgnoreCase));
        }

        public List<TaiTro> LayTatCa()
        {
            return new List<TaiTro>(_danhSachTaiTro);
        }

        // =========================================================================
        // CÁC TRUY VẤN NGHIỆP VỤ (LINQ)
        // =========================================================================

        /// <summary>CÂU 23: Lọc các khoản tài trợ đã được giải ngân trong tháng 10/2026.</summary>
        public List<TaiTro> Cau23_LocTaiTroThang10Nam2026()
        {
            return _danhSachTaiTro
                .Where(tt => tt.NgayKyKet != null
                             && tt.NgayKyKet.Month == 10
                             && tt.NgayKyKet.Year == 2026)
                .ToList();
        }

        /// <summary>CÂU 26: Tính tổng giá trị tài trợ (bằng tiền mặt) của từng nhà tài trợ.</summary>
        public Dictionary<string, decimal> Cau26_TinhTongTaiTroTheoTungNha()
        {
            return _danhSachTaiTro
                .Where(tt => tt.NhaTaiTro != null && tt.SoTienTaiTro != null)
                .GroupBy(tt => tt.NhaTaiTro.HoTen)
                .ToDictionary(g => g.Key, g => g.Sum(tt => tt.SoTienTaiTro.Value));
        }

        /// <summary>CÂU 27: Tìm Top 3 nhà tài trợ đóng góp số tiền lớn nhất cho hệ thống.</summary>
        public List<KeyValuePair<string, decimal>> Cau27_TimTop3NhaTaiTro()
        {
            return Cau26_TinhTongTaiTroTheoTungNha()
                .OrderByDescending(kv => kv.Value)
                .Take(3)
                .ToList();
        }
    }
}
