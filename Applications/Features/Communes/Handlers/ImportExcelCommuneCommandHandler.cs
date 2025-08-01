using Applications.DTOs;
using Applications.Features.Communes.Commands;
using Applications.Services.Interfaces;
using AutoMapper;
using MediatR;
using Shared.Results;
using System.Diagnostics;

namespace Applications.Features.Communes.Handlers
{
    public class ImportExcelCommuneCommandHandler : IRequestHandler<ImportExcelCommuneCommand, BaseResponse<List<CommuneDto>>>
    {
        private readonly ICommuneService _communeService;
        private readonly IExcelService _excelService;
        private readonly IMapper _mapper;

        public ImportExcelCommuneCommandHandler(ICommuneService communeService, IExcelService excelService, IMapper mapper)
        {
            _communeService = communeService;
            _excelService = excelService;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<CommuneDto>>> Handle(ImportExcelCommuneCommand request, CancellationToken cancellationToken)
        {
            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

            try
            {
                // Sử dụng ExcelService để đọc tệp Excel và trả về dữ liệu
                var communes = await _excelService.ImportCommuneExcelDataAsync(request.File);

                // Chuyển đổi từ danh sách Commune thành CommuneDto
                var communeDtos = _mapper.Map<List<CommuneDto>>(communes);

                // Tiến hành lưu vào cơ sở dữ liệu
                await _communeService.CreateManyAsync(communeDtos);

                // Trả về phản hồi thành công với danh sách CommuneDto
                return BaseResponse<List<CommuneDto>>.Success(communeDtos, "Imported successfully", traceId);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CommuneDto>>.Error($"Error occurred during import: {ex.Message}", "500", traceId);
            }
        }
    }
}
