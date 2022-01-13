using Blog.Core.Utilities.Results.Abstract;
using ProgrammersBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Bussiness.Abstract
{
    public interface ICommentService
    {
        Task<IDataResult<int>> Count();
        Task<IDataResult<int>> CountbyNoneDeleted();
        Task<IDataResult<CommentDto>> GetAsync(int commentId);
        Task<IDataResult<CommentUpdateDto>> GetCommentUpdateDtoAsync(int commentId);
        Task<IDataResult<CommentListDto>> GetAllAsync();
        Task<IDataResult<CommentListDto>> GetAllByDeletedAsync();
        Task<IDataResult<CommentListDto>> GetAllByNonDeletedAsync();
        Task<IDataResult<CommentListDto>> GetAllByNonDeletedAndActiveAsync();
        Task<IDataResult<CommentDto>> AddAsync(CommentAddDto commentAddDto);
        Task<IDataResult<CommentDto>> ApproveAsync(int commentId, string modifieByName);
        Task<IDataResult<CommentDto>> UpdateAsync(CommentUpdateDto commentUpdateDto, string modifiedByName);
        Task<IDataResult<CommentDto>> DeleteAsync(int commentId, string modifiedByName);
        Task<IResult> HardDeleteAsync(int commentId);
    }
}
