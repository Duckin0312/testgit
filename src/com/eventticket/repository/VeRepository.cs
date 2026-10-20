using System;
using System.Collections.Generic;
using System.Linq;
using EventTicket.Model.Entity;
using EventTicket.Model.Enums;

namespace EventTicket.Repository
{
    public class VeRepository : GenericRepository<Ve>
    {
        private readonly List<Ve> danhSachVe = new List<Ve>();

        public VeRepository()
        {
            // Du lieu mau phong phu de test day du cac truong hop
            Ve v1 = new Ve("VE001", "KH001", "SK001", "KV01", LoaiVe.STANDARD,
                TrangThaiVe.SOLD, 250000m, DateTime.Now.AddDays(-2));
            Ve v2 = new Ve("VE002", "KH002", "SK001", "KV02", LoaiVe.VIP,
                TrangThaiVe.SOLD, 750000m, DateTime.Now.AddDays(-1));
            Ve v3 = new Ve("VE003", "KH001", "SK002", "KV04", LoaiVe.STANDARD,
                TrangThaiVe.SOLD, 300000m, DateTime.Now.AddDays(-10));
            Ve v4 = new Ve("VE004", "KH001", "SK003", "KV06", LoaiVe.VIP,
                TrangThaiVe.SOLD, 1000000m, DateTime.Now.AddDays(-20));
            Ve v5 = new Ve("VE005", "KH002", "SK002", "KV04", LoaiVe.STANDARD,
                TrangThaiVe.CANCELLED, 300000m, DateTime.Now.AddDays(-3));
            Ve v6 = new Ve("VE006", "KH001", "SK001", "KV01", LoaiVe.STANDARD,
                TrangThaiVe.RESERVED, 250000m, DateTime.Now.AddHours(-30));

            danhSachVe.Add(v1);
            danhSachVe.Add(v2);
            danhSachVe.Add(v3);
            danhSachVe.Add(v4);
            danhSachVe.Add(v5);
            danhSachVe.Add(v6);
        }

        public override void Them(Ve entity)
        {
            danhSachVe.Add(entity);
            Console.WriteLine("[Repo] Da them ve: " + entity.MaVe);
        }

        public override void Sua(Ve entity)
        {
            for (int i = 0; i < danhSachVe.Count; i++)
            {
                if (string.Equals(danhSachVe[i].MaVe, entity.MaVe, StringComparison.OrdinalIgnoreCase))
                {
                    danhSachVe[i] = entity;
                    Console.WriteLine("[Repo] Da cap nhat ve: " + entity.MaVe);
                    return;
                }
            }
        }

        public override void Xoa(string maVe)
        {
            danhSachVe.RemoveAll(v => string.Equals(v.MaVe, maVe, StringComparison.OrdinalIgnoreCase));
            Console.WriteLine("[Repo] Da xoa ve: " + maVe);
        }

        public override Ve TimById(string maVe)
        {
            if (maVe == null) return null;
            return danhSachVe
                .FirstOrDefault(v => v.MaVe != null && v.MaVe.Equals(maVe, StringComparison.OrdinalIgnoreCase));
        }

        public override List<Ve> LayTatCa()
        {
            return new List<Ve>(danhSachVe);
        }

        public bool KiemTraMaDaTonTai(string maVe)
        {
            if (maVe == null) return false;
            return danhSachVe
                .Any(v => v.MaVe != null && v.MaVe.Equals(maVe, StringComparison.OrdinalIgnoreCase));
        }

        // Loc danh sach ve theo ma su kien
        public List<Ve> TimTheoSuKien(string maSuKien)
        {
            if (maSuKien == null) return new List<Ve>();
            return danhSachVe
                .Where(v => v.MaSuKien != null && v.MaSuKien.Equals(maSuKien, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // Loc danh sach ve theo ma khach hang
        public List<Ve> TimTheoKhachHang(string maKhachHang)
        {
            if (maKhachHang == null) return new List<Ve>();
            return danhSachVe
                .Where(v => v.MaKhachHang != null && v.MaKhachHang.Equals(maKhachHang, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // Quý trong năm (1-4), tương đương IsoFields.QUARTER_OF_YEAR
        private static int Quy(DateTime t)
        {
            return (t.Month - 1) / 3 + 1;
        }

        // =========================================================================
        // CÁC TRUY VẤN NGHIỆP VỤ (LINQ)
        // =========================================================================

        /// <summary>
        /// CÂU 1: Tìm những khách hàng thân thiết đã mua vé tham gia ít nhất 3 sự kiện khác nhau trong năm nay.
        /// </summary>
        public List<string> Cau1_TimKhachHangThanThiet()
        {
            int namHienTai = DateTime.Now.Year;
            return danhSachVe
                .Where(v => v.TrangThaiVe == TrangThaiVe.SOLD)
                .Where(v => v.ThoiGianMua != null && v.ThoiGianMua.Value.Year == namHienTai)
                .GroupBy(v => v.MaKhachHang)
                .Where(g => g.Select(v => v.MaSuKien).Distinct().Count() >= 3)
                .Select(g => g.Key)
                .ToList();
        }

        /// <summary>
        /// CÂU 2: Thống kê tỷ lệ khách hàng đăng ký mới so với khách hàng cũ quay lại mua vé theo từng quý.
        /// </summary>
        public void Cau2_ThongKeTyLeKhachTheoQuy()
        {
            // Lấy thời điểm mua vé đầu tiên của từng khách hàng
            Dictionary<string, DateTime> lanMuaDau = danhSachVe
                .Where(v => v.ThoiGianMua != null)
                .GroupBy(v => v.MaKhachHang)
                .ToDictionary(g => g.Key, g => g.Min(v => v.ThoiGianMua.Value));

            // Vé này có phải của lần mua đầu tiên (cùng quý, cùng năm) của khách không
            bool LaKhachMoi(Ve v)
            {
                DateTime t = v.ThoiGianMua.Value;
                return lanMuaDau.TryGetValue(v.MaKhachHang, out DateTime dau)
                       && Quy(dau) == Quy(t)
                       && dau.Year == t.Year;
            }

            var theoQuy = danhSachVe
                .Where(v => v.ThoiGianMua != null)
                .GroupBy(v => "Q" + Quy(v.ThoiGianMua.Value) + "-" + v.ThoiGianMua.Value.Year);

            foreach (var nhom in theoQuy)
            {
                long moi = nhom.Where(LaKhachMoi).Select(v => v.MaKhachHang).Distinct().Count();
                long cu = nhom.Where(v => !LaKhachMoi(v)).Select(v => v.MaKhachHang).Distinct().Count();
                long tong = moi + cu;
                double tyLeMoi = tong > 0 ? (moi * 100.0 / tong) : 0;
                double tyLeCu = tong > 0 ? (cu * 100.0 / tong) : 0;
                Console.WriteLine($"{nhom.Key}: Khách mới: {moi} ({tyLeMoi:F1}%) | Khách cũ: {cu} ({tyLeCu:F1}%)");
            }
        }

        /// <summary>
        /// CÂU 3: Lấy danh sách các vé có trạng thái "Đã hủy" và tính tổng số tiền hệ thống cần hoàn lại.
        /// </summary>
        public Dictionary<string, object> Cau3_LayVeDaHuyVaTongHoanTien()
        {
            List<Ve> veHuy = danhSachVe
                .Where(v => v.TrangThaiVe == TrangThaiVe.CANCELLED)
                .ToList();

            decimal tongTien = veHuy
                .Where(v => v.GiaVe != null)
                .Sum(v => v.GiaVe.Value);

            var res = new Dictionary<string, object>();
            res["DanhSachVeHuy"] = veHuy;
            res["TongTienHoan"] = tongTien;
            return res;
        }

        /// <summary>
        /// CÂU 4: Tìm khách hàng đã mua số lượng vé nhiều nhất cho một sự kiện cụ thể.
        /// Trả về null nếu không có.
        /// </summary>
        public KeyValuePair<string, long>? Cau4_TimKhachMuaNhieuVeNhatTheoSuKien(string maSuKien)
        {
            if (maSuKien == null) return null;
            List<KeyValuePair<string, long>> ds = danhSachVe
                .Where(v => maSuKien.Equals(v.MaSuKien, StringComparison.OrdinalIgnoreCase) && v.TrangThaiVe == TrangThaiVe.SOLD)
                .GroupBy(v => v.MaKhachHang)
                .Select(g => new KeyValuePair<string, long>(g.Key, g.Count()))
                .ToList();
            if (ds.Count == 0) return null;
            return ds.OrderByDescending(kv => kv.Value).First();
        }

        /// <summary>
        /// CÂU 5: Phân tích loại khách hàng (LoaiKhachHang) nào có xu hướng mua vé sát ngày diễn ra sự kiện nhất.
        /// </summary>
        public void Cau5_PhanTichLoaiKhachMuaSatNgay(KhachHangRepository khRepo, SuKienRepository skRepo)
        {
            Dictionary<string, LoaiKhachHang> mapLoaiKH = khRepo.LayTatCa()
                .Where(kh => kh.LoaiKhachHang != null)
                .ToDictionaryKeepFirst(kh => kh.MaKhachHang, kh => kh.LoaiKhachHang.Value);

            Dictionary<string, DateTime> mapTGSK = skRepo.LayTatCa()
                .Where(sk => sk.ThoiGianDienRa != null)
                .ToDictionaryKeepFirst(sk => sk.MaSuKien, sk => sk.ThoiGianDienRa.Value);

            var ketQua = danhSachVe
                .Where(v => v.TrangThaiVe == TrangThaiVe.SOLD && v.ThoiGianMua != null)
                .Where(v => mapLoaiKH.ContainsKey(v.MaKhachHang) && mapTGSK.ContainsKey(v.MaSuKien))
                .GroupBy(v => mapLoaiKH[v.MaKhachHang])
                .Select(g => new
                {
                    Loai = g.Key,
                    TrungBinh = g.Average(v => (double)(long)(mapTGSK[v.MaSuKien] - v.ThoiGianMua.Value).TotalHours)
                })
                .OrderBy(e => e.TrungBinh);

            foreach (var e in ketQua)
            {
                Console.WriteLine($"Loại KH: {e.Loai.MoTa(),-15} -> Trung bình mua trước: {e.TrungBinh:F1} giờ");
            }
        }

        /// <summary>
        /// CÂU 6: Hiển thị danh sách các vé chưa thanh toán quá 24 giờ kể từ lúc đặt để hệ thống tự động hủy.
        /// </summary>
        public List<Ve> Cau6_HuyVeChuaThanhToanQua24h()
        {
            DateTime now = DateTime.Now;
            List<Ve> quaHan = danhSachVe
                .Where(v => v.TrangThaiVe == TrangThaiVe.RESERVED)
                .Where(v => v.ThoiGianMua != null && (long)(now - v.ThoiGianMua.Value).TotalHours > 24)
                .ToList();

            foreach (Ve v in quaHan)
            {
                v.TrangThaiVe = TrangThaiVe.CANCELLED;
                Console.WriteLine("[Repo-AutoCancel] Đã hủy vé quá hạn: " + v.MaVe);
            }
            return quaHan;
        }

        /// <summary>
        /// CÂU 14: Tìm các khu vực có giá vé cao hơn giá vé trung bình của tất cả các khu vực.
        /// </summary>
        public List<string> Cau14_TimKhuVucGiaVeCaoHonTrungBinh()
        {
            List<double> dsGia = danhSachVe
                .Where(v => v.GiaVe != null)
                .Select(v => (double)v.GiaVe.Value)
                .ToList();
            double giaTBChung = dsGia.Count > 0 ? dsGia.Average() : 0.0;

            return danhSachVe
                .Where(v => v.GiaVe != null && v.MaKhuVuc != null)
                .GroupBy(v => v.MaKhuVuc)
                .Select(g => new { MaKhuVuc = g.Key, GiaTB = g.Average(v => (double)v.GiaVe.Value) })
                .Where(e => e.GiaTB > giaTBChung)
                .Select(e => $"Mã khu vực: {e.MaKhuVuc} | Giá TB: {e.GiaTB:N0} VND > TB chung: {giaTBChung:N0} VND")
                .ToList();
        }

        /// <summary>
        /// CÂU 15: Liệt kê tên khách hàng, sự kiện, khu vực và trạng thái của vé mà khách hàng đang sở hữu.
        /// </summary>
        public void Cau15_InChiTietSoHuuVe(KhachHangRepository khRepo, SuKienRepository skRepo)
        {
            Dictionary<string, string> mapKH = khRepo.LayTatCa()
                .ToDictionaryKeepFirst(kh => kh.MaKhachHang, kh => kh.HoTen);
            Dictionary<string, string> mapSK = skRepo.LayTatCa()
                .ToDictionaryKeepFirst(sk => sk.MaSuKien, sk => sk.TenSuKien);

            foreach (Ve v in danhSachVe)
            {
                Console.WriteLine(
                    $"Mã vé: {v.MaVe,-7} | Khách: {mapKH.GetValueOrDefault(v.MaKhachHang, v.MaKhachHang),-18} | " +
                    $"Sự kiện: {mapSK.GetValueOrDefault(v.MaSuKien, v.MaSuKien),-25} | " +
                    $"Khu vực: {v.MaKhuVuc,-8} | Trạng thái: {v.TrangThaiVe?.MoTa()}");
            }
        }
    }
}
