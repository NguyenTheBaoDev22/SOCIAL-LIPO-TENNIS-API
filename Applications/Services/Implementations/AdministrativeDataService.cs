using Applications.Features.ImportData;
using Applications.Interfaces.Repositories;
using Applications.Services.Interfaces;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Shared.Results;

namespace Applications.Services.Implementations
{
    public class AdministrativeDataService : IAdministrativeDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdministrativeDataService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<object>> ImportDataFromJsonFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BaseResponse<object>.Error("File không hợp lệ hoặc rỗng.", "400");

            // 1. Kiểm tra đã có dữ liệu chưa
            var anyProvince = await _unitOfWork.ProvinceRepositories.AnyAsync(p => !p.IsDeleted);
            if (anyProvince)
                return BaseResponse<object>.Error("Dữ liệu hành chính đã được import trước đó.", "409");

            // 2. Đọc nội dung file JSON
            string jsonString;
            try
            {
                using var stream = file.OpenReadStream();
                using var reader = new StreamReader(stream);
                jsonString = await reader.ReadToEndAsync();
            }
            catch (Exception ex)
            {
                return BaseResponse<object>.Error($"Không thể đọc file JSON: {ex.Message}", "500");
            }

            // 3. Parse dữ liệu
            AdministrativeUnitsRootImportDto? rootDto;
            try
            {
                rootDto = JsonConvert.DeserializeObject<AdministrativeUnitsRootImportDto>(jsonString);
            }
            catch (Exception ex)
            {
                return BaseResponse<object>.Error($"Lỗi parse JSON: {ex.Message}", "400");
            }

            if (rootDto?.Data == null || !rootDto.Data.Items.Any())
                return BaseResponse<object>.Error("File không có dữ liệu hợp lệ.", "400");

            // 4. Mapping
            var provincesToCreate = new List<Province>();
            int communeCount = 0;

            foreach (var provinceDto in rootDto.Data.Items)
            {
                var province = _mapper.Map<Province>(provinceDto);
                province.Communes = _mapper.Map<ICollection<Commune>>(provinceDto.Communes);

                foreach (var commune in province.Communes)
                {
                    commune.ProvinceId = province.Id;
                }

                provincesToCreate.Add(province);
                communeCount += province.Communes.Count;
            }

            // 5. Lưu vào DB
            try
            {
                await _unitOfWork.ExecuteInTransactionAsync(async () =>
                {
                    await _unitOfWork.ProvinceRepositories.AddAsync(provincesToCreate);
                });

                var result = new
                {
                    Message = "Import thành công!",
                    Provinces = provincesToCreate.Count,
                    Communes = communeCount
                };

                return BaseResponse<object>.Success(result, "Import completed", "00");
            }
            catch (Exception ex)
            {
                return BaseResponse<object>.Error($"Lỗi khi lưu CSDL: {ex.Message}", "500");
            }
        }
    }
}
