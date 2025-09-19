using AutoMapper;
using Boost.Admin.Data.Models;
using Boost.Admin.Data.Models.Catalog;
using Boost.Admin.DTOs;

namespace Boost.Admin.Configuration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            // entity -> Dto
            CreateMap<CatalogueItem, SIMProductDto>();
            CreateMap<MasterProduct, SIMProductDto>();
            CreateMap<Category, CategoryDto>();


            // Dto -> entity
            
        }
    }
}
