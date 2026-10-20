using System;
using System.Collections.Generic;
using System.Linq;
using EventTicket.Interfaces;
using EventTicket.Model.Base;

namespace EventTicket.Repository
{
    public abstract class GenericRepository<T> : IQuanLy<T>, ITimKiem<T> where T : BaseEntity
    {
        public abstract void Them(T entity);
        public abstract void Sua(T entity);
        public abstract void Xoa(string id);
        public abstract T TimById(string id);
        public abstract List<T> LayTatCa();

        // Khai báo lại để gọi được trực tiếp trên repository
        // (default interface method của C# chỉ gọi được qua biến kiểu interface)
        public bool TonTai(string id)
        {
            return id != null && TimById(id) != null;
        }

        public long DemTatCa()
        {
            List<T> ds = LayTatCa();
            return ds != null ? ds.Count : 0;
        }

        // Triển khai tìm kiếm theo tiêu chí động (Predicate + LINQ)
        public List<T> TimKiemTheoTieuChi(Predicate<T> dieuKien)
        {
            if (dieuKien == null)
            {
                return LayTatCa();
            }
            return LayTatCa().Where(x => dieuKien(x)).ToList();
        }
    }

    // Tương đương Collectors.toMap(key, value, (a, b) -> a) của Java: gặp khóa trùng thì giữ phần tử đầu
    internal static class RepositoryExtensions
    {
        public class GenericRepository<K, T> where K : notnull;
        public static Dictionary<K, V> ToDictionaryKeepFirst<T, K, V>(
            this IEnumerable<T> source, Func<T, K> keySelector, Func<T, V> valueSelector)
        {
            var result = new Dictionary<K, V>();
            foreach (T item in source)
            {
                K key = keySelector(item);
                if (!result.ContainsKey(key))
                {
                    result[key] = valueSelector(item);
                }
            }
            return result;
        }
    }
}
