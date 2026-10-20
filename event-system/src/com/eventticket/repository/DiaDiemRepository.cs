using System;
using System.Collections.Generic;
using System.Linq;
using EventTicket.Model.Entity;
using EventTicket.Model.Enums;

namespace EventTicket.Repository
{
    public class DiaDiemRepository : GenericRepository<DiaDiem>
    {
        private readonly List<DiaDiem> danhSachDiaDiem = new List<DiaDiem>();

        public DiaDiemRepository()
        {
            // Du lieu mau ban dau
            DiaDiem dd1 = new DiaDiem("DD001", "San van dong My Dinh", "Le Duc Tho, Nam Tu Liem, Ha Noi");
            dd1.ThemKhuVuc(new KhuVuc("KV01", "Khu Vuc VIP A", LoaiKhuVuc.KHAN_GIA_VIP, 500));
            dd1.ThemKhuVuc(new KhuVuc("KV02", "Khu Vuc Thuong B", LoaiKhuVuc.KHAN_GIA_THUONG, 3000));
            dd1.ThemKhuVuc(new KhuVuc("KV03", "VIP Lounge", LoaiKhuVuc.VIP_LOUNGE, 40));

            DiaDiem dd2 = new DiaDiem("DD002", "Nha thi dau Phu Tho", "1 Lữ Gia, Quan 11, TP Ho Chi Minh");
            dd2.ThemKhuVuc(new KhuVuc("KV04", "Khan Dai Chinh", LoaiKhuVuc.KHAN_GIA_THUONG, 1200));
            dd2.ThemKhuVuc(new KhuVuc("KV05", "Khu Phong Cho", LoaiKhuVuc.VIP_LOUNGE, 30));

            DiaDiem dd3 = new DiaDiem("DD003", "Trung tam Hoi nghi Quoc gia", "Pham Hung, Me Tri, Ha Noi");
            dd3.ThemKhuVuc(new KhuVuc("KV06", "Hoi Truong 1", LoaiKhuVuc.KHAN_GIA_VIP, 800));

            danhSachDiaDiem.Add(dd1);
            danhSachDiaDiem.Add(dd2);
            danhSachDiaDiem.Add(dd3);
        }

        public override void Them(DiaDiem entity)
        {
            danhSachDiaDiem.Add(entity);
            Console.WriteLine("[Repo] Da them dia diem: " + entity.MaDiaDiem);
        }

        public override void Sua(DiaDiem entity)
        {
            for (int i = 0; i < danhSachDiaDiem.Count; i++)
            {
                if (string.Equals(danhSachDiaDiem[i].MaDiaDiem, entity.MaDiaDiem, StringComparison.OrdinalIgnoreCase))
                {
                    danhSachDiaDiem[i] = entity;
                    Console.WriteLine("[Repo] Da cap nhat dia diem: " + entity.MaDiaDiem);
                    return;
                }
            }
        }

        public override void Xoa(string maDiaDiem)
        {
            danhSachDiaDiem.RemoveAll(dd => string.Equals(dd.MaDiaDiem, maDiaDiem, StringComparison.OrdinalIgnoreCase));
            Console.WriteLine("[Repo] Da xoa dia diem: " + maDiaDiem);
        }

        public override DiaDiem TimById(string maDiaDiem)
        {
            if (maDiaDiem == null) return null;
            return danhSachDiaDiem
                .FirstOrDefault(dd => dd.MaDiaDiem != null && dd.MaDiaDiem.Equals(maDiaDiem, StringComparison.OrdinalIgnoreCase));
        }

        public override List<DiaDiem> LayTatCa()
        {
            return new List<DiaDiem>(danhSachDiaDiem);
        }

        public bool KiemTraMaDaTonTai(string maDiaDiem)
        {
            if (maDiaDiem == null) return false;
            return danhSachDiaDiem
                .Any(dd => dd.MaDiaDiem != null && dd.MaDiaDiem.Equals(maDiaDiem, StringComparison.OrdinalIgnoreCase));
        }

        // =========================================================================
        // CÁC TRUY VẤN NGHIỆP VỤ (LINQ)
        // =========================================================================

        /// <summary>
        /// CÂU 8: Liệt kê chi tiết sức chứa của từng khu vực trong một địa điểm, phân loại theo LoaiKhuVuc.
        /// </summary>
        public Dictionary<LoaiKhuVuc, List<KhuVuc>> Cau8_SucChuaTheoLoaiKhuVuc(string maDiaDiem)
        {
            DiaDiem dd = TimById(maDiaDiem);
            if (dd == null) return new Dictionary<LoaiKhuVuc, List<KhuVuc>>();
            return dd.DanhSachKhuVuc
                .Where(kv => kv.LoaiKhuVuc != null)
                .GroupBy(kv => kv.LoaiKhuVuc.Value)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        /// <summary>
        /// CÂU 13: Với mỗi địa điểm, liệt kê tên địa điểm – nhân viên phụ trách – số sự kiện được tổ chức tại đó.
        /// </summary>
        public void Cau13_ThongKeDiaDiemNhanVienVaSoSuKien(
            SuKienRepository skRepo,
            List<PhanCong> dsPhanCong,
            NhanVienRepository nvRepo)
        {
            Dictionary<string, string> mapNV = nvRepo.LayTatCa()
                .ToDictionaryKeepFirst(nv => nv.MaNguoiDung, nv => nv.HoTen);

            Dictionary<string, List<string>> nvTheoSK = (dsPhanCong ?? new List<PhanCong>())
                .GroupBy(pc => pc.MaSuKien)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(pc => mapNV.GetValueOrDefault(pc.MaNV, pc.MaNV)).ToList());

            Dictionary<string, List<SuKien>> skTheoDD = skRepo.LayTatCa()
                .Where(sk => sk.DiaDiem != null)
                .GroupBy(sk => sk.DiaDiem.MaDiaDiem)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (DiaDiem dd in danhSachDiaDiem)
            {
                List<SuKien> skList = skTheoDD.GetValueOrDefault(dd.MaDiaDiem, new List<SuKien>());
                string nvPhuTrach = string.Join(", ", skList
                    .SelectMany(sk => nvTheoSK.GetValueOrDefault(sk.MaSuKien, new List<string>()))
                    .Distinct());
                if (nvPhuTrach.Length == 0) nvPhuTrach = "Chưa có";
                Console.WriteLine($"Địa điểm: {dd.TenDiaDiem,-25} | NV phụ trách: {nvPhuTrach,-20} | Số sự kiện: {skList.Count}");
            }
        }

        /// <summary>
        /// CÂU 21: Lọc địa điểm có sức chứa trên 1000 người, sắp xếp giảm dần.
        /// </summary>
        public List<DiaDiem> Cau21_LocDiaDiemTren1000GiamDan()
        {
            return danhSachDiaDiem
                .Where(dd => dd.SucChuaTong > 1000)
                .OrderByDescending(dd => dd.SucChuaTong)
                .ToList();
        }

        /// <summary>
        /// CÂU 22: Tìm các khu vực (thuộc địa điểm) có sức chứa dưới 50 người.
        /// </summary>
        public void Cau22_TimKhuVucDuoi50Nguoi()
        {
            foreach (DiaDiem dd in danhSachDiaDiem)
            {
                foreach (KhuVuc kv in dd.DanhSachKhuVuc.Where(k => k.SucChua < 50))
                {
                    Console.WriteLine($"Địa điểm: {dd.TenDiaDiem,-25} | Khu vực: {kv.TenKhuVuc,-15} | Sức chứa: {kv.SucChua} chỗ");
                }
            }
        }

        /// <summary>
        /// CÂU 30: Tìm địa điểm có sức chứa lớn hơn sức chứa trung bình của toàn hệ thống.
        /// </summary>
        public List<DiaDiem> Cau30_TimDiaDiemLonHonTrungBinh()
        {
            double sucChuaTB = danhSachDiaDiem.Count > 0
                ? danhSachDiaDiem.Average(dd => dd.SucChuaTong)
                : 0.0;

            Console.WriteLine($"Sức chứa trung bình hệ thống: {sucChuaTB:N1} chỗ");

            return danhSachDiaDiem
                .Where(dd => dd.SucChuaTong > sucChuaTB)
                .OrderByDescending(dd => dd.SucChuaTong)
                .ToList();
        }
    }
}
