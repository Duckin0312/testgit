using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EventTicket.Exception;
using EventTicket.Interfaces;
using EventTicket.Model.Entity;

namespace EventTicket.Service
{
    public class BaoCaoService : IBaoCao, IXuatDuLieu
    {
        private readonly SuKienService _suKienService;
        private readonly VeService _veService;
        private readonly KhachHangService _khachHangService;
        private readonly TaiTroService _taiTroService;

        public BaoCaoService(SuKienService suKienService, VeService veService,
                             KhachHangService khachHangService, TaiTroService taiTroService)
        {
            _suKienService = suKienService;
            _veService = veService;
            _khachHangService = khachHangService;
            _taiTroService = taiTroService;
        }

        // Tong hop chuoi noi dung bao cao
        public string TaoNoiDungBaoCaoTongQuan()
        {
            var sb = new StringBuilder();

            List<SuKien> dsSuKien = _suKienService.LayTatCa();
            List<KhachHang> dsKhachHang = _khachHangService.LayTatCa();
            List<Ve> dsVe = _veService.LayTatCa();

            decimal doanhThuVe = _veService.TinhTongDoanhThu();
            decimal tongTaiTro = _taiTroService.TinhTongTatCaTaiTro();
            decimal tongThu = doanhThuVe + tongTaiTro;

            sb.Append("====================================================\n");
            sb.Append("             BAO CAO TONG QUAN HE THONG             \n");
            sb.Append("Thoi gian tao: ").Append(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")).Append('\n');
            sb.Append("====================================================\n\n");

            sb.Append("1. THONG KE SO LUONG:\n");
            sb.Append(" - Tong so su kien: ").Append(dsSuKien.Count).Append('\n');
            sb.Append(" - Tong so khach hang: ").Append(dsKhachHang.Count).Append('\n');
            sb.Append(" - Tong so ve trong he thong: ").Append(dsVe.Count).Append("\n\n");

            sb.Append("2. THONG KE TAI CHINH:\n");
            sb.Append(" - Doanh thu tu ban ve: ").Append(doanhThuVe).Append(" VND\n");
            sb.Append(" - Tong tien tai tro nhan duoc: ").Append(tongTaiTro).Append(" VND\n");
            sb.Append(" - Tong nguon thu he thong: ").Append(tongThu).Append(" VND\n\n");

            sb.Append("3. CHI TIET DOANH THU THEO TUNG SU KIEN:\n");
            foreach (SuKien sk in dsSuKien)
            {
                decimal dtSk = _veService.TinhDoanhThuTheoSuKien(sk.MaSuKien);
                decimal ttSk = _taiTroService.TinhTongTaiTroSuKien(sk.MaSuKien);
                sb.Append($" - [{sk.MaSuKien}] {sk.TenSuKien} | Ve: {dtSk} VND | Tai tro: {ttSk} VND\n");
            }

            sb.Append("====================================================\n");
            return sb.ToString();
        }

        public void TaoBaoCao()
        {
            Console.WriteLine(TaoNoiDungBaoCaoTongQuan());
        }

        public void XuatRaFile(string duongDan)
        {
            if (string.IsNullOrWhiteSpace(duongDan))
            {
                duongDan = "bao_cao_he_thong.txt";
            }
            string noiDung = TaoNoiDungBaoCaoTongQuan();
            try
            {
                using (var writer = new StreamWriter(duongDan))
                {
                    writer.Write(noiDung);
                }
                Console.WriteLine("Xuat bao cao thanh cong ra file: " + duongDan);
            }
            catch (IOException e)
            {
                throw new DatabaseException("ERR_FILE_IO", "Loi khi ghi file bao cao xuong dia: " + e.Message);
            }
        }
    }
}
