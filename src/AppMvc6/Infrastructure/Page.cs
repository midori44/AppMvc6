using MR.AspNet.Paging;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvc6.Infrastructure
{
    public class Page
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public Page()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public Page(int pageNumber)
        {
            PageNumber = pageNumber;
            PageSize = 10;
        }

        public Page(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int Skip
        {
            get { return (PageNumber - 1) * PageSize; }
        }
    }

    public static class PagingExtensions
    {
        /// <summary>
        /// Pageインスタンスを引数にしてToPagedList()を呼び出すための拡張メソッド
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> queryable, Page page)
        {
            return queryable.ToPagedList(page.PageNumber, page.PageSize);
        }
        /// <summary>
        /// Pageインスタンスを引数にしてToPagedListAsync()を呼び出すための拡張メソッド
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> queryable, Page page)
        {
            return await queryable.ToPagedListAsync(page.PageNumber, page.PageSize);
        }
    }

}