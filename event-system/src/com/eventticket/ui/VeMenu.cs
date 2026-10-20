using System;
using System.Collections.Generic;
using EventTicket.Exception;
using EventTicket.Model.Entity;
using EventTicket.Model.Enums;
using EventTicket.Service;
using EventTicket.Util;

namespace EventTicket.UI
{
    public class VeMenu
    {
        private readonly VeService _veService;
        private readonly SuKienService _suKienService;
        private readonly KhachHangService _khachHangService;

        public VeMenu(VeService veService, SuKienService suKienService, KhachHangService khachHangService)
        {
            _veService = veService;
            _suKienService = suKienService;
            _khachHangService = khachHangService;
        }

        public void HienThiMenu()
        {
            while (true)
            {
                Console.WriteLine("\n==========================================");
                Console.WriteLine("            QUAN LY BAN VE               ");
                Console.WriteLine("==========================================");
                Console.WriteLine("1. Xem danh sach tat ca ve");
                Console.WriteLine("2. Ban ve moi cho khach hang");
                Console.WriteLine("3. Xem danh sach ve theo su kien");
                Console.WriteLine("4. Xem lich su ve cua khach hang");
                Console.WriteLine("5. Huy ve (hoan ve)");
                Console.WriteLine("6. Xem tong doanh thu ban ve");
                Console.WriteLine("0. Quay lai menu chinh");
                Console.WriteLine("==========================================");
                Console.Write("Nhap lua chon cua ban: ");

                string input = ConsoleHelper.DocDong();
                switch (input)
                {
                    case "1": HienThiTatCaVe(); break;
                    case "2": ThucHienBanVe(); break;
                    case "3": XemVeTheoSuKien(); break;
                    case "4": XemVeTheoKhachHang(); break;
                    case "5": ThucHienHuyVe(); break;
                    case "6":
                        Console.WriteLine("\nTong doanh thu tat ca ve da ban: " + _veService.TinhTongDoanhThu() + " VND");
                        break;
                    case "0": return;
                    default:
                        Console.WriteLine("Lua chon khong hop le! Vui long nhap lai.");
                        break;
                }
            }
        }

        private void HienThiTatCaVe()
        {
            List<Ve> list = _veService.LayTatCa();
            if (list.Count == 0)
            {
                Console.WriteLine("He thong chua co ve nao.");
                return;
            }
            InTieuDeBangVe();
            foreach (Ve v in list)
            {
                InDongVe(v);
            }
        }

        private void ThucHienBanVe()
        {
            Console.WriteLine("\n--- TIEN HANH BAN VE ---");

            // 1. Chon khach hang theo SDT
            Console.Write("Nhap so dien thoai khach hang: ");
            string sdt = ConsoleHelper.DocDong();
            KhachHang kh = _khachHangService.TimBySdt(sdt);
            if (kh == null)
            {
                Console.WriteLine("Loi: Khong tim thay khach hang co SDT nay! Vui long tao khach hang truoc.");
                return;
            }
            Console.WriteLine("Khach hang: " + kh.HoTen + " (" + kh.LoaiKhachHang + ")");

            // 2. Chon su kien
            List<SuKien> dsSuKien = _suKienService.LayTatCa();
            if (dsSuKien.Count == 0)
            {
                Console.WriteLine("Loi: Hien tai chua co su kien nao trong he thong.");
                return;
            }

            Console.WriteLine("Danh sach su kien:");
            for (int i = 0; i < dsSuKien.Count; i++)
            {
                SuKien s = dsSuKien[i];
                Console.WriteLine(string.Format(" {0}. [{1}] {2} (Trang thai: {3})",
                    i + 1, s.MaSuKien, s.TenSuKien, s.TrangThaiSuKien));
            }

            Console.Write("Chon so thu tu su kien: ");
            string chonSkStr = ConsoleHelper.DocDong();
            SuKien sk = null;
            if (int.TryParse(chonSkStr, out int chon))
            {
                int idx = chon - 1;
                if (idx >= 0 && idx < dsSuKien.Count)
                {
                    sk = dsSuKien[idx];
                }
            }

            if (sk == null)
            {
                Console.WriteLine("Loi: Lua chon su kien khong hop le!");
                return;
            }

            // 3. Chon loai ve
            Console.WriteLine("Chon loai ve: 1. STANDARD (x1.0) | 2. PREMIUM (x2.0) | 3. VIP (x3.0)");
            Console.Write("Nhap lua chon (1-3): ");
            string chonLoai = ConsoleHelper.DocDong();
            LoaiVe loaiVe = LoaiVe.STANDARD;
            if (chonLoai == "2")
            {
                loaiVe = LoaiVe.PREMIUM;
            }
            else if (chonLoai == "3")
            {
                loaiVe = LoaiVe.VIP;
            }

            // 4. Gia co ban va tinh gia theo loai ve
            Console.Write("Nhap gia ve co ban (VND, VD: 200000): ");
            string giaStr = ConsoleHelper.DocDong();
            try
            {
                decimal giaCoBan = decimal.Parse(giaStr, System.Globalization.CultureInfo.InvariantCulture);
                decimal giaThucTe = giaCoBan * (decimal)loaiVe.HeSoGia();

                // 5. Tao ma ve va luu
                string maVe = IdGenerator.SinhMaVe();
                string maKhuVuc = "KV01";

                var veMoi = new Ve(maVe, kh.MaKhachHang, sk.MaSuKien, maKhuVuc,
                        loaiVe, TrangThaiVe.SOLD, giaThucTe, DateTime.Now);

                // Goi BanVe co truyen khach hang de kich hoat EventBus (Email, ThongBao)
                _veService.BanVe(veMoi, kh);

                // Cap nhat vao lich su mua ve cua khach hang
                kh.MuaVe(veMoi);

                Console.WriteLine("\nXuat ve thanh cong!");
                Console.WriteLine(string.Format("Ma ve: {0} | Khach: {1} | Su kien: {2} | Loai: {3} | Thanh tien: {4} VND",
                    veMoi.MaVe, kh.HoTen, sk.TenSuKien, loaiVe.MoTa(), giaThucTe));
            }
            catch (VeException e)
            {
                Console.WriteLine("Loi nghiep vu ve: " + e.Message);
            }
            catch (FormatException)
            {
                Console.WriteLine("Loi: Gia ve co ban phai la mot con so hop le!");
            }
            catch (global::System.Exception e)
            {
                Console.WriteLine("Loi he thong khi ban ve: " + e.Message);
            }
        }

        private void XemVeTheoSuKien()
        {
            Console.Write("\nNhap ma su kien can tra cuu: ");
            string maSuKien = ConsoleHelper.DocDong();
            List<Ve> list = _veService.LayVeTheoSuKien(maSuKien);
            if (list.Count == 0)
            {
                Console.WriteLine("Chua co ve nao duoc ban cho su kien nay.");
                return;
            }
            InTieuDeBangVe();
            foreach (Ve v in list)
            {
                InDongVe(v);
            }
            Console.WriteLine("Tong doanh thu su kien: " + _veService.TinhDoanhThuTheoSuKien(maSuKien) + " VND");
        }

        private void XemVeTheoKhachHang()
        {
            Console.Write("\nNhap so dien thoai khach hang: ");
            string sdt = ConsoleHelper.DocDong();
            KhachHang kh = _khachHangService.TimBySdt(sdt);
            if (kh == null)
            {
                Console.WriteLine("Khong tim thay khach hang voi SDT nay.");
                return;
            }

            List<Ve> list = _veService.LayVeTheoKhachHang(kh.MaKhachHang);
            if (list.Count == 0)
            {
                Console.WriteLine("Khach hang nay chua mua ve nao.");
                return;
            }
            Console.WriteLine("\nLich su mua ve cua: " + kh.HoTen);
            InTieuDeBangVe();
            foreach (Ve v in list)
            {
                InDongVe(v);
            }
        }

        private void ThucHienHuyVe()
        {
            Console.Write("\nNhap ma ve can huy: ");
            string maVe = ConsoleHelper.DocDong();
            try
            {
                _veService.HuyVe(maVe);
            }
            catch (VeException e)
            {
                Console.WriteLine("Loi nghiep vu ve: " + e.Message);
            }
            catch (global::System.Exception e)
            {
                Console.WriteLine("Loi khi huy ve: " + e.Message);
            }
        }

        private void InTieuDeBangVe()
        {
            Console.WriteLine(string.Format("{0,-10} {1,-10} {2,-10} {3,-12} {4,-14} {5,-12}",
                "Ma Ve", "Su Kien", "Khach Hang", "Loai Ve", "Gia Ve (VND)", "Trang Thai"));
            Console.WriteLine("-------------------------------------------------------------------------");
        }

        private void InDongVe(Ve v)
        {
            string loaiStr = (v.LoaiVe != null) ? v.LoaiVe.ToString() : "N/A";
            string ttStr = (v.TrangThaiVe != null) ? v.TrangThaiVe.ToString() : "N/A";
            Console.WriteLine(string.Format("{0,-10} {1,-10} {2,-10} {3,-12} {4,-14} {5,-12}",
                v.MaVe, v.MaSuKien, v.MaKhachHang, loaiStr, v.GiaVe, ttStr));
        }
    }
}
