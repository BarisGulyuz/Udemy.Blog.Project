using AutoMapper;
using Blog.Entites.Concrete;
using ProgrammersBlog.Entities;
using System;


namespace Blog.Bussiness.AutoMapper.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<CommentAddDto, Comment>()
               .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(x => DateTime.Now))
               .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(x => DateTime.Now))
               .ForMember(dest => dest.ModifiedByName, opt => opt.MapFrom(x => x.CreatedByName))
               .ForMember(dest => dest.IsActive, opt => opt.MapFrom(x => false));
            CreateMap<CommentUpdateDto, Comment>()
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<Comment, CommentUpdateDto>();
        }
    }
}
