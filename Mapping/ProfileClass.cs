using AutoMapper;
using System;
using TechnicalReport_BL.DTO;
using TechnicalReport_BL.Models;
using TechnicalReport_Data.DTO;


namespace TechnicalReport_BL.Mapping
{
    public static class Mapping
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg => {
                // This line ensures that internal properties are also mapped over.
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<MappingProfile>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });

        public static IMapper Mapper => Lazy.Value;
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DateDTO, DatebaseDTO>();
            CreateMap<DateDTO, Date>();
        }
    }

}
