using PagedList;

namespace CuaHangCongNghe.Extensions
{
    public static class ListExtensions
    {
        public static PagedList<T> Paginate<T>(this List<T> source, int pageSize, int currentPage = 1)
        {
            // Lấy số trang
            var pageCount = (int)Math.Ceiling((double)source.Count / pageSize);

            // Lấy danh sách sản phẩm trên trang hiện tại
            var items = source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            // Trả về danh sách sản phẩm đã được phân trang
            return new PagedList<T>(items, currentPage, pageSize);
        }
    }
}
