using Blog.Core.DataAccess.Concrete.Repository;
using Blog.DataAccess.Abstract;
using Blog.Entites.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DataAccess.Concrete.EFCore
{
    public class EfCommentDal : GenericRepository<Comment>, ICommentDal
    {
        public EfCommentDal(DbContext context) : base(context)
        {

        }
    }
}
