using AutoMapper;
using Blog.Bussiness.Abstract;
using Blog.Bussiness.Constants;
using Blog.Core.Utilities.Results;
using Blog.Core.Utilities.Results.Abstract;
using Blog.Core.Utilities.Results.Concrete;
using Blog.DataAccess.UnitOfWork;
using Blog.Entites.Concrete;
using Blog.Entites.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Bussiness.Concrete
{
    public class ArticleManager : IArticleService
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public ArticleManager(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<IResult> Add(ArticleAddDto articleAddDto, string createdByName, int userId)
        {
            var article = _mapper.Map<Article>(articleAddDto);
            article.CreatedByName = createdByName;
            article.ModifiedByName = createdByName;
            article.UserId = userId;
            await _unitofWork.Articles.AddAsync(article);
            await _unitofWork.SaveAsync();
            return new Result(Core.Utilities.Results.ResultStatus.Success, Messages.GeneralAddESuccess);
        }

        public async Task<IDataResult<int>> Count()
        {
            var count = await _unitofWork.Articles.CountAsync();
            if (count > -1)
                return new DataResult<int>(Core.Utilities.Results.ResultStatus.Success, count);
            return new DataResult<int>(Core.Utilities.Results.ResultStatus.Error, -1);
        }

        public async Task<IDataResult<int>> CountbyNoneDeleted()
        {
            var count = await _unitofWork.Articles.CountAsync(x => !x.IsDeleted);
            if (count > -1)
                return new DataResult<int>(Core.Utilities.Results.ResultStatus.Success, count);
            return new DataResult<int>(Core.Utilities.Results.ResultStatus.Error, -1);
        }

        public async Task<IResult> Delete(int articleId, string modifiedByName)
        {
            var result = await _unitofWork.Articles.AnyAsync(x => x.Id == articleId);
            if (result)
            {
                var article = await _unitofWork.Articles.GetAsync(x => x.Id == articleId);
                article.IsDeleted = true;
                article.IsActive = false;
                article.ModifiedByName = modifiedByName;
                article.ModifiedDate = DateTime.Now;
                await _unitofWork.Articles.UpdateAsync(article);
                await _unitofWork.SaveAsync();
                return new Result(Core.Utilities.Results.ResultStatus.Success, Messages.GeneralUpdateSuccess);
            }
            return new Result(Core.Utilities.Results.ResultStatus.Success, Messages.ArticleNotFound);
        }

        public async Task<IDataResult<ArticleListDto>> GetAll()
        {
            var articles = await _unitofWork.Articles.GetAllAsync(null, x => x.Category, x => x.User);
            if (articles.Count() > -1)
            {
                return new DataResult<ArticleListDto>(Core.Utilities.Results.ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus = Core.Utilities.Results.ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(Core.Utilities.Results.ResultStatus.Error, Messages.GeneralListError, null);

        }

        public async Task<IDataResult<ArticleListDto>> GetAllbyCategory(int categoryId)
        {
            var categoryResult = await _unitofWork.Categories.AnyAsync(x => x.Id == categoryId);
            if (categoryResult)
            {

                var articles = await _unitofWork.Articles.GetAllAsync(x => x.CategoryId == categoryId && x.IsActive, x => x.Category, x => x.User);
                if (articles.Count() > -1)
                {
                    return new DataResult<ArticleListDto>(Core.Utilities.Results.ResultStatus.Success, new ArticleListDto
                    {
                        Articles = articles,
                        ResultStatus = Core.Utilities.Results.ResultStatus.Success
                    });
                }
                return new DataResult<ArticleListDto>(Core.Utilities.Results.ResultStatus.Error, Messages.GeneralListError, null);

            }
            return new DataResult<ArticleListDto>(Core.Utilities.Results.ResultStatus.Error, Messages.CategoryNotFound, null);
        }

        public async Task<IDataResult<ArticleListDto>> GetAllbyDeleted()
        {
            var articles = await _unitofWork.Articles.GetAllAsync(x => !x.IsDeleted, x => x.Category, x => x.User);
            if (articles.Count() > -1)
            {
                return new DataResult<ArticleListDto>(Core.Utilities.Results.ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus = Core.Utilities.Results.ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(Core.Utilities.Results.ResultStatus.Error, Messages.GeneralListError, null);

        }

        public async Task<IDataResult<ArticleListDto>> GetAllbyDeletedandActive()
        {
            var articles = await _unitofWork.Articles.GetAllAsync(x => !x.IsDeleted && x.IsActive, x => x.Category, x => x.User);
            if (articles.Count() > -1)
            {
                return new DataResult<ArticleListDto>(Core.Utilities.Results.ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus = Core.Utilities.Results.ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(Core.Utilities.Results.ResultStatus.Error, Messages.GeneralListError, null);

        }

        public async Task<IDataResult<ArticleListDto>> GetAllbyPages(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            //*Skip: (mevcutsayfa-1) * sayfa büyüklüğü
            pageSize = pageSize > 20 ? pageSize = 20 : pageSize;
            var articles = categoryId == null ? await _unitofWork.Articles.GetAllAsync(x => x.IsActive, x => x.Category, x => x.User)
                : await _unitofWork.Articles.GetAllAsync(x => x.IsActive & x.CategoryId == categoryId, x => x.Category, x => x.User);

            var sortedArticles = isAscending ? articles.OrderBy(x => x.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList() //skip methodu kadarını alma pageSize kadar olanı al
                : articles.OrderByDescending(x => x.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            return new DataResult<ArticleListDto>(Core.Utilities.Results.ResultStatus.Success, new ArticleListDto
            {
                Articles = sortedArticles,
                CategoryId = categoryId == null ? null : categoryId.Value,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = articles.Count,
                isAscending = isAscending
            });

        }

        public async Task<IDataResult<ArticleListDto>> GetAllByViewCount(bool isAscending, int? takeSize)
        {
            var articles = await _unitofWork.Articles.GetAllAsync(x => x.IsActive);
            var sortedArticles = isAscending ? articles.OrderBy(x => x.ViewsCount) : articles.OrderByDescending(x => x.ViewsCount);
            if (sortedArticles.Count() > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = takeSize == null ? sortedArticles.ToList() : sortedArticles.Take(takeSize.Value).ToList(),
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, null);
        }

        public async Task<IDataResult<ArticleDto>> GetArticle(int articleId)
        {
            var article = await _unitofWork.Articles.GetAsync(x => x.Id == articleId, x => x.Category, x => x.User, x => x.Comments);
            if (article != null)
            {
                return new DataResult<ArticleDto>(Core.Utilities.Results.ResultStatus.Success, new ArticleDto
                {
                    Article = article,
                    ResultStatus = Core.Utilities.Results.ResultStatus.Success
                });
            }
            return new DataResult<ArticleDto>(Core.Utilities.Results.ResultStatus.Error, Messages.GeneralGetError, null);
        }

        public async Task<IDataResult<ArticleUpdateDto>> GetArticleUpdateDto(int articleId)
        {
            var result = await _unitofWork.Articles.AnyAsync(x => x.Id == articleId);
            if (result)
            {
                var article = await _unitofWork.Articles.GetAsync(x => x.Id == articleId, x => x.Category);
                var articleUpdateDto = _mapper.Map<ArticleUpdateDto>(article);
                return new DataResult<ArticleUpdateDto>(Core.Utilities.Results.ResultStatus.Success, articleUpdateDto);
            }
            return new DataResult<ArticleUpdateDto>(Core.Utilities.Results.ResultStatus.Error, Messages.GeneralGetError, null);
        }

        public async Task<IResult> HardDelete(int articleId)
        {
            var result = await _unitofWork.Articles.AnyAsync(x => x.Id == articleId);
            if (result)
            {
                var article = await _unitofWork.Articles.GetAsync(x => x.Id == articleId);
                await _unitofWork.Articles.DeleteAsync(article);
                await _unitofWork.SaveAsync();
                return new Result(Core.Utilities.Results.ResultStatus.Success, Messages.GeneralDeleteSuccess);
            }
            return new Result(Core.Utilities.Results.ResultStatus.Success, Messages.ArticleNotFound);
        }

        public async Task<IDataResult<ArticleListDto>> Search(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            //DEFAULT PAGING 
            pageSize = pageSize > 20 ? pageSize = 20 : pageSize;
            if (string.IsNullOrWhiteSpace(keyword)) //boşluk kontrolü
            {
                var articles = await _unitofWork.Articles.GetAllAsync(x => x.IsActive, x => x.Category, x => x.User);

                var sortedArticles = isAscending ? articles.OrderBy(x => x.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                    : articles.OrderByDescending(x => x.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                return new DataResult<ArticleListDto>(Core.Utilities.Results.ResultStatus.Success, new ArticleListDto
                {
                    Articles = sortedArticles,
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    TotalCount = articles.Count,
                    isAscending = isAscending
                });
            }
            //PAGING WITH SEARCH KEYWORDS
            var searchedArticles = await _unitofWork.Articles.SearchAsync(new List<System.Linq.Expressions.Expression<Func<Article, bool>>>
            {
                x=> x.Title.Contains(keyword),
                x=> x.Category.Title.Contains(keyword),
                x=> x.SeoDescription.Contains(keyword),
                x=> x.Content.Contains(keyword),
                x=> x.SeoTags.Contains(keyword)
            }, x => x.Category, x => x.User);

            var sortedAndSearchedArticles = isAscending ? searchedArticles.OrderBy(x => x.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                    : searchedArticles.OrderByDescending(x => x.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            return new DataResult<ArticleListDto>(Core.Utilities.Results.ResultStatus.Success, new ArticleListDto
            {
                Articles = searchedArticles,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = searchedArticles.Count,
                isAscending = isAscending
            });

        }

        public async Task<IResult> Update(ArticleUpdateDto articleUpdateDto, string modifiedByName)
        {
            var oldArticle = await _unitofWork.Articles.GetAsync(a => a.Id == articleUpdateDto.Id);
            var article = _mapper.Map<ArticleUpdateDto, Article>(articleUpdateDto, oldArticle);
            article.ModifiedByName = modifiedByName;
            await _unitofWork.Articles.UpdateAsync(article);
            await _unitofWork.SaveAsync();
            return new Result(ResultStatus.Success, "");
        }
    }
}
