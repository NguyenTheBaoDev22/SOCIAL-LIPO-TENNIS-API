using Applications.DTOs;
using Applications.Interfaces.Repositories;
using Applications.Services.Interfaces;
using AutoMapper;
using Core.Entities;
using Shared.Results;
using System.Diagnostics;

namespace Applications.Services.Implementations
{
    public class CommuneService : BaseService<CommuneDto, Commune>, ICommuneService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommuneService(
            ICommuneRepository communeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper) : base(communeRepository, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Tìm Commune theo mã
        public async Task<BaseResponse<CommuneDto>> GetByCodeAsync(string code)
        {
            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

            // Lấy commune theo mã
            var commune = await _unitOfWork.CommuneRepositories.GetByCodeAsync(code);
            if (commune == null)
                return BaseResponse<CommuneDto>.Error("Commune not found", "404", traceId);

            // Chuyển từ entity sang DTO
            return BaseResponse<CommuneDto>.Success(_mapper.Map<CommuneDto>(commune), "Retrieved successfully", "00");
        }

        // Các phương thức CRUD khác sẽ tự động được kế thừa từ BaseService
    }
}
