using System;
using System.Collections.Generic;
using EventTicket.Exception;
using EventTicket.Model.Entity;
using EventTicket.Model.Enums;
using EventTicket.Service;
using EventTicket.Util;

namespace EventTicket.UI
{
    public class SuKienMenu
    {
        private readonly SuKienService _suKienService;
        private readonly DiaDiemService _diaDiemService;

        public SuKienMenu(SuKienService suKienService, DiaDiemService diaDiemService)
        {
            _suKienService = suKienService;
            _diaDiemService = diaDiemService;
        }

        public void HienThiMenu()
        {
            while (true)
            {
                Console.WriteLine("\n==========================================");
                Console.WriteLine("           QUAN LY SU KIEN               ");
                Console.WriteLine("==========================================");
                Console.WriteLine("1. Xem danh sach tat ca su kien");
                Console.WriteLine("2. Xem su kien dang dien ra / dang ban ve");
                Console.WriteLine("3. Xem su kien sap dien ra");
                Console.WriteLine("4. Tim kiem su kien theo ten");
                Console.WriteLine("5. Them su kien moi");
                Console.WriteLine("6. Bat dau su kien (chuyen sang ONGOING)");
                Console.WriteLine("7. Huy su kien (kich hoat SuKienHuyEvent)");
                Console.WriteLine("0. Quay lai menu chinh");
                Console.WriteLine("==========================================");
                Console.Write("Nhap lua chon cua ban: ");

                string input = ConsoleHelper.DocDong();
                switch (input)
                {
                    case "1": HienThiDanhSach(_suKienService.LayTatCa(), "TAT CA SU KIEN"); break;
                    case "2": HienThiDanhSach(_suKienService.LaySuKienDangDienRa(), "SU KIEN DANG DIEN RA"); break;
                    case "3": HienThiDanhSach(_suKienService.LaySuKienSapDienRa(), "SU KIEN SAP DIEN RA"); break;
                    case "4": TimKiemTheoTen(); break;
                    case "5": ThemSuKienMoi(); break;
                    case "6": BatDauSuKien(); break;
                    case "7": HuySuKien(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Lua chon khong hop le! Vui long nhap lai.");
                        break;
                }
            }
        }

        private void HienThiDanhSach(List<SuKien> list, string tieuDe)
        {
            Console.WriteLine("\n--- " + tieuDe + " (" + list.Count + ") ---");
            if (list.Count == 0)
            {
                Console.WriteLine("Danh sach trong.");
                return;
            }
            InTieuDeBang();
            foreach (SuKien sk in list)
            {
                InDongSuKien(sk);
            }
        }

        private void TimKiemTheoTen()
        {
            Console.Write("\nNhap tu khoa ten su kien: ");
            string kw = ConsoleHelper.DocDong();
            List<SuKien> kq = _suKienService.TimKiemTheoTen(kw);
            HienThiDanhSach(kq, "KET QUA TIM KIEM CHO: '" + kw + "'");
        }

        private void ThemSuKienMoi()
        {
            Console.WriteLine("\n--- THEM SU KIEN MOI ---");

            Console.Write("Nhap ten su kien: ");
            string ten = ConsoleHelper.DocDong();
            if (ten.Length == 0)
            {
                Console.WriteLine("Loi: Ten su kien khong duoc de trong!");
                return;
            }

            Console.Write("Nhap thoi gian dien ra (dinh dang dd/MM/yyyy HH:mm, VD: 25/12/2026 19:30): ");
            string tgStr = ConsoleHelper.DocDong();
            DateTime thoiGian = default;
            try
            {
                var parsed = DateUtil.ParseNgayGio(tgStr);
                if (!parsed.HasValue)
                {
                    Console.WriteLine("Loi: Dinh dang thoi gian khong hop le!");
                    return;
                }
                thoiGian = parsed.Value;
                if (thoiGian < DateTime.Now)
                {
                    Console.WriteLine("Loi: Thoi gian dien ra su kien phai o tuong lai!");
                    return;
                }
            }
            catch (global::System.Exception)
            {
                Console.WriteLine("Loi: Dinh dang thoi gian khong dung (dd/MM/yyyy HH:mm)!");
                return;
            }

            // Chon dia diem
            List<DiaDiem> dsDiaDiem = _diaDiemService.LayTatCa();
            if (dsDiaDiem.Count == 0)
            {
                Console.WriteLine("Loi: Chua co dia diem nao trong he thong de gan cho su kien!");
                return;
            }

            Console.WriteLine("Danh sach dia diem kha dung:");
            for (int i = 0; i < dsDiaDiem.Count; i++)
            {
                DiaDiem dd = dsDiaDiem[i];
                Console.WriteLine(string.Format(" {0}. [{1}] {2} ({3})", i + 1, dd.MaDiaDiem, dd.TenDiaDiem, dd.DiaChi));
            }
            Console.Write("Chon so thu tu dia diem: ");
            string chonDdStr = ConsoleHelper.DocDong();
            DiaDiem diaDiemChon = null;
            if (int.TryParse(chonDdStr, out int chon))
            {
                int idx = chon - 1;
                if (idx >= 0 && idx < dsDiaDiem.Count)
                {
                    diaDiemChon = dsDiaDiem[idx];
                }
            }

            if (diaDiemChon == null)
            {
                Console.WriteLine("Loi: Lua chon dia diem khong hop le!");
                return;
            }

            string maSuKien = IdGenerator.SinhMaSuKien();
            try
            {
                var skMoi = new SuKien(maSuKien, ten, TrangThaiSuKien.UPCOMING, thoiGian, diaDiemChon);
                _suKienService.Them(skMoi);
            }
            catch (SuKienException e)
            {
                Console.WriteLine("Loi nghiep vu su kien: " + e.Message);
            }
            catch (global::System.Exception e)
            {
                Console.WriteLine("Loi he thong khi them su kien: " + e.Message);
            }
        }

        private void BatDauSuKien()
        {
            Console.Write("\nNhap ma su kien can bat dau (VD: SK001): ");
            string maSk = ConsoleHelper.DocDong();
            try
            {
                _suKienService.BatDauSuKien(maSk);
            }
            catch (SuKienException e)
            {
                Console.WriteLine("Loi nghiep vu su kien: " + e.Message);
            }
            catch (global::System.Exception e)
            {
                Console.WriteLine("Loi he thong: " + e.Message);
            }
        }

        private void HuySuKien()
        {
            Console.Write("\nNhap ma su kien can huy (VD: SK001): ");
            string maSk = ConsoleHelper.DocDong();
            try
            {
                // Ham nay se phat SuKienHuyEvent sang EventBus de LogListener ghi audit log
                _suKienService.HuySuKien(maSk);
            }
            catch (SuKienException e)
            {
                Console.WriteLine("Loi nghiep vu su kien: " + e.Message);
            }
            catch (global::System.Exception e)
            {
                Console.WriteLine("Loi he thong: " + e.Message);
            }
        }

        private void InTieuDeBang()
        {
            Console.WriteLine(string.Format("{0,-10} {1,-25} {2,-20} {3,-15} {4,-25}",
                "Ma SK", "Ten Su Kien", "Thoi Gian", "Trang Thai", "Dia Diem"));
            Console.WriteLine("--------------------------------------------------------------------------------------------------");
        }

        private void InDongSuKien(SuKien sk)
        {
            string tg = DateUtil.FormatNgayGio(sk.ThoiGianDienRa);
            string tt = (sk.TrangThaiSuKien != null) ? sk.TrangThaiSuKien.ToString() : "N/A";
            string dd = (sk.DiaDiem != null) ? sk.DiaDiem.TenDiaDiem : "N/A";
            Console.WriteLine(string.Format("{0,-10} {1,-25} {2,-20} {3,-15} {4,-25}",
                sk.MaSuKien, sk.TenSuKien, tg, tt, dd));
        }
    }
}
