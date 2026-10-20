using System;
using System.Collections.Generic;
using System.Linq;

namespace EventTicket.Util
{
    // Các hàm tiện ích generic dùng chung cho thống kê
    public static class ThongKeUtils
    {
        // Nhóm một tập phần tử theo khóa
        public static Dictionary<K, List<T>> NhomTheo<T, K>(IEnumerable<T> danhSach, Func<T, K> khoa) where K : notnull
        {
            return danhSach
                .GroupBy(khoa)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        // Đếm số phần tử thỏa điều kiện
        public static long Dem<T>(IEnumerable<T> danhSach, Func<T, bool> dieuKien)
        {
            return danhSach.LongCount(dieuKien);
        }

        // Cộng dồn một trường decimal của các phần tử (ví dụ tổng giá vé)
        public static decimal Tong<T>(IEnumerable<T> danhSach, Func<T, decimal> laySoTien)
        {
            return danhSach.Select(laySoTien).Aggregate(0m, (a, b) => a + b);
        }

        // Các khóa có giá trị lớn nhất (trả về nhiều khóa nếu đồng hạng)
        public static List<K> KhoaCoGiaTriLonNhat<K, V>(IDictionary<K, V> map) where V : IComparable<V>
        {
            if (map.Count == 0) return new List<K>();

            var comparer = Comparer<V>.Default;
            V max = map.Values.Max(comparer)!;

            return map
                .Where(e => comparer.Compare(e.Value, max) == 0)
                .Select(e => e.Key)
                .ToList();
        }

        // Alias tien ich TimKhoaGiaTriLonNhat
        public static List<K> TimKhoaGiaTriLonNhat<K, V>(IDictionary<K, V> map) where V : IComparable<V>
        {
            return KhoaCoGiaTriLonNhat(map);
        }
    }
}
