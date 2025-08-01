using Applications.Features.ImportData;
using AutoMapper;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.MappingProfiles
{
    public class AdministrativeProfile : Profile
    {
        public AdministrativeProfile()
        {
            CreateMap<ProvinceImportDto, Province>()
                .ForMember(dest => dest.Communes, opt => opt.Ignore());

            CreateMap<CommuneImportDto, Commune>();
        }
    }
}
