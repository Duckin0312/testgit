using System;
using System.Collections.Generic;
using System.Linq;
using EventTicket.Model.Entity;
using EventTicket.Model.Enums;

namespace EventTicket.Repository
{
    public class NhanVienRepository : GenericRepository<NhanVien>
    {
        // Danh sách lưu trữ tạm thời trên bộ nhớ (RAM)
        private List<NhanVien> danhSachNhanVien = new List<NhanVien>();

        public NhanVienRepository()
        {
            // Tài khoản Admin mặc định để test đăng nhập
            NhanVien admin = new NhanVien();
            admin.Id = "NV000";
            admin.MaNguoiDung = "admin";
            admin.MatKhau = "admin123456";
            admin.HoTen = "Giám Đốc Quản Lý";
            admin.Sdt = "0901234567";
            admin.Email = "admin@eventticket.com";
            admin.Quyen = QuyenHeThong.ADMIN;
            NhanVien nv1 = new NhanVien("NV001", "123456", "Tran Van Nam", "0911223344", "nam.tran@eventticket.com", "Dieu Phoi San Khau");
            NhanVien nv2 = new NhanVien("NV002", "123456", "Le Thi Mai", "0922334455", "mai.le@eventticket.com", "Soat Ve");
            NhanVien nv3 = new NhanVien("NV003", "123456", "Pham Minh Tuan", "0933445566", "tuan.pham@eventticket.com", "An Ninh");
            NhanVien nv4 = new NhanVien("NV004", "123456", "Hoang Thi Cuc", "0944556677", "cuc.hoang@eventticket.com", "Soat Ve");

            danhSachNhanVien.Add(admin);
            danhSachNhanVien.Add(nv1);
            danhSachNhanVien.Add(nv2);
            danhSachNhanVien.Add(nv3);
            danhSachNhanVien.Add(nv4);
        }

        public override void Them(NhanVien entity)
        {
            danhSachNhanVien.Add(entity);
            Console.WriteLine("[Repo] Da luu nhan vien: " + entity.MaNguoiDung + " vao kho.");
        }

        public override void Sua(NhanVien entity)
        {
            for (int i = 0; i < danhSachNhanVien.Count; i++)
            {
                if (string.Equals(danhSachNhanVien[i].MaNguoiDung, entity.MaNguoiDung, StringComparison.OrdinalIgnoreCase))
                {
                    danhSachNhanVien[i] = entity;
                    Console.WriteLine("[Repo] Da cap nhat nhan vien: " + entity.MaNguoiDung);
                    return;
                }
            }
        }

        public override void Xoa(string maNhanVien)
        {
            danhSachNhanVien.RemoveAll(nv =>
                string.Equals(nv.MaNguoiDung, maNhanVien, StringComparison.OrdinalIgnoreCase)
                || (nv.Id != null && nv.Id.Equals(maNhanVien, StringComparison.OrdinalIgnoreCase)));
            Console.WriteLine("[Repo] Da xoa nhan vien: " + maNhanVien);
        }

        public override NhanVien TimById(string maNhanVien)
        {
            if (maNhanVien == null) return null;
            return danhSachNhanVien
                .FirstOrDefault(nv =>
                    (nv.MaNguoiDung != null && nv.MaNguoiDung.Equals(maNhanVien, StringComparison.OrdinalIgnoreCase))
                    || (nv.Id != null && nv.Id.Equals(maNhanVien, StringComparison.OrdinalIgnoreCase)));
        }

        public override List<NhanVien> LayTatCa()
        {
            return new List<NhanVien>(danhSachNhanVien);
        }

        // Kiểm tra trùng lặp mã nhân viên
        public bool KiemTraMaDaTonTai(string maNguoiDung)
        {
            if (maNguoiDung == null) return false;
            return danhSachNhanVien
                .Any(nv => nv.MaNguoiDung != null && nv.MaNguoiDung.Equals(maNguoiDung, StringComparison.OrdinalIgnoreCase));
        }

        // =========================================================================
        // CÁC TRUY VẤN NGHIỆP VỤ (LINQ)
        // =========================================================================

        /// <summary>
        /// CÂU 24: Lấy danh sách các chức vụ nhân viên không trùng lặp (Distinct).
        /// </summary>
        public List<string> Cau24_LayDanhSachChucVuDistinct()
        {
            return danhSachNhanVien
                .Select(nv => nv.ChucVu)
                .Where(cv => cv != null)
                .Select(cv => cv.Trim())
                .Where(cv => cv.Length > 0)
                .Distinct()
                .OrderBy(cv => cv, StringComparer.Ordinal)
                .ToList();
        }

        /// <summary>
        /// CÂU 25: Kết nối bảng Phân công, Nhân viên và Sự kiện để in chi tiết lịch trực.
        /// </summary>
        public void Cau25_InChiTietLichTruc(List<PhanCong> dsPhanCong, SuKienRepository skRepo)
        {
            if (dsPhanCong == null) return;
            Dictionary<string, NhanVien> mapNV = danhSachNhanVien
                .ToDictionaryKeepFirst(nv => nv.MaNguoiDung, nv => nv);
            Dictionary<string, SuKien> mapSK = skRepo.LayTatCa()
                .ToDictionaryKeepFirst(sk => sk.MaSuKien, sk => sk);

            foreach (PhanCong pc in dsPhanCong)
            {
                NhanVien nv = mapNV.GetValueOrDefault(pc.MaNV);
                SuKien sk = mapSK.GetValueOrDefault(pc.MaSuKien);
                string tenNV = nv != null ? nv.HoTen : pc.MaNV;
                string chucVu = nv != null ? nv.ChucVu : "N/A";
                string tenSK = sk != null ? sk.TenSuKien : pc.MaSuKien;
                string thoiGian = (sk != null && sk.ThoiGianDienRa != null) ? sk.ThoiGianDienRa.Value.ToString() : "Chưa rõ";
                Console.WriteLine($"NV: {tenNV,-20} ({chucVu,-18}) | Sự kiện: {tenSK,-22} | Thời gian: {thoiGian,-18} | Nhiệm vụ: {pc.NhiemVu}");
            }
        }

        /// <summary>
        /// CÂU 28: Thống kê tổng số nhiệm vụ của từng nhân viên (gồm cả người chưa có việc = 0).
        /// </summary>
        public Dictionary<string, long> Cau28_ThongKeSoNhiemVuNhanVien(List<PhanCong> dsPhanCong)
        {
            Dictionary<string, long> countMap = (dsPhanCong ?? new List<PhanCong>())
                .Where(pc => pc.MaNV != null)
                .GroupBy(pc => pc.MaNV)
                .ToDictionary(g => g.Key, g => (long)g.Count());

            // Giữ thứ tự nhân viên như danh sách gốc (tương đương LinkedHashMap)
            return danhSachNhanVien
                .ToDictionaryKeepFirst(nv => nv.HoTen, nv => countMap.GetValueOrDefault(nv.MaNguoiDung, 0L));
        }

        /// <summary>
        /// CÂU 29: Tìm các nhân viên (đã đăng ký) nhưng chưa từng được phân công nhiệm vụ nào.
        /// </summary>
        public List<NhanVien> Cau29_TimNhanVienChuaPhanCong(List<PhanCong> dsPhanCong)
        {
            HashSet<string> maDaPhanCong = (dsPhanCong ?? new List<PhanCong>())
                .Select(pc => pc.MaNV)
                .Where(ma => ma != null)
                .ToHashSet();

            return danhSachNhanVien
                .Where(nv => !maDaPhanCong.Contains(nv.MaNguoiDung))
                .ToList();
        }
    }
}
