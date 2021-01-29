using AutoMapper;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.WriteApplication.Articles.DTOs;

namespace Office4U.WriteApplication.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ArticleForUpdateDto, Article>();
            CreateMap<ArticleForCreationDto, Article>();
            //CreateMap<Article, ArticleForReturnDto>();

            //CreateMap<RegisterDto, AppUser>();
        }
    }
}