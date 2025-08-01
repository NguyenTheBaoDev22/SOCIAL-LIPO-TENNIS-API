using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.ImportData
{
    public class AdministrativeUnitsRootImportDto
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public AdministrativeUnitsDataDto Data { get; set; }
        public string TraceId { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsSuccess { get; set; }
    }



    public class AdministrativeUnitsDataDto
    {
        public List<ProvinceImportDto> Items { get; set; }
        public AdministrativeUnitsMetaDto Meta { get; set; }
    }
    public class AdministrativeUnitsMetaDto
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
    }
    public class ProvinceImportDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DivisionType { get; set; }
        public List<CommuneImportDto> Communes { get; set; }
    }

    public class CommuneImportDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DivisionType { get; set; }
    }

    public class ProvinceWithCommunesDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DivisionType { get; set; }
        public List<CommuneDto> Communes { get; set; }
    }

    public class CommuneDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DivisionType { get; set; }
    }
}
