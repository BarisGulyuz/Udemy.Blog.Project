using AutoMapper;
using Blog.Entites.DTOs;
using Blog.UI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.UI.AutoMapper.Profiles
{
    public class ViewModelsProfiles : Profile
    {
        public ViewModelsProfiles()
        {
            CreateMap<ArticleAddViewModel, ArticleAddDto>();
            CreateMap<ArticleUpdateDto, ArticleUpdateViewModel>().ReverseMap();
        }
    }
}
