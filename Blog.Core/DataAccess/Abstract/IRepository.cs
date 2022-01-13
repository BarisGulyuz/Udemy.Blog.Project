using Blog.Core.Entites.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.DataAccess.Abstract
{
    public interface IRepository<T> where T : class, IEntity, new()
    {
        Task<T> GetAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes);

        Task<T> AddAsync(T entity); //Jquery-Ajax için T tipinde return
        Task<T> UpdateAsync(T entity); //Jquery-Ajax için T tipinde return
        Task DeleteAsync(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
        Task<int> CountAsync(Expression<Func<T, bool>> filter = null);
    }
}
