using System;
using System.Collections.Generic;
using EventTicket.Model.Entity;
using EventTicket.Model.Enums;
using EventTicket.Service;
using EventTicket.Util;

namespace EventTicket.UI
{
    public class KhachHangMenu
    {
        private readonly KhachHangService _khachHangService;

        public KhachHangMenu(KhachHangService khachHangService)
        {
            _khachHangService = khachHangService;
        }

        public void HienThiMenu()
        {
            while (true)
            {
                Console.WriteLine("\n==========================================");
                Console.WriteLine("          QUAN LY KHACH HANG              ");
                Console.WriteLine("==========================================");
                Console.WriteLine("1. Xem danh sach tat ca khach hang");
                Console.WriteLine("2. Them khach hang moi");
                Console.WriteLine("3. Tim kiem khach hang theo ten");
                Console.WriteLine("4. Tra cuu khach hang theo so dien thoai");
                Console.WriteLine("5. Xem lich su mua ve cua khach hang");
                Console.WriteLine("6. Xoa khach hang");
                Console.WriteLine("0. Quay lai menu chinh");
                Console.WriteLine("==========================================");
                Console.Write("Nhap lua chon cua ban: ");

                string input = ConsoleHelper.DocDong();
                switch (input)
                {
                    case "1": HienThiDanhSach(); break;
                    case "2": ThemKhachHangMoi(); break;
                    case "3": TimKiemTheoTen(); break;
                    case "4": TraCuuTheoSdt(); break;
                    case "5": XemLichSuMuaVe(); break;
                    case "6": XoaKhachHang(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Lua chon khong hop le! Vui long nhap lai.");
                        break;
                }
            }
        }

        private void HienThiDanhSach()
        {
            List<KhachHang> list = _khachHangService.LayTatCa();
            if (list.Count == 0)
            {
                Console.WriteLine("Chua co khach hang nao trong he thong.");
                return;
            }

            Console.WriteLine("\n--- DANH SACH KHACH HANG ---");
            InTieuDeBang();
            foreach (KhachHang kh in list)
            {
                InDongKhachHang(kh);
            }
        }

        private void ThemKhachHangMoi()
        {
            Console.WriteLine("\n--- THEM KHACH HANG MOI ---");

            Console.Write("Nhap ho ten: ");
            string hoTen = ConsoleHelper.DocDong();
            if (!ValidationUtil.LaTenHopLe(hoTen))
            {
                Console.WriteLine("Loi: Ho ten chi duoc chua chu cai va tu 2 den 50 ky tu!");
                return;
            }

            Console.Write("Nhap so dien thoai (10 so, bat dau bang 0): ");
            string sdt = ConsoleHelper.DocDong();
            if (!ValidationUtil.LaSdtHopLe(sdt))
            {
                Console.WriteLine("Loi: So dien thoai khong hop le!");
                return;
            }

            Console.Write("Nhap email: ");
            string email = ConsoleHelper.DocDong();
            if (!ValidationUtil.LaEmailHopLe(email))
            {
                Console.WriteLine("Loi: Dinh dang email khong hop le!");
                return;
            }

            Console.Write("Nhap ngay sinh (dd/MM/yyyy, VD: 15/01/2000): ");
            string ngaySinhStr = ConsoleHelper.DocDong();
            DateTime? ngaySinh;
            try
            {
                var parsedDate = DateUtil.ParseNgay(ngaySinhStr);
                ngaySinh = parsedDate.HasValue ? parsedDate.Value.ToDateTime(TimeOnly.MinValue) : ((DateTime?)null);
                if (ngaySinh != null && !(ngaySinh.Value.Date < DateTime.Today))
                {
                    Console.WriteLine("Loi: Ngay sinh phai truoc ngay hien tai!");
                    return;
                }
            }
            catch (global::System.Exception)
            {
                Console.WriteLine("Loi: Dinh dang ngay sinh khong dung (dd/MM/yyyy)!");
                return;
            }

            Console.Write("Nhap dia chi: ");
            string diaChi = ConsoleHelper.DocDong();

            Console.WriteLine("Chon loai khach hang: 1. THUONG | 2. THAN_THIET | 3. VIP");
            Console.Write("Nhap lua chon (1-3, mac dinh THUONG): ");
            string loaiStr = ConsoleHelper.DocDong();
            LoaiKhachHang loaiKhachHang = LoaiKhachHang.THUONG;
            if (loaiStr == "2")
            {
                loaiKhachHang = LoaiKhachHang.THAN_THIET;
            }
            else if (loaiStr == "3")
            {
                loaiKhachHang = LoaiKhachHang.VIP;
            }

            string maKhachHang = IdGenerator.SinhMaKhachHang();
            try
            {
                var khMoi = new KhachHang(maKhachHang, hoTen, sdt, email, ngaySinh.Value, diaChi, loaiKhachHang);
                _khachHangService.Them(khMoi);
                Console.WriteLine("Them khach hang thanh cong voi ma: " + maKhachHang);
            }
            catch (global::System.Exception e)
            {
                Console.WriteLine("Loi: " + e.Message);
            }
        }

        private void TimKiemTheoTen()
        {
            Console.Write("\nNhap ten can tim kiem: ");
            string ten = ConsoleHelper.DocDong();
            List<KhachHang> ketQua = _khachHangService.TimKiemTheoTen(ten);
            if (ketQua.Count == 0)
            {
                Console.WriteLine("Khong tim thay khach hang nao phu hop.");
                return;
            }
            Console.WriteLine("\nKet qua tim kiem (" + ketQua.Count + " khach hang):");
            InTieuDeBang();
            foreach (KhachHang kh in ketQua)
            {
                InDongKhachHang(kh);
            }
        }

        private void TraCuuTheoSdt()
        {
            Console.Write("\nNhap so dien thoai can tra cuu: ");
            string sdt = ConsoleHelper.DocDong();
            KhachHang kh = _khachHangService.TimBySdt(sdt);
            if (kh == null)
            {
                Console.WriteLine("Khong tim thay khach hang nao voi so dien thoai: " + sdt);
                return;
            }
            Console.WriteLine("\nThong tin khach hang:");
            InTieuDeBang();
            InDongKhachHang(kh);
        }

        private void XemLichSuMuaVe()
        {
            Console.Write("\nNhap so dien thoai khach hang: ");
            string sdt = ConsoleHelper.DocDong();
            KhachHang kh = _khachHangService.TimBySdt(sdt);
            if (kh == null)
            {
                Console.WriteLine("Khong tim thay khach hang co SDT nay.");
                return;
            }

            List<Ve> lichSu = _khachHangService.LayLichSuMuaVe(kh.MaKhachHang);
            if (lichSu.Count == 0)
            {
                Console.WriteLine("Khach hang '" + kh.HoTen + "' chua mua ve nao.");
                return;
            }

            Console.WriteLine("\nLich su mua ve cua: " + kh.HoTen + " (" + kh.MaKhachHang + ")");
            Console.WriteLine(string.Format("{0,-10} {1,-10} {2,-12} {3,-14} {4,-12}",
                "Ma Ve", "Su Kien", "Loai Ve", "Gia Ve (VND)", "Trang Thai"));
            Console.WriteLine("------------------------------------------------------------------");
            foreach (Ve v in lichSu)
            {
                Console.WriteLine(string.Format("{0,-10} {1,-10} {2,-12} {3,-14} {4,-12}",
                    v.MaVe, v.MaSuKien, v.LoaiVe, v.GiaVe, v.TrangThaiVe));
            }
        }

        private void XoaKhachHang()
        {
            Console.Write("\nNhap ma khach hang can xoa (VD: KH0001): ");
            string maKh = ConsoleHelper.DocDong();
            try
            {
                _khachHangService.Xoa(maKh);
            }
            catch (global::System.Exception e)
            {
                Console.WriteLine("Loi: " + e.Message);
            }
        }

        private void InTieuDeBang()
        {
            Console.WriteLine(string.Format("{0,-10} {1,-22} {2,-12} {3,-25} {4,-12} {5,-12}",
                "Ma KH", "Ho Ten", "SDT", "Email", "Ngay Sinh", "Loai KH"));
            Console.WriteLine("--------------------------------------------------------------------------------------------------");
        }

        private void InDongKhachHang(KhachHang kh)
        {
            string ns = DateUtil.FormatNgay(kh.NgaySinh.HasValue ? DateOnly.FromDateTime(kh.NgaySinh.Value) : ((DateOnly?)null));
            string loai = (kh.LoaiKhachHang != null) ? kh.LoaiKhachHang.ToString() : "N/A";
            Console.WriteLine(string.Format("{0,-10} {1,-22} {2,-12} {3,-25} {4,-12} {5,-12}",
                kh.MaKhachHang, kh.HoTen, kh.Sdt, kh.Email, ns, loai));
        }
    }
}
