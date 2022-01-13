using AutoMapper;
using Blog.Bussiness.Abstract;
using Blog.Bussiness.Constants;
using Blog.Core.Utilities.Results;
using Blog.Core.Utilities.Results.Abstract;
using Blog.Core.Utilities.Results.Concrete;
using Blog.DataAccess.UnitOfWork;
using Blog.Entites.Concrete;
using ProgrammersBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Bussiness.Concrete
{

    public class CommentManager : ICommentService
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public CommentManager(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<IDataResult<int>> Count()
        {
            var count = await _unitofWork.Comments.CountAsync();
            if (count > -1)
                return new DataResult<int>(Core.Utilities.Results.ResultStatus.Success, count);
            return new DataResult<int>(Core.Utilities.Results.ResultStatus.Error, -1);
        }

        public async Task<IDataResult<int>> CountbyNoneDeleted()
        {
            var count = await _unitofWork.Comments.CountAsync(x => !x.IsDeleted);
            if (count > -1)
                return new DataResult<int>(Core.Utilities.Results.ResultStatus.Success, count);
            return new DataResult<int>(Core.Utilities.Results.ResultStatus.Error, -1);
        }

        public async Task<IDataResult<CommentDto>> GetAsync(int commentId)
        {
            var comment = await _unitofWork.Comments.GetAsync(c => c.Id == commentId);
            if (comment != null)
            {
                return new DataResult<CommentDto>(ResultStatus.Success, new CommentDto
                {
                    Comment = comment,
                });
            }
            return new DataResult<CommentDto>(ResultStatus.Error, Messages.Comment.NotFound(isPlural: false), new CommentDto
            {
                Comment = null,
            });
        }

        public async Task<IDataResult<CommentUpdateDto>> GetCommentUpdateDtoAsync(int commentId)
        {
            var result = await _unitofWork.Comments.AnyAsync(c => c.Id == commentId);
            if (result)
            {
                var comment = await _unitofWork.Comments.GetAsync(c => c.Id == commentId);
                var commentUpdateDto = _mapper.Map<CommentUpdateDto>(comment);
                return new DataResult<CommentUpdateDto>(ResultStatus.Success, commentUpdateDto);
            }
            else
            {
                return new DataResult<CommentUpdateDto>(ResultStatus.Error, Messages.Comment.NotFound(isPlural: false), null);
            }
        }

        public async Task<IDataResult<CommentListDto>> GetAllAsync()
        {
            var comments = await _unitofWork.Comments.GetAllAsync(null, x => x.Article);
            if (comments.Count > -1)
            {
                return new DataResult<CommentListDto>(ResultStatus.Success, new CommentListDto
                {
                    Comments = comments,
                });
            }
            return new DataResult<CommentListDto>(ResultStatus.Error, Messages.Comment.NotFound(isPlural: true), new CommentListDto
            {
                Comments = null,
            });
        }

        public async Task<IDataResult<CommentListDto>> GetAllByDeletedAsync()
        {
            var comments = await _unitofWork.Comments.GetAllAsync(c => c.IsDeleted, x => x.Article);
            if (comments.Count > -1)
            {
                return new DataResult<CommentListDto>(ResultStatus.Success, new CommentListDto
                {
                    Comments = comments,
                });
            }
            return new DataResult<CommentListDto>(ResultStatus.Error, Messages.Comment.NotFound(isPlural: true), new CommentListDto
            {
                Comments = null,
            });
        }

        public async Task<IDataResult<CommentListDto>> GetAllByNonDeletedAsync()
        {
            var comments = await _unitofWork.Comments.GetAllAsync(c => !c.IsDeleted, x => x.Article);
            if (comments.Count > -1)
            {
                return new DataResult<CommentListDto>(ResultStatus.Success, new CommentListDto
                {
                    Comments = comments,
                });
            }
            return new DataResult<CommentListDto>(ResultStatus.Error, Messages.Comment.NotFound(isPlural: true), new CommentListDto
            {
                Comments = null,
            });
        }

        public async Task<IDataResult<CommentListDto>> GetAllByNonDeletedAndActiveAsync()
        {
            var comments = await _unitofWork.Comments.GetAllAsync(c => !c.IsDeleted && c.IsActive, x => x.Article);
            if (comments.Count > -1)
            {
                return new DataResult<CommentListDto>(ResultStatus.Success, new CommentListDto
                {
                    Comments = comments,
                });
            }
            return new DataResult<CommentListDto>(ResultStatus.Error, Messages.Comment.NotFound(isPlural: true), new CommentListDto
            {
                Comments = null,
            });
        }

        public async Task<IDataResult<CommentDto>> AddAsync(CommentAddDto commentAddDto)
        {
            var comment = _mapper.Map<Comment>(commentAddDto);
            var addedComment = await _unitofWork.Comments.AddAsync(comment);
            await _unitofWork.SaveAsync();
            return new DataResult<CommentDto>(ResultStatus.Success, Messages.Comment.Add(commentAddDto.CreatedByName), new CommentDto
            {
                Comment = addedComment,
            });
        }

        public async Task<IDataResult<CommentDto>> UpdateAsync(CommentUpdateDto commentUpdateDto, string modifiedByName)
        {
            var oldComment = await _unitofWork.Comments.GetAsync(c => c.Id == commentUpdateDto.Id, x => x.Article);
            var comment = _mapper.Map<CommentUpdateDto, Comment>(commentUpdateDto, oldComment);
            comment.ModifiedByName = modifiedByName;
            var updatedComment = await _unitofWork.Comments.UpdateAsync(comment);
            updatedComment.Article = await _unitofWork.Articles.GetAsync(x => x.Id == updatedComment.Article.Id);
            await _unitofWork.SaveAsync();
            return new DataResult<CommentDto>(ResultStatus.Success, Messages.Comment.Update(comment.CreatedByName), new CommentDto
            {
                Comment = updatedComment,
            });
        }

        public async Task<IDataResult<CommentDto>> DeleteAsync(int commentId, string modifiedByName)
        {
            var comment = await _unitofWork.Comments.GetAsync(c => c.Id == commentId);
            if (comment != null)
            {
                comment.IsDeleted = true;
                comment.ModifiedByName = modifiedByName;
                comment.ModifiedDate = DateTime.Now;
                var deletedComment = await _unitofWork.Comments.UpdateAsync(comment);
                await _unitofWork.SaveAsync();
                return new DataResult<CommentDto>(ResultStatus.Success, Messages.Comment.Delete(deletedComment.CreatedByName), new CommentDto
                {
                    Comment = deletedComment,
                });
            }
            return new DataResult<CommentDto>(ResultStatus.Error, Messages.Comment.NotFound(isPlural: false), new CommentDto
            {
                Comment = null,
            });
        }

        public async Task<IResult> HardDeleteAsync(int commentId)
        {
            var comment = await _unitofWork.Comments.GetAsync(c => c.Id == commentId);
            if (comment != null)
            {
                await _unitofWork.Comments.DeleteAsync(comment);
                await _unitofWork.SaveAsync();
                return new Result(ResultStatus.Success, Messages.Comment.HardDelete(comment.CreatedByName));
            }
            return new Result(ResultStatus.Error, Messages.Comment.NotFound(isPlural: false));
        }

        public async Task<IDataResult<CommentDto>> ApproveAsync(int commentId, string modifiedByName)
        {
            var comment = await _unitofWork.Comments.GetAsync(x => x.Id == commentId, x=> x.Article);
            if(comment != null)
            {
                comment.IsActive = true;
                comment.ModifiedByName = modifiedByName;
                comment.ModifiedDate = DateTime.Now;
                var updatedComment = await _unitofWork.Comments.UpdateAsync(comment);
                await _unitofWork.SaveAsync();
                return new DataResult<CommentDto>(ResultStatus.Success, Messages.Comment.Approve(commentId), new CommentDto
                {
                    Comment = updatedComment
                });
            }
            return new DataResult<CommentDto>(ResultStatus.Error, null);
        }
    }
}
