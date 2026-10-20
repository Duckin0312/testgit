using System;
using System.Collections;
using System.Collections.Generic;
using EventTicket.Event;
using EventTicket.Exception;
using System.Globalization;
using System.Linq;
using EventTicket.Model.Entity;
using EventTicket.Model.Enums;
using EventTicket.Repository;
using EventTicket.Service;

namespace EventTicket
{
    /// <summary>
    /// LỚP CHẠY THỬ NGHIỆM TỰ ĐỘNG 30 CÂU TRUY VẤN (C# LINQ &amp; LAMBDA)
    /// Trực tiếp trên các Repository của hệ thống
    /// </summary>
    public static class TestQueryRunner
    {
        private static readonly string DongDoi = new string('=', 80);
        private static readonly string DongDon = new string('-', 80);

        // Java in List dạng "[a, b, c]", C# thì không nên cần hàm hỗ trợ
        private static string Chuoi(object? giaTri)
        {
            if (giaTri is string s) return s;
            if (giaTri is IEnumerable ds)
                return "[" + string.Join(", ", ds.Cast<object?>()) + "]";
            return giaTri?.ToString() ?? "null";
        }

        // Tương đương LocalDateTime.now().plusDays(n).withHour(h).withMinute(m)
        private static DateTime ThoiDiem(int soNgayThem, int gio, int phut)
        {
            return DateTime.Now.AddDays(soNgayThem).Date + new TimeSpan(gio, phut, 0);
        }

        public static void ChayTatCaTruyVan()
        {
            Console.WriteLine(DongDoi);
            Console.WriteLine("     CHƯƠNG TRÌNH CHẠY THỬ NGHIỆM 30 CÂU TRUY VẤN (C# LINQ & LAMBDAS)    ");
            Console.WriteLine(DongDoi);

            // Khởi tạo các Repository
            var veRepo = new VeRepository();
            var skRepo = new SuKienRepository();
            var ddRepo = new DiaDiemRepository();
            var khRepo = new KhachHangRepository();
            var nsRepo = new NgheSiRepository();
            var nvRepo = new NhanVienRepository();
            var ttService = new TaiTroService();

            // Khởi tạo danh sách Lịch biểu diễn mẫu
            var dsLich = new List<LichBieuDien>
            {
                new LichBieuDien("LBD01", "SK001", "NS001",
                    ThoiDiem(30, 19, 0), ThoiDiem(30, 21, 30), 1, "Hát các ca khúc Hit"),
                new LichBieuDien("LBD02", "SK001", "NS002",
                    ThoiDiem(30, 21, 30), ThoiDiem(30, 22, 30), 2, "Giao lưu Rap và kết màn"),
                new LichBieuDien("LBD03", "SK002", "NS003",
                    ThoiDiem(4, 20, 0), ThoiDiem(4, 21, 0), 1, "Acoustic Solo")
            };

            // Khởi tạo danh sách Phân công mẫu
            var dsPhanCong = new List<PhanCong>
            {
                new PhanCong("SK001", "NV001", "Điều phối âm thanh ánh sáng"),
                new PhanCong("SK001", "NV002", "Soát vé cửa A"),
                new PhanCong("SK001", "NV003", "An ninh vòng ngoài"),
                new PhanCong("SK002", "NV002", "Soát vé cửa chính")
            };

            // ==============================================================================
            // PHẦN 1: HIỂN THỊ TOÀN BỘ DỮ LIỆU BAN ĐẦU TRƯỚC KHI TRUY VẤN
            // ==============================================================================
            InToanBoDuLieuBanDau(khRepo, skRepo, ddRepo, veRepo, nsRepo, nvRepo, ttService, dsLich, dsPhanCong);

            // PHẦN 2: THỰC THI 30 CÂU TRUY VẤN (LINQ & LAMBDAS)

            Console.WriteLine("\n" + DongDoi);
            Console.WriteLine("                 BẮT ĐẦU THỰC HIỆN 30 CÂU TRUY VẤN (LAMBDAS)                    ");
            Console.WriteLine(DongDoi);
            Console.WriteLine("\n" + DongDon);
            Console.WriteLine(">>> MẢNG 1: PHÂN TÍCH KINH DOANH & BÁN VÉ (CÂU 1 -> 10)");
            Console.WriteLine(DongDon);

            Console.WriteLine("\n[Câu 1] Khách hàng thân thiết mua vé >= 3 sự kiện khác nhau trong năm nay:");
            List<string> cau1 = veRepo.Cau1_TimKhachHangThanThiet();
            Console.WriteLine("Kết quả mã khách hàng: " + Chuoi(cau1));

            Console.WriteLine("\n[Câu 2] Tỷ lệ khách hàng mới vs cũ theo từng quý:");
            veRepo.Cau2_ThongKeTyLeKhachTheoQuy();

            Console.WriteLine("\n[Câu 3] Vé đã hủy và tổng tiền hoàn lại:");
            var cau3 = veRepo.Cau3_LayVeDaHuyVaTongHoanTien();
            Console.WriteLine("Tổng tiền hoàn lại: " + cau3["TongTienHoan"] + " VND | Danh sách: " + Chuoi(cau3["DanhSachVeHuy"]));

            Console.WriteLine("\n[Câu 4] Khách mua nhiều vé nhất cho sự kiện 'SK001':");
            var cau4 = veRepo.Cau4_TimKhachMuaNhieuVeNhatTheoSuKien("SK001");
            if (cau4 != null)
            {
                Console.WriteLine($"Khách: {cau4.Value.Key} -> Số vé đã mua: {cau4.Value.Value} vé");
            }

            Console.WriteLine("\n[Câu 5] Phân tích loại khách mua vé sát ngày sự kiện nhất:");
            veRepo.Cau5_PhanTichLoaiKhachMuaSatNgay(khRepo, skRepo);

            Console.WriteLine("\n[Câu 6] Quét và tự động hủy các vé giữ chỗ quá 24h chưa thanh toán:");
            List<Ve> cau6 = veRepo.Cau6_HuyVeChuaThanhToanQua24h();
            Console.WriteLine("Số lượng vé bị tự động hủy: " + cau6.Count);

            Console.WriteLine("\n[Câu 7] Số lượng vé còn trống của các sự kiện trong tuần tới:");
            skRepo.Cau7_DemVeConTrongSuKienTuanToi(veRepo);

            Console.WriteLine("\n[Câu 8] Sức chứa các khu vực tại địa điểm 'DD001' theo loại khu vực:");
            var cau8 = ddRepo.Cau8_SucChuaTheoLoaiKhuVuc("DD001");
            foreach (var (loai, kvs) in cau8)
            {
                Console.WriteLine($" - {loai.MoTa(),-25}: {Chuoi(kvs)}");
            }

            Console.WriteLine("\n[Câu 9] Sự kiện có tỷ lệ lấp đầy vé đạt trên 95%:");
            List<string> cau9 = skRepo.Cau9_TimSuKienLapDayTren95(veRepo);
            if (cau9.Count == 0) Console.WriteLine("Không có sự kiện nào đạt > 95% lấp đầy (sức chứa lớn chưa bán hết).");
            else cau9.ForEach(Console.WriteLine);

            Console.WriteLine("\n[Câu 10] Sự kiện bị hủy và số vé cần thu hồi:");
            skRepo.Cau10_ThongKeSuKienHuyVaVeThuHoi(veRepo);

            // ══════════════════════════════════════════════════════════════════════════════
            // MẢNG 2: THÔNG TIN CƠ BẢN & NGHỆ THUẬT (CÂU 11 -> 20)
            // ══════════════════════════════════════════════════════════════════════════════
            Console.WriteLine("\n" + DongDon);
            Console.WriteLine(">>> MẢNG 2: THÔNG TIN CƠ BẢN & NGHỆ THUẬT (CÂU 11 -> 20)");
            Console.WriteLine(DongDon);

            Console.WriteLine("\n[Câu 11] Tên sự kiện, địa điểm tổ chức và sức chứa:");
            skRepo.Cau11_LietKeSuKienVaDiaDiem();

            Console.WriteLine("\n[Câu 12] Các sự kiện tổ chức tại địa điểm có sức chứa > 1.000 người:");
            foreach (var sk in skRepo.Cau12_TimSuKienSucChuaTren1000())
            {
                Console.WriteLine($" - [{sk.MaSuKien}] {sk.TenSuKien,-25} | Địa điểm: {sk.DiaDiem.TenDiaDiem} (Sức chứa: {sk.DiaDiem.SucChuaTong:N0})");
            }

            Console.WriteLine("\n[Câu 13] Tên địa điểm – Nhân viên phụ trách – Số sự kiện tổ chức:");
            ddRepo.Cau13_ThongKeDiaDiemNhanVienVaSoSuKien(skRepo, dsPhanCong, nvRepo);

            Console.WriteLine("\n[Câu 14] Khu vực có giá vé cao hơn giá vé trung bình chung:");
            foreach (var s in veRepo.Cau14_TimKhuVucGiaVeCaoHonTrungBinh())
            {
                Console.WriteLine(" - " + s);
            }

            Console.WriteLine("\n[Câu 15] Thông tin chi tiết sở hữu vé:");
            veRepo.Cau15_InChiTietSoHuuVe(khRepo, skRepo);

            Console.WriteLine("\n[Câu 16] Những khách hàng đang sở hữu vé VIP chưa sử dụng:");
            foreach (var kh in khRepo.Cau16_TimKhachHangSoHuuVeVipChuaDung(veRepo))
            {
                Console.WriteLine($" - [{kh.MaKhachHang}] {kh.HoTen,-20} | SĐT: {kh.Sdt,-12} | Loại KH: {kh.LoaiKhachHang?.MoTa()}");
            }

            Console.WriteLine("\n[Câu 17] Tên nghệ sĩ, sự kiện và địa điểm biểu diễn:");
            nsRepo.Cau17_LietKeNgheSiSuKienDiaDiem(dsLich, skRepo);

            Console.WriteLine("\n[Câu 18] Các sự kiện chưa có nghệ sĩ nào được xếp lịch biểu diễn:");
            foreach (var sk in skRepo.Cau18_TimSuKienChuaCoNgheSi(dsLich))
            {
                Console.WriteLine($" - [{sk.MaSuKien}] {sk.TenSuKien,-25} | Trạng thái: {sk.TrangThaiSuKien}");
            }

            Console.WriteLine("\n[Câu 19] Lịch biểu diễn chi tiết:");
            nsRepo.Cau19_LietKeLichBieuDienChiTiet(dsLich, skRepo);

            Console.WriteLine("\n[Câu 20] Sự kiện có tổng thời gian biểu diễn trên 2 giờ (> 120 phút):");
            foreach (var s in skRepo.Cau20_TimSuKienBieuDienTren2Gio(dsLich))
            {
                Console.WriteLine(" - " + s);
            }

            // ══════════════════════════════════════════════════════════════════════════════
            // MẢNG 3: HẬU CẦN, VẬN HÀNH & ĐỐI TÁC (CÂU 21 -> 30)
            // ══════════════════════════════════════════════════════════════════════════════
            Console.WriteLine("\n" + DongDon);
            Console.WriteLine(">>> MẢNG 3: HẬU CẦN, VẬN HÀNH & ĐỐI TÁC (CÂU 21 -> 30)");
            Console.WriteLine(DongDon);

            Console.WriteLine("\n[Câu 21] Lọc địa điểm có sức chứa > 1.000 người, sắp xếp giảm dần:");
            foreach (var dd in ddRepo.Cau21_LocDiaDiemTren1000GiamDan())
            {
                Console.WriteLine($" - [{dd.MaDiaDiem}] {dd.TenDiaDiem,-30} | Sức chứa: {dd.SucChuaTong:N0} chỗ");
            }

            Console.WriteLine("\n[Câu 22] Các khu vực có sức chứa dưới 50 người:");
            ddRepo.Cau22_TimKhuVucDuoi50Nguoi();

            Console.WriteLine("\n[Câu 23] Các khoản tài trợ giải ngân trong tháng 10/2026:");
            foreach (var tt in ttService.Cau23_LocTaiTroThang10Nam2026())
            {
                Console.WriteLine($" - Đối tác: {tt.NhaTaiTro.HoTen,-25} | Số tiền: {tt.SoTienTaiTro:N6} VND | Ngày: {tt.NgayKyKet}");
            }

            Console.WriteLine("\n[Câu 24] Danh sách chức vụ nhân viên không trùng lặp (Distinct):");
            Console.WriteLine("Chức vụ: " + Chuoi(nvRepo.Cau24_LayDanhSachChucVuDistinct()));

            Console.WriteLine("\n[Câu 25] Kết nối Phân công, Nhân viên và Sự kiện in lịch trực:");
            nvRepo.Cau25_InChiTietLichTruc(dsPhanCong, skRepo);

            Console.WriteLine("\n[Câu 26] Tổng giá trị tài trợ theo từng nhà tài trợ:");
            foreach (var (ten, tien) in ttService.Cau26_TinhTongTaiTroTheoTungNha())
            {
                Console.WriteLine($" - {ten,-30}: {tien:N6} VND");
            }

            Console.WriteLine("\n[Câu 27] Top 3 nhà tài trợ đóng góp số tiền lớn nhất:");
            foreach (var entry in ttService.Cau27_TimTop3NhaTaiTro())
            {
                Console.WriteLine($" - {entry.Key,-30}: {entry.Value:N6} VND");
            }

            Console.WriteLine("\n[Câu 28] Tổng số nhiệm vụ của từng nhân viên (kể cả 0):");
            foreach (var (ten, sl) in nvRepo.Cau28_ThongKeSoNhiemVuNhanVien(dsPhanCong))
            {
                Console.WriteLine($" - {ten,-25}: {sl} nhiệm vụ");
            }

            Console.WriteLine("\n[Câu 29] Nhân viên chưa từng được phân công nhiệm vụ nào:");
            foreach (var nv in nvRepo.Cau29_TimNhanVienChuaPhanCong(dsPhanCong))
            {
                Console.WriteLine($" - [{nv.MaNguoiDung}] {nv.HoTen,-20} ({nv.ChucVu})");
            }

            Console.WriteLine("\n[Câu 30] Địa điểm có sức chứa lớn hơn sức chứa trung bình hệ thống:");
            foreach (var dd in ddRepo.Cau30_TimDiaDiemLonHonTrungBinh())
            {
                Console.WriteLine($" - [{dd.MaDiaDiem}] {dd.TenDiaDiem,-30} | Sức chứa: {dd.SucChuaTong:N0} chỗ");
            }

            Console.WriteLine("\n" + DongDoi);
            Console.WriteLine("                 HOÀN THÀNH TEST TOÀN BỘ 30 CÂU TRUY VẤN!                      ");
            Console.WriteLine(DongDoi);
        }

        private static void InToanBoDuLieuBanDau(
            KhachHangRepository khRepo,
            SuKienRepository skRepo,
            DiaDiemRepository ddRepo,
            VeRepository veRepo,
            NgheSiRepository nsRepo,
            NhanVienRepository nvRepo,
            TaiTroService ttService,
            List<LichBieuDien> dsLich,
            List<PhanCong> dsPhanCong)
        {
            Console.WriteLine("\n" + DongDon);
            Console.WriteLine(">>> DỮ LIỆU GỐC TRƯỚC KHI TRUY VẤN (RAW DATA TRONG HỆ THỐNG)");
            Console.WriteLine(DongDon);

            Console.WriteLine("\n1. DANH SÁCH KHÁCH HÀNG:");
            foreach (var kh in khRepo.LayTatCa())
            {
                Console.WriteLine($"   • [{kh.MaKhachHang}] {kh.HoTen,-20} | SĐT: {kh.Sdt,-12} | Email: {kh.Email,-22} | Loại: {kh.LoaiKhachHang?.MoTa()}");
            }

            Console.WriteLine("\n2. DANH SÁCH ĐỊA ĐIỂM & KHU VỰC:");
            foreach (var dd in ddRepo.LayTatCa())
            {
                Console.WriteLine($"   • [{dd.MaDiaDiem}] {dd.TenDiaDiem,-30} | Sức chứa: {dd.SucChuaTong:N0} chỗ | Địa chỉ: {dd.DiaChi}");
                foreach (var kv in dd.DanhSachKhuVuc)
                {
                    Console.WriteLine($"      - [{kv.MaKhuVuc}] {kv.TenKhuVuc,-20} | Loại: {kv.LoaiKhuVuc?.MoTa(),-22} | Sức chứa: {kv.SucChua:N0} chỗ");
                }
            }

            Console.WriteLine("\n3. DANH SÁCH SỰ KIỆN:");
            foreach (var sk in skRepo.LayTatCa())
            {
                string diaDiem = sk.DiaDiem != null ? sk.DiaDiem.TenDiaDiem : "N/A";
                Console.WriteLine($"   • [{sk.MaSuKien}] {sk.TenSuKien,-25} | Địa điểm: {diaDiem,-25} | Trạng thái: {sk.TrangThaiSuKien,-10} | Thời gian: {sk.ThoiGianDienRa}");
            }

            Console.WriteLine("\n4. DANH SÁCH VÉ HIỆN CÓ TRONG HỆ THỐNG:");
            foreach (var v in veRepo.LayTatCa())
            {
                Console.WriteLine($"   • [{v.MaVe}] Sự kiện: {v.MaSuKien,-6} | Khách: {v.MaKhachHang,-6} | Khu vực: {v.MaKhuVuc,-5} | Loại: {v.LoaiVe,-8} | Giá: {v.GiaVe:N0} VND | TT: {v.TrangThaiVe?.MoTa(),-10} | Mua lúc: {v.ThoiGianMua}");
            }

            Console.WriteLine("\n5. DANH SÁCH NGHỆ SĨ & LỊCH DIỄN:");
            foreach (var ns in nsRepo.LayTatCa())
            {
                Console.WriteLine($"   • [{ns.MaNgheSi}] {ns.TenNgheSi,-20} | Thể loại: {ns.TheLoaiBieuDien}");
            }
            foreach (var l in dsLich)
            {
                Console.WriteLine($"      - Lịch [{l.MaLich}]: Sự kiện {l.MaSuKien,-6} | Nghệ sĩ: {l.MaNgheSi,-6} | Từ: {l.ThoiGianBatDau} Đến: {l.ThoiGianKetThuc} | Tiết mục: {l.NoiDungBieuDien}");
            }

            Console.WriteLine("\n6. DANH SÁCH NHÂN VIÊN & PHÂN CÔNG:");
            foreach (var nv in nvRepo.LayTatCa())
            {
                Console.WriteLine($"   • [{nv.MaNguoiDung}] {nv.HoTen,-20} | Chức vụ: {nv.ChucVu,-20} | Quyền: {nv.Quyen}");
            }
            foreach (var pc in dsPhanCong)
            {
                Console.WriteLine($"      - Phân công: Sự kiện {pc.MaSuKien,-6} | NV: {pc.MaNV,-6} | Nhiệm vụ: {pc.NhiemVu}");
            }

            Console.WriteLine("\n7. DANH SÁCH NHÀ TÀI TRỢ:");
            foreach (var tt in ttService.LayTatCa())
            {
                Console.WriteLine($"   • Nhà tài trợ: {tt.NhaTaiTro.HoTen,-30} | Sự kiện: {tt.SuKien.TenSuKien,-25} | Số tiền: {tt.SoTienTaiTro:N0} VND | Gói: {tt.GoiTaiTro}");
            }
        }
    }
}
