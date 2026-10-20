using System;
using System.Collections.Generic;
using System.Linq;
using EventTicket.Model.Entity;

namespace EventTicket.Repository
{
    public class NgheSiRepository : GenericRepository<NgheSi>
    {
        private readonly List<NgheSi> danhSachNgheSi = new List<NgheSi>();

        public NgheSiRepository()
        {
            // Du lieu mau ban dau
            NgheSi ns1 = new NgheSi("NS001", "Son Tung M-TP", "Pop / R&B");
            NgheSi ns2 = new NgheSi("NS002", "Den Vau", "Rap / Hip Hop");
            NgheSi ns3 = new NgheSi("NS003", "Vu", "Indie Pop");

            danhSachNgheSi.Add(ns1);
            danhSachNgheSi.Add(ns2);
            danhSachNgheSi.Add(ns3);
        }

        public override void Them(NgheSi entity)
        {
            danhSachNgheSi.Add(entity);
            Console.WriteLine("[Repo] Da them nghe si: " + entity.MaNgheSi);
        }

        public override void Sua(NgheSi entity)
        {
            for (int i = 0; i < danhSachNgheSi.Count; i++)
            {
                if (string.Equals(danhSachNgheSi[i].MaNgheSi, entity.MaNgheSi, StringComparison.OrdinalIgnoreCase))
                {
                    danhSachNgheSi[i] = entity;
                    Console.WriteLine("[Repo] Da cap nhat nghe si: " + entity.MaNgheSi);
                    return;
                }
            }
        }

        public override void Xoa(string maNgheSi)
        {
            danhSachNgheSi.RemoveAll(ns => string.Equals(ns.MaNgheSi, maNgheSi, StringComparison.OrdinalIgnoreCase));
            Console.WriteLine("[Repo] Da xoa nghe si: " + maNgheSi);
        }

        public override NgheSi TimById(string maNgheSi)
        {
            if (maNgheSi == null) return null;
            return danhSachNgheSi
                .FirstOrDefault(ns => ns.MaNgheSi != null && ns.MaNgheSi.Equals(maNgheSi, StringComparison.OrdinalIgnoreCase));
        }

        public override List<NgheSi> LayTatCa()
        {
            return new List<NgheSi>(danhSachNgheSi);
        }

        public bool KiemTraMaDaTonTai(string maNgheSi)
        {
            if (maNgheSi == null) return false;
            return danhSachNgheSi
                .Any(ns => ns.MaNgheSi != null && ns.MaNgheSi.Equals(maNgheSi, StringComparison.OrdinalIgnoreCase));
        }

        // Loc danh sach nghe si theo the loai bieu dien
        public List<NgheSi> TimTheoTheLoai(string theLoai)
        {
            if (theLoai == null || string.IsNullOrWhiteSpace(theLoai)) return new List<NgheSi>();
            string theLoaiLower = theLoai.Trim().ToLowerInvariant();
            return danhSachNgheSi
                .Where(ns => ns.TheLoaiBieuDien != null && ns.TheLoaiBieuDien.ToLowerInvariant().Contains(theLoaiLower))
                .ToList();
        }

        // =========================================================================
        // CÁC TRUY VẤN NGHIỆP VỤ (LINQ)
        // =========================================================================

        /// <summary>
        /// CÂU 17: Liệt kê tên nghệ sĩ, sự kiện và địa điểm mà nghệ sĩ đó biểu diễn.
        /// </summary>
        public void Cau17_LietKeNgheSiSuKienDiaDiem(List<LichBieuDien> dsLich, SuKienRepository skRepo)
        {
            if (dsLich == null) return;
            Dictionary<string, string> mapNS = danhSachNgheSi
                .ToDictionaryKeepFirst(ns => ns.MaNgheSi, ns => ns.TenNgheSi);
            Dictionary<string, SuKien> mapSK = skRepo.LayTatCa()
                .ToDictionaryKeepFirst(sk => sk.MaSuKien, sk => sk);

            IEnumerable<string> dong = dsLich
                .Select(l =>
                {
                    string tenNS = mapNS.GetValueOrDefault(l.MaNgheSi, l.MaNgheSi);
                    SuKien sk = mapSK.GetValueOrDefault(l.MaSuKien);
                    string tenSK = sk != null ? sk.TenSuKien : l.MaSuKien;
                    string tenDD = (sk != null && sk.DiaDiem != null) ? sk.DiaDiem.TenDiaDiem : "N/A";
                    return $"Nghệ sĩ: {tenNS,-18} | Sự kiện: {tenSK,-25} | Địa điểm: {tenDD}";
                })
                .Distinct();

            foreach (string d in dong)
            {
                Console.WriteLine(d);
            }
        }

        /// <summary>
        /// CÂU 19: Liệt kê lịch biểu diễn gồm tên nghệ sĩ – tên sự kiện – địa điểm – thời gian bắt đầu/kết thúc.
        /// </summary>
        public void Cau19_LietKeLichBieuDienChiTiet(List<LichBieuDien> dsLich, SuKienRepository skRepo)
        {
            if (dsLich == null) return;
            Dictionary<string, string> mapNS = danhSachNgheSi
                .ToDictionaryKeepFirst(ns => ns.MaNgheSi, ns => ns.TenNgheSi);
            Dictionary<string, SuKien> mapSK = skRepo.LayTatCa()
                .ToDictionaryKeepFirst(sk => sk.MaSuKien, sk => sk);

            IEnumerable<string> dong = dsLich
                .OrderBy(l => l.ThoiGianBatDau == null) // null xếp cuối (nullsLast)
                .ThenBy(l => l.ThoiGianBatDau)
                .Select(l =>
                {
                    string tenNS = mapNS.GetValueOrDefault(l.MaNgheSi, "N/A");
                    SuKien sk = mapSK.GetValueOrDefault(l.MaSuKien);
                    string tenSK = sk != null ? sk.TenSuKien : "N/A";
                    string diaDiem = (sk != null && sk.DiaDiem != null) ? sk.DiaDiem.TenDiaDiem : "N/A";
                    return $"Nghệ sĩ: {tenNS,-16} | Sự kiện: {tenSK,-22} | Địa điểm: {diaDiem,-18} | {l.ThoiGianBatDau} -> {l.ThoiGianKetThuc}";
                });

            foreach (string d in dong)
            {
                Console.WriteLine(d);
            }
        }
    }
}
