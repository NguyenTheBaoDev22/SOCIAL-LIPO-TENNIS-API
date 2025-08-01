using Applications.Interfaces;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Repositories.Logs;
using Applications.Interfaces.Repositories.Partners;
using Applications.MappingProfiles;
using Applications.Services.Implementations;
using Applications.Services.Interfaces;
using Core.Entities.Partners;
using Infrastructure.ExternalSystems.Zalo;
using Infrastructure.Persistences;
using Infrastructure.Persistences.Repositories;
using Infrastructure.Persistences.Repositories.Logs;
using Infrastructure.Persistences.Repositories.Partners;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using Shared.Interfaces;

namespace Infrastructure.DependencyInjections
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            // Đăng ký DbContext với Npgsql và chuỗi kết nối
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Đăng ký các service repository và UnitOfWork
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Đăng ký các repositories khác
            services.AddRepositories();

            return services;
        }
        public static void AddServices(this IServiceCollection services)
        {
            //services.AddSingleton<ISpamProtectionService, RedisSpamProtectionService>();
            // Đăng ký IProvinceService và ProvinceService vào DI container
            services.AddScoped<IProvinceService, ProvinceService>();
            services.AddScoped<ICommuneService, CommuneService>();
            services.AddScoped<IExcelService, ExcelService>();
            services.AddScoped<IPartnerCallbackService, PartnerCallbackService>();
            services.AddScoped<IPaymentGatewayService, ZenPayPaymentGatewayService>();
            services.AddScoped<IMerchantService, MerchantService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddHttpClient<IZaloApiClient, ZaloApiClient>();
            services.AddScoped<IZaloAuthenticationService, ZaloAuthenticationService>();
           services.AddScoped<IAdministrativeDataService, AdministrativeDataService>();
            services.AddScoped<IOtpService, OtpService>();
           
            
            // ✅ Sau đó mới đăng ký service cần sử dụng

            // Cài giấy phép miễn phí cho EPPlus (NonCommercial)
            // If you are a Noncommercial organization.
            ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization"); //This will also set the Company property to the organization name provided in the argument.


        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Thêm các repository khác nếu cần
            services.AddScoped<IProvinceRepository, ProvinceRepository>();
            services.AddScoped<ICommuneRepository, CommuneRepository>();
            services.AddScoped<IMerchantRepository, MerchantRepository>();
            services.AddScoped<IMerchantBranchRepository, MerchantBranchRepository>();
            services.AddScoped<IPaymentTerminalRepository, PaymentTerminalRepository>();

            // AppUsers / Authorization Repositories
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleAssignmentRepository, UserRoleAssignmentRepository>();

            services.AddScoped<IUserMerchantRepository, UserMerchantRepository>();
            services.AddScoped<IProductRepository,ProductRepository>();
            services.AddScoped<IShopProductInventoryRepository, ShopProductInventoryRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<IProductImportRepository, ProductImportRepository>();
            services.AddScoped<IPartnerMerchantStatusCallbackLogRepository, PartnerMerchantStatusCallbackLogRepository>();
            services.AddScoped<IPartnerTransactionCallbackLogRepository, PartnerTransactionCallbackLogRepository>();
            services.AddScoped<IPartnerOrderRepository, PartnerOrderRepository>();
            services.AddScoped<IZaloAuthLogRepository, ZaloAuthLogRepository>();
            services.AddScoped<ILarkEmailLogRepository, LarkEmailLogRepository>();
            services.AddScoped<IOtpCodeLogRepository, OtpCodeLogRepository>();
            services.AddScoped<IOtpRequestCounterRepository, OtpRequestCounterRepository>();
            services.AddScoped<ILarkTokensRepository, LarkTokensRepository>();
            return services;
        }
    }

}
