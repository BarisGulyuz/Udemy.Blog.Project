using Blog.Core.DataAccess.Abstract;
using Blog.Entites.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DataAccess.Abstract
{
    public interface IUserDal : IRepository<User>
    {
    }
}
