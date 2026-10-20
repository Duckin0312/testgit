using System;
using System.Collections.Generic;

namespace EventTicket.Interfaces
{
    public interface ITimKiem<T>
    {
        List<T> TimKiemTheoTieuChi(Predicate<T> dieuKien);
    }
}
