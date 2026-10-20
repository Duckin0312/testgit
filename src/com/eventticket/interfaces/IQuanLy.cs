using System.Collections.Generic;
using EventTicket.Model.Base;

namespace EventTicket.Interfaces
{
    public interface IQuanLy<T> where T : BaseEntity
    {
        void Them(T entity);
        void Sua(T entity);
        void Xoa(string id);
        T TimById(string id);
        List<T> LayTatCa();

        // Default interface method (C# 8+) tien ich de kiem tra ton tai
        bool TonTai(string id)
        {
            return id != null && TimById(id) != null;
        }

        // Default interface method dem tong so phan tu
        long DemTatCa()
        {
            List<T> ds = LayTatCa();
            return ds != null ? ds.Count : 0;
        }
    }
}
