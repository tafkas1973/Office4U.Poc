using AutoMapper;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.WriteApplication.Articles.DTOs;

namespace Office4U.WriteApplication.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Article, ArticleForReturnDto>();
            CreateMap<ArticleForUpdateDto, Article>();
            CreateMap<ArticleForCreationDto, Article>();
        }
    }
}