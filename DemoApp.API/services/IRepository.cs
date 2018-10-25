using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DemoApp.API.Data;

namespace DemoApp.API.services
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAllWithCondition(Expression<Func<T, bool>> expression);
        Task<T> GetSingleWithCondition(Expression<Func<T, bool>> expression);
        Task<T> Find(object id);
        Task<T> InsertAsync(T entity);
        Task<T> UpdateAsync(T entity, object key);
        Task<int> DeleteAsync(T entity);

        Task<PagedList<T>> GetDataPaging(PagingParams pagingParams);

        void Save();
    }
}