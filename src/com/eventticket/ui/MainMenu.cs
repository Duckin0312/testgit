using System;
using System.Collections.Generic;
using EventTicket.Event;
using EventTicket.Model.Entity;
using EventTicket.Service;

namespace EventTicket.UI
{
    public class MainMenu
    {
        private readonly NhanVienService _nhanVienService = new NhanVienService();
        private readonly KhachHangService _khachHangService = new KhachHangService();
        private readonly SuKienService _suKienService = new SuKienService();
        private readonly VeService _veService = new VeService();
        private readonly DiaDiemService _diaDiemService = new DiaDiemService();
        private readonly NgheSiService _ngheSiService = new NgheSiService();
        private readonly TaiTroService _taiTroService = new TaiTroService();
        private readonly BaoCaoService _baoCaoService;

        private NhanVien _currentUser = null;

        public MainMenu()
        {
            _baoCaoService = new BaoCaoService(_suKienService, _veService, _khachHangService, _taiTroService);

            // Dang ky cac Listener vao EventBus khi khoi dong he thong
            EventBus.Instance.DangKy(new EmailListener());
            EventBus.Instance.DangKy(new ThongBaoListener());
            EventBus.Instance.DangKy(new LogListener());
            EventBus.Instance.DangKy(new LogListener());
        }

        public void HienThi()
        {
            Console.WriteLine("====================================================");
            Console.WriteLine("     HE THONG QUAN LY SU KIEN VA BAN VE CONCERT     ");
            Console.WriteLine("====================================================");

            // Vong lap dang nhap
            while (_currentUser == null)
            {
                Console.WriteLine("\n--- DANG NHAP HE THONG ---");
                Console.WriteLine("(Goi y tai khoan mac dinh: admin / admin123456)");
                Console.Write("Nhap tai khoan (Ma nguoi dung): ");
                string username = ConsoleHelper.DocDong();
                Console.Write("Nhap mat khau: ");
                string password = ConsoleHelper.DocDong();

                _currentUser = _nhanVienService.DangNhap(username, password);
            }

            // Vong lap Menu chinh sau khi dang nhap thanh cong
            while (true)
            {
                Console.WriteLine("\n====================================================");
                Console.WriteLine("MENU CHINH - Nguoi dung: " + _currentUser.HoTen + " (" + _currentUser.Quyen + ")");
                Console.WriteLine("====================================================");
                Console.WriteLine("1. Quan ly Su Kien");
                Console.WriteLine("2. Quan ly Khach Hang");
                Console.WriteLine("3. Quan ly Ban Ve");
                Console.WriteLine("4. Quan ly Nhan Vien & Doi mat khau");
                Console.WriteLine("5. Quan ly Bao Cao & Thong Ke");
                Console.WriteLine("6. Xuat Bao Cao ra file txt");
                Console.WriteLine("7. Chay thu nghiem 30 cau truy van Lambda (Mang 1, 2, 3)");
                Console.WriteLine("0. Dang xuat va Thoat");
                Console.WriteLine("====================================================");
                Console.Write("Nhap lua chon cua ban: ");

                string choice = ConsoleHelper.DocDong();
                switch (choice)
                {
                    case "1":
                        new SuKienMenu(_suKienService, _diaDiemService).HienThiMenu();
                        break;
                    case "2":
                        new KhachHangMenu(_khachHangService).HienThiMenu();
                        break;
                    case "3":
                        new VeMenu(_veService, _suKienService, _khachHangService).HienThiMenu();
                        break;
                    case "4":
                        new NhanVienMenu(_nhanVienService, _currentUser).HienThiMenu();
                        break;
                    case "5":
                        new BaoCaoMenu(_suKienService, _veService, _khachHangService, _taiTroService, _baoCaoService).HienThiMenu();
                        break;
                    case "6":
                        _baoCaoService.XuatRaFile("bao_cao_su_kien.txt");
                        break;
                    case "7":
                        TestQueryRunner.ChayTatCaTruyVan();
                        break;
                    case "0":
                        Console.WriteLine("Cam on ban da su dung chuong trinh. Tam biet!");
                        return;
                    default:
                        Console.WriteLine("Lua chon khong hop le! Vui long nhap lai.");
                        break;
                }
            }
        }

        private void HienThiSuKien()
        {
            Console.WriteLine("\n--- DANH SACH SU KIEN ---");
            List<SuKien> list = _suKienService.LayTatCa();
            foreach (SuKien sk in list)
            {
                Console.WriteLine(string.Format("[{0}] {1} - Thoi gian: {2} - Trang thai: {3}",
                    sk.MaSuKien, sk.TenSuKien, sk.ThoiGianDienRa, sk.TrangThaiSuKien));
            }
        }

        private void HienThiKhachHang()
        {
            Console.WriteLine("\n--- DANH SACH KHACH HANG ---");
            List<KhachHang> list = _khachHangService.LayTatCa();
            foreach (KhachHang kh in list)
            {
                Console.WriteLine(string.Format("[{0}] {1} - SDT: {2} - Loai: {3}",
                    kh.MaKhachHang, kh.HoTen, kh.Sdt, kh.LoaiKhachHang));
            }
        }

        private void HienThiVe()
        {
            Console.WriteLine("\n--- DANH SACH VE ---");
            List<Ve> list = _veService.LayTatCa();
            foreach (Ve v in list)
            {
                Console.WriteLine(string.Format("[{0}] Su kien: {1} - Khach: {2} - Gia: {3} VND - Trang thai: {4}",
                    v.MaVe, v.MaSuKien, v.MaKhachHang, v.GiaVe, v.TrangThaiVe));
            }
        }

        private void HienThiNhanVien()
        {
            Console.WriteLine("\n--- DANH SACH NHAN VIEN ---");
            List<NhanVien> list = _nhanVienService.LayTatCa();
            foreach (NhanVien nv in list)
            {
                Console.WriteLine(string.Format("[{0}] {1} - Quyen: {2} - Chuc vu: {3}",
                    nv.MaNguoiDung, nv.HoTen, nv.Quyen, nv.ChucVu));
            }
        }
    }
}
