using Applications.DTOs;
using Applications.Features.MerchantBranches.Commands;
using Applications.Features.MerchantBranches.Dtos;
using Applications.Features.Shops.Inventories.DTOs;
using Applications.Features.Shops.Products.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Entities.Shops;

namespace Applications.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Ánh xạ từ Entity sang DTO
            CreateMap<Province, ProvinceDto>().ReverseMap();
            CreateMap<Commune, CommuneDto>().ReverseMap();
            CreateMap<Province, ProvinceRes>();

            // Ánh xạ từ CreateMerchantBranchCommand sang MerchantBranch
            CreateMap<CreateMerchantBranchCommand, MerchantBranch>()
                .ForMember(dest => dest.MerchantCode, opt => opt.MapFrom(src => src.MerchantCode))
                .ForMember(dest => dest.MerchantId, opt => opt.MapFrom(src => src.MerchantId))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.BranchName))
                .ForMember(dest => dest.BranchEmail, opt => opt.MapFrom(src => src.BranchEmail))
                .ForMember(dest => dest.ProvinceCode, opt => opt.MapFrom(src => src.ProvinceCode))
                .ForMember(dest => dest.ProvinceName, opt => opt.MapFrom(src => src.ProvinceName))
                .ForMember(dest => dest.CommuneCode, opt => opt.MapFrom(src => src.CommuneCode))
                .ForMember(dest => dest.CommuneName, opt => opt.MapFrom(src => src.CommuneName))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude))
                .ForMember(dest => dest.BranchAddress, opt => opt.MapFrom(src => src.BranchAddress))
                .ForMember(dest => dest.ExteriorImages, opt => opt.MapFrom(src => src.ExteriorImages))
                .ForMember(dest => dest.InteriorImages, opt => opt.MapFrom(src => src.InteriorImages));

            // Ánh xạ từ MerchantBranch sang MerchantBranchDto
            CreateMap<MerchantBranch, MerchantBranchDto>()
                .ForMember(dest => dest.MerchantCode, opt => opt.MapFrom(src => src.MerchantCode))
                .ForMember(dest => dest.MerchantId, opt => opt.MapFrom(src => src.MerchantId))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.BranchName))
                .ForMember(dest => dest.BranchEmail, opt => opt.MapFrom(src => src.BranchEmail))
                .ForMember(dest => dest.ProvinceCode, opt => opt.MapFrom(src => src.ProvinceCode))
                .ForMember(dest => dest.ProvinceName, opt => opt.MapFrom(src => src.ProvinceName))
                .ForMember(dest => dest.CommuneCode, opt => opt.MapFrom(src => src.CommuneCode))
                .ForMember(dest => dest.CommuneName, opt => opt.MapFrom(src => src.CommuneName))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude))
                .ForMember(dest => dest.BranchAddress, opt => opt.MapFrom(src => src.BranchAddress))
                .ForMember(dest => dest.ExteriorImages, opt => opt.MapFrom(src => src.ExteriorImages))
                .ForMember(dest => dest.InteriorImages, opt => opt.MapFrom(src => src.InteriorImages));

            // Ánh xạ từ MerchantBranch sang CreateMerchantBranchRes
            CreateMap<MerchantBranch, CreateMerchantBranchRes>()
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.BranchName))
                .ForMember(dest => dest.MerchantCode, opt => opt.MapFrom(src => src.MerchantCode))
                .ForMember(dest => dest.MerchantId, opt => opt.MapFrom(src => src.MerchantId))
                .ForMember(dest => dest.BranchEmail, opt => opt.MapFrom(src => src.BranchEmail))
                .ForMember(dest => dest.ExteriorImages, opt => opt.MapFrom(src => src.ExteriorImages))
                .ForMember(dest => dest.InteriorImages, opt => opt.MapFrom(src => src.InteriorImages))
                .ForMember(dest => dest.ProvinceCode, opt => opt.MapFrom(src => src.ProvinceCode))
                .ForMember(dest => dest.ProvinceName, opt => opt.MapFrom(src => src.ProvinceName))
                .ForMember(dest => dest.CommuneCode, opt => opt.MapFrom(src => src.CommuneCode))
                .ForMember(dest => dest.CommuneName, opt => opt.MapFrom(src => src.CommuneName))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude))
                .ForMember(dest => dest.BranchAddress, opt => opt.MapFrom(src => src.BranchAddress));

            // ProductCategory Mapping
            CreateMap<ProductCategory, ProductCategoryDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<ProductCategoryDto, ProductCategory>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            // ShopProductInventory Mapping
            CreateMap<ShopProductInventory, ShopInventoryAuditDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.AdjustmentQty, opt => opt.MapFrom(src => src.ActualQty - src.ExpectedQty));

            CreateMap<ShopInventoryAuditDto, ShopProductInventory>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ExpectedQty, opt => opt.MapFrom(src => src.ExpectedQty))
                .ForMember(dest => dest.ActualQty, opt => opt.MapFrom(src => src.ActualQty))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason))
                .ForMember(dest => dest.AuditorName, opt => opt.MapFrom(src => src.AuditorName));


        }
    }

}
