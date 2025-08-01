using Applications.DTOs;
using Applications.Features.ClientCredentials.Commands;
using AutoMapper;
using Core.Entities;

namespace Applications.MappingProfiles
{
    public class ClientCredentialProfile : Profile
    {
        public ClientCredentialProfile()
        {
            // Dùng khi cần hiển thị danh sách ClientCredential (Admin xem)
            CreateMap<ClientCredential, ClientCredentialDto>().ReverseMap();

            // Dùng khi tạo mới client → Hash password vào ClientSecretHash
            CreateMap<CreateClientCredentialCommand, ClientCredential>()
                .ForMember(dest => dest.ClientSecretHash, opt =>
                    opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.ClientSecret)))
                .ForMember(dest => dest.Description, opt => opt.NullSubstitute(string.Empty));

            // Không cần map cho login command → xử lý thủ công trong Handler
        }
    }

}
