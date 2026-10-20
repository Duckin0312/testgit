using System;
using System.Collections.Generic;
using System.Linq;
using EventTicket.Model.Base;

namespace EventTicket.Model.Entity
{
    public class DiaDiem : BaseEntity
    {
        private List<KhuVuc> danhSachKhuVuc;

        public string TenDiaDiem { get; set; }
        public string DiaChi { get; set; }
        public int SucChuaTong { get; private set; }

        public DiaDiem()
        {
            danhSachKhuVuc = new List<KhuVuc>();
        }

        // tạo Constructor
        public DiaDiem(string maDiaDiem, string tenDiaDiem, string diaChi)
        {
            Id = maDiaDiem;
            TenDiaDiem = tenDiaDiem;
            DiaChi = diaChi;
            SucChuaTong = 0;
            danhSachKhuVuc = new List<KhuVuc>();
        }

        public void ThemKhuVuc(KhuVuc kv)
        {
            if (kv != null)
            {
                danhSachKhuVuc.Add(kv);
                TinhTongSucChua();
            }
        }

        // dùng LINQ (tương đương stream API)
        public void TinhTongSucChua()
        {
            // duyệt qua danh sách, lấy sức chứa từng khu vực rồi cộng lại
            SucChuaTong = danhSachKhuVuc.Sum(kv => kv.SucChua);
        }

        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(TenDiaDiem))
            {
                Console.WriteLine("Lỗi: Tên địa chỉ không được để trống!");
                return false;
            }
            if (string.IsNullOrWhiteSpace(DiaChi))
            {
                Console.WriteLine("Lỗi: Địa chỉ không được để trống!");
                return false;
            }
            if (SucChuaTong < 0)
            {
                Console.WriteLine("Lỗi: Sức Chứa không hợp lệ!");
                return false;
            }
            return true;
        }

        // in thông tin
        public override string ToString()
        {
            return $"Địa Điểm [{MaDiaDiem}] {TenDiaDiem} - Địa Chỉ: {DiaChi} - Sức Chứa: {SucChuaTong} (có {danhSachKhuVuc.Count} khu vực)";
        }

        // -- GETTER & SETTER
        public string MaDiaDiem
        {
            get => Id;
            set => Id = value;
        }

        public List<KhuVuc> DanhSachKhuVuc
        {
            get => danhSachKhuVuc;
            set
            {
                danhSachKhuVuc = value;
                TinhTongSucChua();
            }
        }
    }
}
