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
    public class AdministrativeQueryProfile : Profile
    {
        public AdministrativeQueryProfile()
        {
            CreateMap<Province, ProvinceWithCommunesDto>();
            CreateMap<Commune, CommuneDto>();
        }
    }
}
