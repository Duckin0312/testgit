using System;
using System.Collections.Generic;
using System.Linq;
using EventTicket.Exception;
using EventTicket.Model.Entity;
using EventTicket.Model.Enums;
using EventTicket.Service;
using EventTicket.Util;

namespace EventTicket.UI
{
    public class BaoCaoMenu
    {
        private readonly SuKienService _suKienService;
        private readonly VeService _veService;
        private readonly KhachHangService _khachHangService;
        private readonly TaiTroService _taiTroService;
        private readonly BaoCaoService _baoCaoService;

        public BaoCaoMenu(SuKienService suKienService, VeService veService,
                          KhachHangService khachHangService, TaiTroService taiTroService,
                          BaoCaoService baoCaoService)
        {
            _suKienService = suKienService;
            _veService = veService;
            _khachHangService = khachHangService;
            _taiTroService = taiTroService;
            _baoCaoService = baoCaoService;
        }

        public void HienThiMenu()
        {
            while (true)
            {
                Console.WriteLine("\n==========================================");
                Console.WriteLine("       THONG KE & BAO CAO HE THONG        ");
                Console.WriteLine("==========================================");
                Console.WriteLine("1. Bao cao tong quan tai chinh va so luong");
                Console.WriteLine("2. Thong ke ve theo loai ve (Standard / Premium / VIP)");
                Console.WriteLine("3. Tim su kien dat doanh thu cao nhat");
                Console.WriteLine("4. Thong ke doanh thu & tai tro theo tung su kien");
                Console.WriteLine("5. Xuat bao cao he thong ra file text");
                Console.WriteLine("0. Quay lai menu chinh");
                Console.WriteLine("==========================================");
                Console.Write("Nhap lua chon cua ban: ");

                string input = ConsoleHelper.DocDong();
                switch (input)
                {
                    case "1":
                        _baoCaoService.TaoBaoCao();
                        break;
                    case "2":
                        ThongKeTheoLoaiVe();
                        break;
                    case "3":
                        TimSuKienDoanhThuCaoNhat();
                        break;
                    case "4":
                        ThongKeChiTietSuKien();
                        break;
                    case "5":
                        XuatBaoCaoFile();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Lua chon khong hop le! Vui long nhap lai.");
                        break;
                }
            }
        }

        // Su dung ThongKeUtils.NhomTheo de gom nhom ve theo loai ve
        private void ThongKeTheoLoaiVe()
        {
            List<Ve> dsVeDaBan = _veService.LayTatCa()
                .Where(v => v.TrangThaiVe == TrangThaiVe.SOLD)
                .ToList();

            if (dsVeDaBan.Count == 0)
            {
                Console.WriteLine("Chua co ve nao duoc ban trong he thong.");
                return;
            }

            Dictionary<LoaiVe, List<Ve>> nhomVe = ThongKeUtils.NhomTheo(dsVeDaBan, v => v.LoaiVe ?? LoaiVe.STANDARD);

            Console.WriteLine("\n--- THONG KE THEO LOAI VE ---");
            Console.WriteLine(string.Format("{0,-15} {1,-12} {2,-20}", "Loai Ve", "So Luong", "Tong Doanh Thu"));
            Console.WriteLine("--------------------------------------------------");

            foreach (KeyValuePair<LoaiVe, List<Ve>> entry in nhomVe)
            {
                LoaiVe loai = entry.Key;
                List<Ve> veCungLoai = entry.Value;
                long soLuong = veCungLoai.Count;

                decimal tongDoanhThu = veCungLoai
                    .Where(v => v.GiaVe != null)
                    .Sum(v => v.GiaVe.Value);

                Console.WriteLine(string.Format("{0,-15} {1,-12} {2,-20}",
                    loai.MoTa(), soLuong, FormatUtil.DinhDangTien(tongDoanhThu)));
            }
        }

        private void TimSuKienDoanhThuCaoNhat()
        {
            List<SuKien> dsSuKien = _suKienService.LayTatCa();
            if (dsSuKien.Count == 0)
            {
                Console.WriteLine("Chua co su kien nao trong he thong.");
                return;
            }

            var doanhThuTungSuKien = new Dictionary<string, decimal>();
            foreach (SuKien sk in dsSuKien)
            {
                decimal dt = _veService.TinhDoanhThuTheoSuKien(sk.MaSuKien);
                doanhThuTungSuKien[sk.MaSuKien] = dt;
            }

            List<string> dsTopSuKien = ThongKeUtils.KhoaCoGiaTriLonNhat(doanhThuTungSuKien);
            if (dsTopSuKien.Count == 0)
            {
                Console.WriteLine("Chua co su kien nao phat sinh doanh thu.");
                return;
            }

            Console.WriteLine("\n--- SU KIEN CO DOANH THU CAO NHAT ---");
            foreach (string maSk in dsTopSuKien)
            {
                SuKien sk = _suKienService.TimById(maSk);
                decimal dt = doanhThuTungSuKien[maSk];
                string ten = (sk != null) ? sk.TenSuKien : maSk;
                Console.WriteLine("Ma su kien: " + maSk + " | Ten: " + ten + " | Doanh thu ve: " + FormatUtil.DinhDangTien(dt));
            }
        }

        private void ThongKeChiTietSuKien()
        {
            List<SuKien> dsSuKien = _suKienService.LayTatCa();
            if (dsSuKien.Count == 0)
            {
                Console.WriteLine("Chua co su kien nao trong he thong.");
                return;
            }

            Console.WriteLine("\n--- CHI TIET DOANH THU & TAI TRO THEO SU KIEN ---");
            Console.WriteLine(string.Format("{0,-10} {1,-25} {2,-18} {3,-18} {4,-18}",
                "Ma SK", "Ten Su Kien", "Doanh Thu Ve", "Tien Tai Tro", "Tong Thu"));
            Console.WriteLine("-----------------------------------------------------------------------------------------");

            foreach (SuKien sk in dsSuKien)
            {
                decimal dtVe = _veService.TinhDoanhThuTheoSuKien(sk.MaSuKien);
                decimal tt = _taiTroService.TinhTongTaiTroSuKien(sk.MaSuKien);
                decimal tong = dtVe + tt;

                Console.WriteLine(string.Format("{0,-10} {1,-25} {2,-18} {3,-18} {4,-18}",
                    sk.MaSuKien,
                    sk.TenSuKien,
                    FormatUtil.DinhDangTien(dtVe),
                    FormatUtil.DinhDangTien(tt),
                    FormatUtil.DinhDangTien(tong)));
            }
        }

        private void XuatBaoCaoFile()
        {
            Console.Write("\nNhap ten file muon luu (VD: bao_cao.txt, an Enter de dung mac dinh): ");
            string fileName = ConsoleHelper.DocDong();
            if (fileName.Length == 0)
            {
                fileName = "bao_cao_doanh_thu.txt";
            }
            try
            {
                _baoCaoService.XuatRaFile(fileName);
            }
            catch (DatabaseException e)
            {
                Console.WriteLine("Loi luu tru du lieu [" + e.MaLoi + "]: " + e.Message);
            }
            catch (global::System.Exception e)
            {
                Console.WriteLine("Loi he thong khi xuat file: " + e.Message);
            }
        }
    }
}
