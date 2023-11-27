using AutoMapper;
using CatalogService.Domain.Dtos;
using CatalogService.Domain.Entities;

namespace CatalogService.Application.Mappings
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateItemInputDto, Item>();
            CreateMap<Item, CreateItemInputDto>();
            CreateMap<CreateCategoryInputDto, Category>();
            CreateMap<Category, CreateCategoryInputDto>();
        }
    }
}