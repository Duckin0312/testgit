using System;
using System.Collections.Generic;
using System.Linq;
using EventTicket.Model.Entity;
using EventTicket.Model.Enums;

namespace EventTicket.Repository
{
    public class SuKienRepository : GenericRepository<SuKien>
    {
        private readonly List<SuKien> danhSachSuKien = new List<SuKien>();

        public SuKienRepository()
        {
            DiaDiem diaDiemMau = new DiaDiem("DD001", "San van dong My Dinh", "Le Duc Tho, Ha Noi");
            diaDiemMau.ThemKhuVuc(new KhuVuc("KV01", "Khu Vuc VIP A", LoaiKhuVuc.KHAN_GIA_VIP, 500));
            diaDiemMau.ThemKhuVuc(new KhuVuc("KV02", "Khu Vuc Thuong B", LoaiKhuVuc.KHAN_GIA_THUONG, 3000));

            SuKien sk1 = new SuKien("SK001", "Live Concert mua thu", TrangThaiSuKien.ONGOING,
                DateTime.Now.AddDays(30), diaDiemMau);
            SuKien sk2 = new SuKien("SK002", "Dem nhac Rock Ha Noi", TrangThaiSuKien.UPCOMING,
                DateTime.Now.AddDays(4), diaDiemMau);
            SuKien sk3 = new SuKien("SK003", "Acoustic Night Ha Noi", TrangThaiSuKien.CANCELLED,
                DateTime.Now.AddDays(10), diaDiemMau);

            danhSachSuKien.Add(sk1);
            danhSachSuKien.Add(sk2);
            danhSachSuKien.Add(sk3);
        }

        public override void Them(SuKien entity)
        {
            danhSachSuKien.Add(entity);
            Console.WriteLine("[Repo] Da them su kien: " + entity.MaSuKien);
        }

        public override void Sua(SuKien entity)
        {
            for (int i = 0; i < danhSachSuKien.Count; i++)
            {
                if (string.Equals(danhSachSuKien[i].MaSuKien, entity.MaSuKien, StringComparison.OrdinalIgnoreCase))
                {
                    danhSachSuKien[i] = entity;
                    Console.WriteLine("[Repo] Da cap nhat su kien: " + entity.MaSuKien);
                    return;
                }
            }
        }

        public override void Xoa(string maSuKien)
        {
            danhSachSuKien.RemoveAll(sk => string.Equals(sk.MaSuKien, maSuKien, StringComparison.OrdinalIgnoreCase));
            Console.WriteLine("[Repo] Da xoa su kien: " + maSuKien);
        }

        public override SuKien TimById(string maSuKien)
        {
            if (maSuKien == null) return null;
            return danhSachSuKien
                .FirstOrDefault(sk => sk.MaSuKien != null && sk.MaSuKien.Equals(maSuKien, StringComparison.OrdinalIgnoreCase));
        }

        public override List<SuKien> LayTatCa()
        {
            return new List<SuKien>(danhSachSuKien);
        }

        // Kiem tra ma su kien da ton tai chua
        public bool KiemTraMaDaTonTai(string maSuKien)
        {
            if (maSuKien == null) return false;
            return danhSachSuKien
                .Any(sk => sk.MaSuKien != null && sk.MaSuKien.Equals(maSuKien, StringComparison.OrdinalIgnoreCase));
        }

        // Loc danh sach theo trang thai su kien
        public List<SuKien> LocTheoTrangThai(TrangThaiSuKien? trangThai)
        {
            if (trangThai == null) return new List<SuKien>();
            return danhSachSuKien
                .Where(sk => sk.TrangThaiSuKien == trangThai)
                .ToList();
        }

        // =========================================================================
        // CÁC TRUY VẤN NGHIỆP VỤ (LINQ)
        // =========================================================================

        /// <summary>
        /// CÂU 7: Đếm số lượng vé còn trống (chưa bán được) của các sự kiện sẽ diễn ra trong tuần tới để đẩy mạnh marketing.
        /// </summary>
        public void Cau7_DemVeConTrongSuKienTuanToi(VeRepository veRepo)
        {
            DateTime now = DateTime.Now;
            DateTime nextWeek = now.AddDays(7);

            Dictionary<string, long> veDaBan = veRepo.LayTatCa()
                .Where(v => v.TrangThaiVe == TrangThaiVe.SOLD || v.TrangThaiVe == TrangThaiVe.RESERVED)
                .GroupBy(v => v.MaSuKien)
                .ToDictionary(g => g.Key, g => (long)g.Count());

            foreach (SuKien sk in danhSachSuKien.Where(sk => sk.ThoiGianDienRa != null
                                                            && sk.ThoiGianDienRa > now
                                                            && sk.ThoiGianDienRa < nextWeek))
            {
                int sucChua = sk.DiaDiem != null ? sk.DiaDiem.SucChuaTong : 0;
                long daBan = veDaBan.GetValueOrDefault(sk.MaSuKien, 0L);
                long conTrong = Math.Max(0, sucChua - daBan);
                Console.WriteLine($"[{sk.MaSuKien}] {sk.TenSuKien,-25} | Sức chứa: {sucChua} | Đã bán: {daBan} | Còn trống: {conTrong} vé");
            }
        }

        /// <summary>
        /// CÂU 9: Tìm các sự kiện có tỷ lệ lấp đầy (số vé bán ra / tổng sức chứa) đạt trên 95%.
        /// </summary>
        public List<string> Cau9_TimSuKienLapDayTren95(VeRepository veRepo)
        {
            Dictionary<string, long> veBanMap = veRepo.LayTatCa()
                .Where(v => v.TrangThaiVe == TrangThaiVe.SOLD)
                .GroupBy(v => v.MaSuKien)
                .ToDictionary(g => g.Key, g => (long)g.Count());

            return danhSachSuKien
                .Where(sk => sk.DiaDiem != null && sk.DiaDiem.SucChuaTong > 0)
                .Where(sk =>
                {
                    long ban = veBanMap.GetValueOrDefault(sk.MaSuKien, 0L);
                    return ((double)ban / sk.DiaDiem.SucChuaTong) >= 0.95;
                })
                .Select(sk =>
                {
                    long ban = veBanMap.GetValueOrDefault(sk.MaSuKien, 0L);
                    double tyLe = (double)ban / sk.DiaDiem.SucChuaTong * 100;
                    return $"[{sk.MaSuKien}] {sk.TenSuKien} -> {tyLe:F1}% lấp đầy ({ban}/{sk.DiaDiem.SucChuaTong} vé)";
                })
                .ToList();
        }

        /// <summary>
        /// CÂU 10: Hiển thị các sự kiện bị hủy và đếm tổng số lượng vé đã phát hành cần phải thu hồi cho từng sự kiện.
        /// </summary>
        public void Cau10_ThongKeSuKienHuyVaVeThuHoi(VeRepository veRepo)
        {
            Dictionary<string, long> veThuHoiMap = veRepo.LayTatCa()
                .Where(v => v.TrangThaiVe == TrangThaiVe.SOLD || v.TrangThaiVe == TrangThaiVe.RESERVED)
                .GroupBy(v => v.MaSuKien)
                .ToDictionary(g => g.Key, g => (long)g.Count());

            foreach (SuKien sk in danhSachSuKien.Where(sk => sk.TrangThaiSuKien == TrangThaiSuKien.CANCELLED))
            {
                long soVe = veThuHoiMap.GetValueOrDefault(sk.MaSuKien, 0L);
                Console.WriteLine($"Sự kiện hủy: [{sk.MaSuKien}] {sk.TenSuKien,-25} -> Số vé cần thu hồi: {soVe} vé");
            }
        }

        /// <summary>
        /// CÂU 11: Liệt kê tên sự kiện, địa điểm tổ chức và sức chứa của địa điểm đó.
        /// </summary>
        public void Cau11_LietKeSuKienVaDiaDiem()
        {
            foreach (SuKien sk in danhSachSuKien.Where(sk => sk.DiaDiem != null))
            {
                Console.WriteLine($"Sự kiện: {sk.TenSuKien,-30} | Địa điểm: {sk.DiaDiem.TenDiaDiem,-25} | Sức chứa: {sk.DiaDiem.SucChuaTong:N0} chỗ");
            }
        }

        /// <summary>
        /// CÂU 12: Tìm các sự kiện được tổ chức tại địa điểm có sức chứa trên 1.000 người.
        /// </summary>
        public List<SuKien> Cau12_TimSuKienSucChuaTren1000()
        {
            return danhSachSuKien
                .Where(sk => sk.DiaDiem != null && sk.DiaDiem.SucChuaTong > 1000)
                .ToList();
        }

        /// <summary>
        /// CÂU 18: Liệt kê các sự kiện chưa có nghệ sĩ nào được xếp lịch biểu diễn, kèm thông tin.
        /// </summary>
        public List<SuKien> Cau18_TimSuKienChuaCoNgheSi(List<LichBieuDien> dsLich)
        {
            if (dsLich == null) return LayTatCa();
            HashSet<string> skCoLich = dsLich
                .Select(l => l.MaSuKien)
                .ToHashSet();

            return danhSachSuKien
                .Where(sk => !skCoLich.Contains(sk.MaSuKien))
                .ToList();
        }

        /// <summary>
        /// CÂU 20: Tìm các sự kiện có thời gian biểu diễn kéo dài trên 2 giờ đồng hồ (> 120 phút).
        /// </summary>
        public List<string> Cau20_TimSuKienBieuDienTren2Gio(List<LichBieuDien> dsLich)
        {
            if (dsLich == null) return new List<string>();

            Dictionary<string, long> thoiLuongSK = dsLich
                .Where(l => l.ThoiGianBatDau != null && l.ThoiGianKetThuc != null)
                .GroupBy(l => l.MaSuKien)
                .ToDictionary(g => g.Key, g => g.Sum(l => (long)(l.ThoiGianKetThuc.Value - l.ThoiGianBatDau.Value).TotalMinutes));

            return thoiLuongSK
                .Where(e => e.Value > 120)
                .Select(e =>
                {
                    SuKien sk = TimById(e.Key);
                    string ten = sk != null ? sk.TenSuKien : e.Key;
                    return $"[{e.Key}] {ten} -> Thời lượng: {e.Value} phút ({e.Value / 60.0:F1} giờ)";
                })
                .ToList();
        }
    }
}
