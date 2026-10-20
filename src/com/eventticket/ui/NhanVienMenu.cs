using System;
using System.Collections.Generic;
using EventTicket.Model.Entity;
using EventTicket.Model.Enums;
using EventTicket.Service;
using EventTicket.Util;

namespace EventTicket.UI
{
    public class NhanVienMenu
    {
        private const string Line = "--------------------------------------------------------------------------------------------------------";

        private readonly NhanVienService _nhanVienService;
        private readonly NhanVien _currentUser;

        public NhanVienMenu(NhanVienService nhanVienService, NhanVien currentUser)
        {
            _nhanVienService = nhanVienService;
            _currentUser = currentUser;
        }

        public void HienThiMenu()
        {
            while (true)
            {
                Console.WriteLine("\n==========================================");
                Console.WriteLine("            QUAN LY NHAN VIEN             ");
                Console.WriteLine("==========================================");
                Console.WriteLine("1. Xem danh sach tat ca nhan vien");
                Console.WriteLine("2. Them nhan vien moi (MK mac dinh: 123456)");
                Console.WriteLine("3. Chinh sua thong tin nhan vien");
                Console.WriteLine("4. Xoa nhan vien (chan xoa ADMIN)");
                Console.WriteLine("5. Tim kiem nhan vien theo ten");
                Console.WriteLine("6. Doi mat khau tai khoan ca nhan");
                Console.WriteLine("0. Quay lai menu chinh");
                Console.WriteLine("==========================================");
                Console.Write("Nhap lua chon cua ban: ");

                string input = ConsoleHelper.DocDong();
                switch (input)
                {
                    case "1": XemDanhSach(); break;
                    case "2": ThemNhanVien(); break;
                    case "3": ChinhSuaNhanVien(); break;
                    case "4": XoaNhanVien(); break;
                    case "5": TimKiemNhanVien(); break;
                    case "6": DoiMatKhauCaNhan(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Lua chon khong hop le! Vui long nhap lai.");
                        break;
                }
            }
        }

        private void InTieuDeBang()
        {
            Console.WriteLine(Line);
            Console.WriteLine(string.Format("| {0,-8} | {1,-20} | {2,-12} | {3,-26} | {4,-18} | {5,-10} |",
                "Ma NV", "Ho va Ten", "SDT", "Email", "Chuc vu", "Quyen"));
            Console.WriteLine(Line);
        }

        private void InDongNhanVien(NhanVien nv)
        {
            Console.WriteLine(string.Format("| {0,-8} | {1,-20} | {2,-12} | {3,-26} | {4,-18} | {5,-10} |",
                nv.MaNguoiDung,
                nv.HoTen ?? "N/A",
                nv.Sdt ?? "N/A",
                nv.Email ?? "N/A",
                nv.ChucVu ?? "N/A",
                nv.Quyen != null ? nv.Quyen.ToString() : "N/A"));
        }

        private void XemDanhSach()
        {
            List<NhanVien> list = _nhanVienService.LayTatCa();
            if (list.Count == 0)
            {
                Console.WriteLine("He thong chua co nhan vien nao.");
                return;
            }

            Console.WriteLine("\n--- DANH SACH NHAN VIEN HE THONG ---");
            InTieuDeBang();
            foreach (NhanVien nv in list)
            {
                InDongNhanVien(nv);
            }
            Console.WriteLine(Line);
        }

        private void ThemNhanVien()
        {
            Console.WriteLine("\n--- THEM NHAN VIEN MOI ---");

            Console.Write("Nhap ma nhan vien (VD: NV005): ");
            string maNV = ConsoleHelper.DocDong();
            if (maNV.Length == 0)
            {
                Console.WriteLine("Loi: Ma nhan vien khong duoc de trong!");
                return;
            }

            if (_nhanVienService.TimById(maNV) != null)
            {
                Console.WriteLine("Loi: Ma nhan vien '" + maNV + "' da ton tai tren he thong!");
                return;
            }

            Console.Write("Nhap ho va ten: ");
            string hoTen = ConsoleHelper.DocDong();
            if (!ValidationUtil.LaTenHopLe(hoTen))
            {
                Console.WriteLine("Loi: Ho ten chi duoc chua chu cai va co do dai tu 2 den 50 ky tu!");
                return;
            }

            Console.Write("Nhap so dien thoai (10 chu so, bat dau bang 0): ");
            string sdt = ConsoleHelper.DocDong();
            if (!ValidationUtil.LaSdtHopLe(sdt))
            {
                Console.WriteLine("Loi: So dien thoai khong hop le (phai co 10 so va bat dau bang 0)!");
                return;
            }

            Console.Write("Nhap email: ");
            string email = ConsoleHelper.DocDong();
            if (!ValidationUtil.LaEmailHopLe(email))
            {
                Console.WriteLine("Loi: Dinh dang email khong hop le!");
                return;
            }

            Console.Write("Nhap chuc vu (VD: Soat Ve, Dieu Phoi, An Ninh, Ke Toan): ");
            string chucVu = ConsoleHelper.DocDong();
            if (chucVu.Length == 0)
            {
                chucVu = "Nhan Vien";
            }

            try
            {
                var nvMoi = new NhanVien(maNV, "123456", hoTen, sdt, email, chucVu);
                _nhanVienService.Them(nvMoi);
                Console.WriteLine(">> Da tao thanh cong nhan vien: " + hoTen + " [" + maNV + "]");
                Console.WriteLine(">> Mat khau mac dinh ban dau la: 123456");
            }
            catch (global::System.Exception e)
            {
                Console.WriteLine("Loi: " + e.Message);
            }
        }

        private void ChinhSuaNhanVien()
        {
            Console.WriteLine("\n--- CHINH SUA THONG TIN NHAN VIEN ---");
            Console.Write("Nhap ma nhan vien can chinh sua: ");
            string maNV = ConsoleHelper.DocDong();

            NhanVien nv = _nhanVienService.TimById(maNV);
            if (nv == null)
            {
                Console.WriteLine("Loi: Khong tim thay nhan vien co ma '" + maNV + "'!");
                return;
            }

            Console.WriteLine("Thong tin hien tai cua nhan vien:");
            InTieuDeBang();
            InDongNhanVien(nv);
            Console.WriteLine(Line);
            Console.WriteLine("(Nhan Enter de giu nguyen thong tin cu)");

            // 1. Sua ho ten
            Console.Write("Ho ten moi [" + nv.HoTen + "]: ");
            string hoTenMoi = ConsoleHelper.DocDong();
            if (hoTenMoi.Length > 0)
            {
                if (!ValidationUtil.LaTenHopLe(hoTenMoi))
                {
                    Console.WriteLine("Loi: Ho ten moi khong hop le!");
                    return;
                }
                nv.HoTen = hoTenMoi;
            }

            // 2. Sua SDT
            Console.Write("SDT moi [" + nv.Sdt + "]: ");
            string sdtMoi = ConsoleHelper.DocDong();
            if (sdtMoi.Length > 0)
            {
                if (!ValidationUtil.LaSdtHopLe(sdtMoi))
                {
                    Console.WriteLine("Loi: So dien thoai moi khong hop le!");
                    return;
                }
                nv.Sdt = sdtMoi;
            }

            // 3. Sua Email
            Console.Write("Email moi [" + nv.Email + "]: ");
            string emailMoi = ConsoleHelper.DocDong();
            if (emailMoi.Length > 0)
            {
                if (!ValidationUtil.LaEmailHopLe(emailMoi))
                {
                    Console.WriteLine("Loi: Email moi khong hop le!");
                    return;
                }
                nv.Email = emailMoi;
            }

            // 4. Sua Chuc vu
            Console.Write("Chuc vu moi [" + nv.ChucVu + "]: ");
            string chucVuMoi = ConsoleHelper.DocDong();
            if (chucVuMoi.Length > 0)
            {
                nv.ChucVu = chucVuMoi;
            }

            try
            {
                _nhanVienService.Sua(nv);
                Console.WriteLine(">> Cap nhat thong tin nhan vien thanh cong!");
            }
            catch (global::System.Exception e)
            {
                Console.WriteLine("Loi: " + e.Message);
            }
        }

        private void XoaNhanVien()
        {
            Console.WriteLine("\n--- XOA NHAN VIEN ---");
            Console.Write("Nhap ma nhan vien can xoa: ");
            string maNV = ConsoleHelper.DocDong();

            NhanVien nv = _nhanVienService.TimById(maNV);
            if (nv == null)
            {
                Console.WriteLine("Loi: Khong tim thay nhan vien co ma '" + maNV + "'!");
                return;
            }

            if (nv.Quyen == QuyenHeThong.ADMIN)
            {
                Console.WriteLine("Loi: Khong the xoa tai khoan Quan Tri Vien (ADMIN)!");
                return;
            }

            if (_currentUser != null &&
                string.Equals(_currentUser.MaNguoiDung, maNV, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Loi: Khong the tu xoa tai khoan dang dang nhap hien tai!");
                return;
            }

            Console.Write("Ban co chac chan muon xoa nhan vien '" + nv.HoTen + "' [" + maNV + "]? (y/n): ");
            string xacNhan = ConsoleHelper.DocDong();
            if (string.Equals(xacNhan, "y", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    _nhanVienService.Xoa(maNV);
                    Console.WriteLine(">> Da xoa nhan vien thanh cong!");
                }
                catch (global::System.Exception e)
                {
                    Console.WriteLine("Loi: " + e.Message);
                }
            }
            else
            {
                Console.WriteLine("Da huy thao tac xoa.");
            }
        }

        private void TimKiemNhanVien()
        {
            Console.WriteLine("\n--- TIM KIEM NHAN VIEN THEO TEN ---");
            Console.Write("Nhap ten nhan vien can tim: ");
            string tuKhoa = ConsoleHelper.DocDong();

            List<NhanVien> ketQua = _nhanVienService.TimKiemTheoTen(tuKhoa);
            if (ketQua.Count == 0)
            {
                Console.WriteLine("Khong tim thay nhan vien nao phu hop voi tu khoa: " + tuKhoa);
                return;
            }

            Console.WriteLine("\nKet qua tim kiem (" + ketQua.Count + " nhan vien):");
            InTieuDeBang();
            foreach (NhanVien nv in ketQua)
            {
                InDongNhanVien(nv);
            }
            Console.WriteLine(Line);
        }

        private void DoiMatKhauCaNhan()
        {
            Console.WriteLine("\n--- DOI MAT KHAU TAI KHOAN CA NHAN ---");
            if (_currentUser == null)
            {
                Console.WriteLine("Loi: Chua co thong tin nguoi dung hien tai!");
                return;
            }

            Console.WriteLine("Tai khoan dang dang nhap: " + _currentUser.HoTen + " (" + _currentUser.MaNguoiDung + ")");
            Console.Write("Nhap mat khau cu: ");
            string passCu = ConsoleHelper.DocDong();

            Console.Write("Nhap mat khau moi (toi thieu 6 ky tu): ");
            string passMoi = ConsoleHelper.DocDong();

            if (!ValidationUtil.LaMatKhauHopLe(passMoi))
            {
                Console.WriteLine("Loi: Mat khau moi phai co do dai tu 6 ky tu tro len!");
                return;
            }

            Console.Write("Xac nhan lai mat khau moi: ");
            string xacNhanPass = ConsoleHelper.DocDong();

            if (passMoi != xacNhanPass)
            {
                Console.WriteLine("Loi: Mat khau xac nhan khong trung khop!");
                return;
            }

            bool thanhCong = _nhanVienService.DoiMatKhau(_currentUser.MaNguoiDung, passCu, passMoi);
            if (thanhCong)
            {
                Console.WriteLine(">> Chuc mung! Ban da doi mat khau thanh cong.");
            }
        }
    }
}
