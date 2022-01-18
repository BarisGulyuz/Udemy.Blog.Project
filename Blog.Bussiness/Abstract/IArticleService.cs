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
        Task<IDataResult<ArticleUpdateDto>> GetArticleUpdateDto(int articleId);
        Task<IDataResult<ArticleListDto>> GetAll();
        Task<IDataResult<ArticleListDto>> GetAllbyCategory(int categoryId);
        Task<IDataResult<ArticleListDto>> GetAllbyDeleted();
        Task<IDataResult<ArticleListDto>> GetAllbyDeletedandActive();
        Task<IDataResult<ArticleListDto>> GetAllbyPages(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false );
        Task<IDataResult<ArticleListDto>> Search(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false);
        Task<IDataResult<ArticleListDto>> GetAllByViewCount(bool isAscending, int? takeSize);
        Task<IResult> Add(ArticleAddDto articleAddDto, string createdByName, int userId);
        Task<IResult> Update(ArticleUpdateDto articleUpdateDto, string modifiedByName);
        Task<IResult> Delete(int articleId, string modifiedByName);
        Task<IResult> HardDelete(int articleId);
        Task<IDataResult<int>> Count();
        Task<IDataResult<int>> CountbyNoneDeleted();
    }
}
