using Blog.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DataAccess.UnitOfWork
{
    public interface IUnitofWork: IAsyncDisposable
    {
        IArticleDal Articles { get; }
        ICategoryDal Categories { get; }
        ICommentDal Comments { get; }
        IRoleDal Roles { get; }
        IUserDal Users { get; }
        Task<int> SaveAsync();
    }
}
