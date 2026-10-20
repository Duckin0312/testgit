using System;
using System.Collections.Generic;
using System.Linq;
using EventTicket.Interfaces;
using EventTicket.Model.Entity;
using EventTicket.Model.Enums;
using EventTicket.Repository;

namespace EventTicket.Service
{
    public class NhanVienService : IQuanLy<NhanVien>
    {
        private readonly NhanVienRepository _nhanVienRepo;

        public NhanVienService()
        {
            _nhanVienRepo = new NhanVienRepository();
        }

        // Nghiệp vụ: Đăng nhập
        public NhanVien DangNhap(string maNguoiDung, string matKhau)
        {
            NhanVien nv = _nhanVienRepo.TimById(maNguoiDung);

            if (nv != null && nv.KiemTraMatKhau(matKhau))
            {
                Console.WriteLine("Đăng nhập thành công! Xin chào " + nv.HoTen);
                return nv;
            }
            Console.WriteLine(" Lỗi: Sai tài khoản hoặc mật khẩu!");
            return null;
        }

        // Nghiệp vụ: Đổi mật khẩu
        public bool DoiMatKhau(string maNguoiDung, string passCu, string passMoi)
        {
            NhanVien nv = _nhanVienRepo.TimById(maNguoiDung);
            if (nv != null)
            {
                try
                {
                    nv.DoiMatKhau(passCu, passMoi);
                    _nhanVienRepo.Sua(nv);
                    return true;
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            Console.WriteLine("Loi: Khong tim thay nhan vien!");
            return false;
        }

        // Nghiệp vụ: Tìm kiếm nhân viên theo tên
        public List<NhanVien> TimKiemTheoTen(string tenCanTim)
        {
            if (string.IsNullOrWhiteSpace(tenCanTim))
            {
                return new List<NhanVien>();
            }
            string tenLower = tenCanTim.Trim().ToLower();
            return _nhanVienRepo.LayTatCa()
                .Where(nv => nv.HoTen != null && nv.HoTen.ToLower().Contains(tenLower))
                .ToList();
        }

        public void Them(NhanVien nvMoi)
        {
            if (nvMoi == null)
            {
                throw new ArgumentException(" Lỗi: Dữ liệu nhân viên không được rỗng!");
            }

            // B1: Gán mật khẩu mặc định (123456) trước để qua được validation của NhanVien
            if (string.IsNullOrEmpty(nvMoi.MatKhau))
            {
                nvMoi.MatKhau = "123456";
            }

            // B2: Validate dữ liệu thực thể
            if (!nvMoi.Validate())
            {
                throw new ArgumentException("Lỗi: Dữ liệu nhân viên không hợp lệ!");
            }

            // B3: Kiểm tra trùng mã
            if (_nhanVienRepo.KiemTraMaDaTonTai(nvMoi.MaNguoiDung))
            {
                throw new ArgumentException(" Lỗi: Mã nhân viên '" + nvMoi.MaNguoiDung + "' đã tồn tại!");
            }

            // B4: Đồng bộ ID sang Id của BaseEntity nếu chưa có
            if (nvMoi.Id == null)
            {
                nvMoi.Id = nvMoi.MaNguoiDung;
            }

            _nhanVienRepo.Them(nvMoi);
            Console.WriteLine("Đã tạo thành công nhân viên mới! Mật khẩu mặc định là: 123456");
        }

        public void Sua(NhanVien nvCanSua)
        {
            if (nvCanSua == null || !nvCanSua.Validate())
            {
                throw new ArgumentException("Lỗi: Dữ liệu cập nhật không hợp lệ!");
            }

            if (!_nhanVienRepo.KiemTraMaDaTonTai(nvCanSua.MaNguoiDung))
            {
                throw new ArgumentException(" Lỗi: Không tìm thấy nhân viên để cập nhật!");
            }

            _nhanVienRepo.Sua(nvCanSua);
            Console.WriteLine("Cap nhat nhan vien thanh cong!");
        }

        public void Xoa(string id)
        {
            NhanVien nvCanXoa = _nhanVienRepo.TimById(id);

            if (nvCanXoa == null)
            {
                throw new ArgumentException(" Lỗi: Không tìm thấy nhân viên cần xóa!");
            }

            // Chặn không cho xóa Quản trị viên (ADMIN)
            if (nvCanXoa.Quyen == QuyenHeThong.ADMIN)
            {
                throw new ArgumentException(" Lỗi: Không được phép xóa tài khoản ADMIN (" + nvCanXoa.HoTen + ")!");
            }

            _nhanVienRepo.Xoa(id);
            Console.WriteLine("Da xoa nhan vien thanh cong!");
        }

        public NhanVien TimById(string id)
        {
            return _nhanVienRepo.TimById(id);
        }

        public List<NhanVien> LayTatCa()
        {
            return _nhanVienRepo.LayTatCa();
        }
    }
}
