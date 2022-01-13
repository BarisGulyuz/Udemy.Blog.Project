using Blog.DataAccess.Abstract;
using Blog.DataAccess.Concrete.Contexts;
using Blog.DataAccess.Concrete.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitofWork
    {
        private readonly BlogContext _context;
        private EfArticleDal _efArticleDal;
        private EfCategoryDal _efCategoryDal;
        private EfCommentDal _efCommentDal;

        public UnitOfWork(BlogContext context)
        {
            _context = context;
        }

        public IArticleDal Articles => _efArticleDal ?? new EfArticleDal(_context);

        public ICategoryDal Categories => _efCategoryDal ?? new EfCategoryDal(_context);

        public ICommentDal Comments => _efCommentDal ?? new EfCommentDal(_context);

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
