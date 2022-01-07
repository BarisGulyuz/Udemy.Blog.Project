using Blog.Core.Utilities.Results.Abstract;
using Blog.Entites.Concrete;
using Blog.Entites.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Bussiness.Abstract
{
    public interface IArticleService
    {
        Task<IDataResult<ArticleDto>> GetArticle(int articleId);
        Task<IDataResult<ArticleListDto>> GetAll();
        Task<IDataResult<ArticleListDto>> GetAllbyCategory(int categoryId);
        Task<IDataResult<ArticleListDto>> GetAllbyDeleted();
        Task<IDataResult<ArticleListDto>> GetAllbyDeletedandActive();
        Task<IResult> Add(ArticleAddDto articleAddDto, string createdByName);
        Task<IResult> Update(ArticleUpdateDto articleUpdateDto, string modifiedByName);
        Task<IResult> Delete(int articleId, string modifiedByName);
        Task<IResult> HardDelete(int articleId);
    }
}
