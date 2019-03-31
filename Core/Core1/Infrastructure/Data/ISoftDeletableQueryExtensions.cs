using System.Linq;
using Core1.Model;

namespace Core1.Infrastructure.Data
{
    public static class ISoftDeletableQueryExtensions
    {
        public static IQueryable<T> NotDeleted<T>(this IQueryable<T> dbSet) where T : class, ISoftDeletable
        {
            return dbSet.Where(e => !e.DeletedOn.HasValue);
        }
    }
}