using System;
using System.Collections.Generic;
using System.Linq;
using EventTicket.Model.Base;

namespace EventTicket.Interfaces
{
    // Repository generic:
    // Dung duoc cho moi thuc the ke thua BaseEntity (KhachHang, Ve, SuKien, DiaDiem...).
    public interface IRepository<T> where T : BaseEntity
    {
        // Them thuc the. Tra ve false neu null, trung id hoac validate() that bai.
        bool TempEntity(T entity);

        // Tra ve null neu khong tim thay (thay cho Optional.empty() cua Java)
        T TimTheoId(string id);

        List<T> LayTatCa();

        // Loc theo dieu kien (lambda / method group)
        List<T> Loc(Predicate<T> dieuKien);

        bool CapNhat(T entity);

        bool Xoa(string id);

        long Dem(Predicate<T> dieuKien)
        {
            return LayTatCa().Count(x => dieuKien(x));
        }

        // Them nhieu thuc the, tra ve so thuc the them thanh cong.
        int ThemTatCa(IEnumerable<T> danhSachEntity)
        {
            int soLuong = 0;
            foreach (T entity in danhSachEntity)
            {
                if (TempEntity(entity)) soLuong++;
            }
            return soLuong;
        }
    }
}
