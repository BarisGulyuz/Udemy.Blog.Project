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
    public interface ICategoryService 
    {
     
        Task<IDataResult<CategoryDto>> GetCategory(int categoryId);
        /// <summary>
        /// Id parametresine ait kategorinin 'categoryUpdateDto' temsilini geriye döner.
        /// </summary>
        /// <param name="categoryId">0'a eşit olamaz (categoryId>0)</param>
        /// <returns>Task olarak DataResult tipinde geriye döner</returns>
        Task<IDataResult<CategoryUpdateDto>> GetCategoryUpdateDto(int categoryId);
        Task<IDataResult<CategoryListDto>> GetAll();
        Task<IDataResult<CategoryListDto>> GetAllbyDeleted();
        Task<IDataResult<CategoryListDto>> GetAllbyDeletedandActive();
        Task<IDataResult<CategoryDto>> Add(CategoryAddDto categoryAddDto, string createdByName);
        Task<IDataResult<CategoryDto>> Update(CategoryUpdateDto categoryUpdateDto, string modifiedByName);
        Task<IResult> Delete(int categoryId, string modifiedByName);
        Task<IResult> HardDelete(int categoryId);
        Task<IDataResult<int>> Count();
        Task<IDataResult<int>> CountbyNoneDeleted();
    }
}
