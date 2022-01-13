using AutoMapper;
using Blog.Bussiness.Abstract;
using Blog.Bussiness.Constants;
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
    public class CategoryManager : ICategoryService
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public CategoryManager(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<IDataResult<CategoryDto>> Add(CategoryAddDto categoryAddDto, string createdByName)
        {
            var category = _mapper.Map<Category>(categoryAddDto);
            category.CreatedByName = createdByName;
            category.ModifiedByName = createdByName;
            var addedCategory = await _unitofWork.Categories.AddAsync(category);
            await _unitofWork.SaveAsync();
            return new DataResult<CategoryDto>(Core.Utilities.Results.ResultStatus.Success, Messages.GeneralAddESuccess, new CategoryDto
            {
                Category = addedCategory,
                ResultStatus = Core.Utilities.Results.ResultStatus.Success,
                Messages = Messages.GeneralAddESuccess
            });
        }

        public async Task<IDataResult<int>> Count()
        {
            var count = await _unitofWork.Categories.CountAsync();
            if (count > -1)
                return new DataResult<int>(Core.Utilities.Results.ResultStatus.Success, count);
            return new DataResult<int>(Core.Utilities.Results.ResultStatus.Error, -1);
        }

        public async Task<IDataResult<int>> CountbyNoneDeleted()
        {
            var count = await _unitofWork.Categories.CountAsync(x=> !x.IsDeleted);
            if (count > -1)
                return new DataResult<int>(Core.Utilities.Results.ResultStatus.Success, count);
            return new DataResult<int>(Core.Utilities.Results.ResultStatus.Error, -1);
        }

        public async Task<IResult> Delete(int categoryId, string modifiedByName)
        {
            var category = await _unitofWork.Categories.GetAsync(x => x.Id == categoryId);
            if (category != null)
            {
                category.IsDeleted = true;
                category.IsActive = false;
                category.ModifiedByName = modifiedByName;
                category.ModifiedDate = DateTime.Now;
                await _unitofWork.Categories.UpdateAsync(category).ContinueWith(async s => await _unitofWork.SaveAsync());
                return new Result(Core.Utilities.Results.ResultStatus.Success, Messages.GeneralDeleteSuccess);
            }
            return new Result(Core.Utilities.Results.ResultStatus.Error, Messages.GeneralDeleteError);
        }

        public async Task<IDataResult<CategoryListDto>> GetAll()
        {
            var categories = await _unitofWork.Categories.GetAllAsync(null);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(Core.Utilities.Results.ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = Core.Utilities.Results.ResultStatus.Success
                });
            }
            return new DataResult<CategoryListDto>(Core.Utilities.Results.ResultStatus.Error, Messages.GeneralListError, new CategoryListDto
            {
                Categories = null,
                ResultStatus = Core.Utilities.Results.ResultStatus.Error,
                Messages = Messages.GeneralListError
            }); ;
        }

        public async Task<IDataResult<CategoryListDto>> GetAllbyDeleted()
        {
            var categories = await _unitofWork.Categories.GetAllAsync(x => !x.IsDeleted);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(Core.Utilities.Results.ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = Core.Utilities.Results.ResultStatus.Success
                });
            }
            return new DataResult<CategoryListDto>(Core.Utilities.Results.ResultStatus.Error, Messages.GeneralListError, new CategoryListDto
            {
                Categories = null,
                ResultStatus = Core.Utilities.Results.ResultStatus.Error,
                Messages = Messages.GeneralListError
            }); ;
        }

        public async Task<IDataResult<CategoryListDto>> GetAllbyDeletedandActive()
        {
            var categories = await _unitofWork.Categories.GetAllAsync(x => !x.IsDeleted && x.IsActive);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(Core.Utilities.Results.ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = Core.Utilities.Results.ResultStatus.Success
                });
            }
            return new DataResult<CategoryListDto>(Core.Utilities.Results.ResultStatus.Error, Messages.GeneralListError, new CategoryListDto
            {
                Categories = null,
                ResultStatus = Core.Utilities.Results.ResultStatus.Error,
                Messages = Messages.GeneralListError
            }); ;
        }

        public async Task<IDataResult<CategoryDto>> GetCategory(int categoryId)
        {
            var category = await _unitofWork.Categories.GetAsync(x => x.Id == categoryId, x => x.Articles);
            if (category != null)
            {
                return new DataResult<CategoryDto>(Core.Utilities.Results.ResultStatus.Success, new CategoryDto
                {
                    Category = category,
                    ResultStatus = Core.Utilities.Results.ResultStatus.Success,
                    Messages = Messages.GeneralGetSuccess

                }); ;
            }
            return new DataResult<CategoryDto>(Core.Utilities.Results.ResultStatus.Error, Messages.GeneralGetError, null);

        }
        /// <summary>
        /// Id parametresine ait kategorinin 'categoryUpdateDto' temsilini geriye döner.
        /// </summary>
        /// <param name="categoryId">0'a eşit olamaz (categoryId>0)</param>
        /// <returns>Task olarak DataResult tipinde geriye döner></returns>
        public async Task<IDataResult<CategoryUpdateDto>> GetCategoryUpdateDto(int categoryId)
        {
            var result = await _unitofWork.Categories.AnyAsync(x => x.Id == categoryId);
            if (result)
            {
                var category = await _unitofWork.Categories.GetAsync(x => x.Id == categoryId, x => x.Articles);
                var categoryUpdateDto = _mapper.Map<CategoryUpdateDto>(category);
                return new DataResult<CategoryUpdateDto>(Core.Utilities.Results.ResultStatus.Success, categoryUpdateDto);
            }
            return new DataResult<CategoryUpdateDto>(Core.Utilities.Results.ResultStatus.Error, Messages.GeneralGetError, null);
        }

        public async Task<IResult> HardDelete(int categoryId)
        {
            var category = await _unitofWork.Categories.GetAsync(x => x.Id == categoryId);
            if (category != null)
            {
                await _unitofWork.Categories.DeleteAsync(category).ContinueWith(async s => await _unitofWork.SaveAsync());
                return new Result(Core.Utilities.Results.ResultStatus.Success, Messages.GeneralDeleteSuccess);
            }
            return new Result(Core.Utilities.Results.ResultStatus.Error, Messages.GeneralDeleteError);
        }

        public async Task<IDataResult<CategoryDto>> Update(CategoryUpdateDto categoryUpdateDto, string modifiedByName)
        {
            var oldCategory = await _unitofWork.Categories.GetAsync(x => x.Id == categoryUpdateDto.Id);
            if (oldCategory != null)
            {
                var category = _mapper.Map<CategoryUpdateDto, Category>(categoryUpdateDto, oldCategory);
                category.ModifiedByName = modifiedByName;
                var uptatedCategory = await _unitofWork.Categories.UpdateAsync(category);
                await _unitofWork.SaveAsync();
                return new DataResult<CategoryDto>(Core.Utilities.Results.ResultStatus.Success, Messages.GeneralUpdateSuccess, new CategoryDto
                {
                    Category = uptatedCategory,
                    ResultStatus = Core.Utilities.Results.ResultStatus.Success,
                    Messages = Messages.GeneralUpdateSuccess

                });
            }
            return new DataResult<CategoryDto>(Core.Utilities.Results.ResultStatus.Error, Messages.GeneralUpdateSuccess, null);
        }
    }
}
