//using Applications.Interfaces.Repositories;
//using Microsoft.Extensions.Logging;

//namespace Infrastructure.Persistences
//{
//    public class UnitOfWork : IUnitOfWork, IDisposable
//    {
//        private readonly AppDbContext _context;
//        private readonly ILogger<UnitOfWork> _logger;

//        public IClientCredentialRepository ClientCredentials { get; }
//        public ISmsLogRepository SmsLogRepositories { get; }
//        public ISmsRetryQueueRepository ISmsRetryQueueRepositories { get; }
//        public IProvinceRepository ProvinceRepositories { get; }
//        public ICommuneRepository CommuneRepositories { get; }
//        public IMerchantRepository MerchantRepositories { get; }
//        public IMerchantBranchRepository MerchantBranchRepositories { get; }
//        public IPaymentTerminalRepository PaymentTerminalRepositories { get; }



//        public UnitOfWork(
//            AppDbContext context,
//            ILogger<UnitOfWork> logger,
//            IClientCredentialRepository clientCredentials,
//            ISmsLogRepository smsLogRepositories,
//            ISmsRetryQueueRepository smsRetryQueueRepositories,
//            IProvinceRepository provinceRepository,
//            ICommuneRepository communeRepository,
//            IMerchantRepository merchantRepositories,
//            IMerchantBranchRepository merchantBranchRepositories,
//            IPaymentTerminalRepository paymentTerminalRepositories)
//        {
//            _context = context ?? throw new ArgumentNullException(nameof(context));
//            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//            ClientCredentials = clientCredentials;
//            SmsLogRepositories = smsLogRepositories;
//            ISmsRetryQueueRepositories = smsRetryQueueRepositories;
//            ProvinceRepositories = provinceRepository;
//            CommuneRepositories = communeRepository;
//            MerchantRepositories = merchantRepositories;
//            MerchantBranchRepositories = merchantBranchRepositories;
//            PaymentTerminalRepositories = paymentTerminalRepositories;
//        }

//        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
//        {
//            return await _context.SaveChangesAsync(cancellationToken);
//        }

//        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
//        {
//            await _context.Database.BeginTransactionAsync(cancellationToken);
//        }

//        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
//        {
//            try
//            {
//                await _context.Database.CommitTransactionAsync(cancellationToken);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "❌ CommitTransactionAsync failed.");
//                throw;
//            }
//        }

//        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
//        {
//            try
//            {
//                await _context.Database.RollbackTransactionAsync(cancellationToken);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "❌ RollbackTransactionAsync failed.");
//                throw;
//            }
//        }

//        public void Dispose()
//        {
//            _context.Dispose();
//            GC.SuppressFinalize(this);
//        }
//    }
//}


//using Applications.Interfaces.Repositories;
//using Applications.Interfaces.Repositories.Partners;
//using Core.Entities.Partners;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Shared.Interfaces;

//namespace Infrastructure.Persistences
//{
//    public class UnitOfWork : IUnitOfWork
//    {
//        private readonly AppDbContext _context;
//        private readonly ILogger<UnitOfWork> _logger;
//        private readonly ICurrentUserService? _currentUserService;
//        public ICurrentUserService? CurrentUser => _currentUserService; // ✅ Expose ra ngoài

//        public IClientCredentialRepository ClientCredentials { get; }
//        public ISmsLogRepository SmsLogRepositories { get; }
//        public ISmsRetryQueueRepository ISmsRetryQueueRepositories { get; }
//        public IProvinceRepository ProvinceRepositories { get; }
//        public ICommuneRepository CommuneRepositories { get; }
//        public IMerchantRepository MerchantRepositories { get; }
//        public IMerchantBranchRepository MerchantBranchRepositories { get; }
//        public IPaymentTerminalRepository PaymentTerminalRepositories { get; }
//        public IRolePermissionRepository RolePermissionRepositories { get; }
//        public IUserRoleAssignmentRepository UserRoleAssignmentRepositories { get; }
//        public IUserRepository UserRepositories { get; }
//        public IRoleRepository RoleRepositories { get; }
//        public IPermissionRepository PermissionRepositories { get; }
//        public IUserMerchantRepository UserMerchantRepositories { get; }

//        // Product-related repositories
//        public IProductRepository ProductRepositories { get; }
//        public IProductCategoryRepository ProductCategoryRepositories { get; }
//        public IShopProductInventoryRepository ShopProductInventoryRepositories { get; }
//        public IPartnerTransactionCallbackLogRepository PartnerTransactionCallbackLogRepositories { get; }
//        public IPartnerMerchantStatusCallbackLogRepository PartnerMerchantStatusCallbackLogRepositories { get; }
//        public IPartnerOrderRepository PartnerOrderRepositories { get; }


//        public UnitOfWork(
//            AppDbContext context,
//        ILogger<UnitOfWork> logger,
//        ICurrentUserService? currentUserService,
//        IClientCredentialRepository clientCredentials,
//        ISmsLogRepository smsLogRepositories,
//        ISmsRetryQueueRepository smsRetryQueueRepositories,
//        IProvinceRepository provinceRepository,
//        ICommuneRepository communeRepository,
//        IMerchantRepository merchantRepositories,
//        IMerchantBranchRepository merchantBranchRepositories,
//        IPaymentTerminalRepository paymentTerminalRepositories,
//        IRolePermissionRepository rolePermissionRepositories,
//        IUserRoleAssignmentRepository userRoleAssignmentRepositories,
//        IUserRepository userRepositories,
//        IRoleRepository roleRepositories,
//        IPermissionRepository permissionRepositories,
//        IUserMerchantRepository userMerchantRepositories,
//        IProductRepository productRepositories,
//        IProductCategoryRepository productCategoryRepositories,
//        IShopProductInventoryRepository shopProductInventoryRepositories,
//        IPartnerTransactionCallbackLogRepository partnerTransactionCallbackLogRepositories,
//        IPartnerMerchantStatusCallbackLogRepository partnerMerchantStatusCallbackLogRepositories,
//        IPartnerOrderRepository partnerOrderRepositories)
//        {
//            _context = context ?? throw new ArgumentNullException(nameof(context));
//            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//            _currentUserService = currentUserService;

//            ClientCredentials = clientCredentials;
//            SmsLogRepositories = smsLogRepositories;
//            ISmsRetryQueueRepositories = smsRetryQueueRepositories;
//            ProvinceRepositories = provinceRepository;
//            CommuneRepositories = communeRepository;
//            MerchantRepositories = merchantRepositories;
//            MerchantBranchRepositories = merchantBranchRepositories;
//            PaymentTerminalRepositories = paymentTerminalRepositories;
//            RolePermissionRepositories = rolePermissionRepositories;
//            UserRoleAssignmentRepositories = userRoleAssignmentRepositories;
//            UserRepositories = userRepositories;
//            RoleRepositories = roleRepositories;
//            PermissionRepositories = permissionRepositories;
//            UserMerchantRepositories = userMerchantRepositories;
//            // Product-related repositories
//            ProductRepositories = productRepositories;
//            ProductCategoryRepositories = productCategoryRepositories;
//            ShopProductInventoryRepositories = shopProductInventoryRepositories;
//            PartnerMerchantStatusCallbackLogRepositories = partnerMerchantStatusCallbackLogRepositories;
//            PartnerTransactionCallbackLogRepositories = partnerTransactionCallbackLogRepositories;
//            PartnerOrderRepositories= partnerOrderRepositories;
//        }

//        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
//        {
//            return await _context.SaveChangesAsync(cancellationToken);
//        }

//        public async Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default)
//        {
//            var strategy = _context.Database.CreateExecutionStrategy();
//            await strategy.ExecuteAsync(async () =>
//            {
//                await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
//                try
//                {
//                    await action();
//                    await _context.SaveChangesAsync(cancellationToken);
//                    await transaction.CommitAsync(cancellationToken);
//                }
//                catch (Exception ex)
//                {
//                    await transaction.RollbackAsync(cancellationToken);
//                    _logger.LogError(ex, "❌ Transaction failed and was rolled back.");
//                    throw;
//                }
//            });
//        }
//        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default)
//        {
//            var strategy = _context.Database.CreateExecutionStrategy();
//            return await strategy.ExecuteAsync(async () =>
//            {
//                await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
//                try
//                {
//                    var result = await action();
//                    await _context.SaveChangesAsync(cancellationToken);
//                    await transaction.CommitAsync(cancellationToken);
//                    return result;
//                }
//                catch (Exception ex)
//                {
//                    await transaction.RollbackAsync(cancellationToken);
//                    _logger.LogError(ex, "❌ Transaction failed and was rolled back.");
//                    throw;
//                }
//            });
//        }
//        public async Task ExecuteWithTraceAsync(
//            Func<Task> action,
//            string traceId,
//            Func<Task>? afterCommit = null,
//            CancellationToken cancellationToken = default)
//        {
//            var strategy = _context.Database.CreateExecutionStrategy();
//            await strategy.ExecuteAsync(async () =>
//            {
//                await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
//                try
//                {
//                    _logger.LogInformation("[{TraceId}] ➡️ Begin transaction", traceId);

//                    await action();
//                    await _context.SaveChangesAsync(cancellationToken);
//                    await transaction.CommitAsync(cancellationToken);

//                    _logger.LogInformation("[{TraceId}] ✅ Transaction committed", traceId);

//                    if (afterCommit != null)
//                    {
//                        try
//                        {
//                            await afterCommit();
//                            _logger.LogInformation("[{TraceId}] 🔁 afterCommit callback executed", traceId);
//                        }
//                        catch (Exception ipnEx)
//                        {
//                            _logger.LogError(ipnEx, "[{TraceId}] ❌ afterCommit failed", traceId);
//                            // Optionally push to retry queue or DLQ
//                        }
//                    }

//                    if (_currentUserService?.UserId != null)
//                    {
//                        _logger.LogInformation("[{TraceId}] 🕵️ Audit by user: {UserId}", traceId, _currentUserService.UserId);
//                    }
//                }
//                catch (Exception ex)
//                {
//                    await transaction.RollbackAsync(cancellationToken);
//                    _logger.LogError(ex, "[{TraceId}] ❌ Transaction failed. Rolled back.", traceId);
//                    throw;
//                }
//            });
//        }
//        public void Dispose()
//        {
//            _context.Dispose();
//            GC.SuppressFinalize(this);
//        }
//    }
//}


using System;
using System.Threading;
using System.Threading.Tasks;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Repositories.Partners;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;

namespace Infrastructure.Persistences
{
    /// <summary>
    /// Unit of Work implementation for managing repositories and transactions.
    /// It provides a way to manage transactions across multiple repositories.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UnitOfWork> _logger;
        private readonly ICurrentUserService? _currentUserService;

        public ICurrentUserService? CurrentUser => _currentUserService;

        // Repositories for various entities
        public IClientCredentialRepository ClientCredentials { get; }
        public ISmsLogRepository SmsLogRepositories { get; }
        public ISmsRetryQueueRepository ISmsRetryQueueRepositories { get; }
        public IProvinceRepository ProvinceRepositories { get; }
        public ICommuneRepository CommuneRepositories { get; }
        public IMerchantRepository MerchantRepositories { get; }
        public IMerchantBranchRepository MerchantBranchRepositories { get; }
        public IPaymentTerminalRepository PaymentTerminalRepositories { get; }
        public IRolePermissionRepository RolePermissionRepositories { get; }
        public IUserRoleAssignmentRepository UserRoleAssignmentRepositories { get; }
        public IUserRepository UserRepositories { get; }
        public IRoleRepository RoleRepositories { get; }
        public IPermissionRepository PermissionRepositories { get; }
        public IUserMerchantRepository UserMerchantRepositories { get; }

        public IProductRepository ProductRepositories { get; }
        public IProductCategoryRepository ProductCategoryRepositories { get; }
        public IShopProductInventoryRepository ShopProductInventoryRepositories { get; }
        public IPartnerTransactionCallbackLogRepository PartnerTransactionCallbackLogRepositories { get; }
        public IPartnerMerchantStatusCallbackLogRepository PartnerMerchantStatusCallbackLogRepositories { get; }
        public IPartnerOrderRepository PartnerOrderRepositories { get; }
        public ILarkEmailLogRepository LarkEmailLogRepositories { get; }
        public IOtpCodeLogRepository OtpCodeLogRepositories { get; }
        public ILarkTokensRepository LarkTokensRepository { get; }


        public UnitOfWork(
            AppDbContext context,
            ILogger<UnitOfWork> logger,
            ICurrentUserService? currentUserService,
            IClientCredentialRepository clientCredentials,
            ISmsLogRepository smsLogRepositories,
            ISmsRetryQueueRepository smsRetryQueueRepositories,
            IProvinceRepository provinceRepository,
            ICommuneRepository communeRepository,
            IMerchantRepository merchantRepositories,
            IMerchantBranchRepository merchantBranchRepositories,
            IPaymentTerminalRepository paymentTerminalRepositories,
            IRolePermissionRepository rolePermissionRepositories,
            IUserRoleAssignmentRepository userRoleAssignmentRepositories,
            IUserRepository userRepositories,
            IRoleRepository roleRepositories,
            IPermissionRepository permissionRepositories,
            IUserMerchantRepository userMerchantRepositories,
            IProductRepository productRepositories,
            IProductCategoryRepository productCategoryRepositories,
            IShopProductInventoryRepository shopProductInventoryRepositories,
            IPartnerTransactionCallbackLogRepository partnerTransactionCallbackLogRepositories,
            IPartnerMerchantStatusCallbackLogRepository partnerMerchantStatusCallbackLogRepositories,
            IPartnerOrderRepository partnerOrderRepositories,
            ILarkEmailLogRepository larkEmailLogRepositories,
            IOtpCodeLogRepository otpCodeLogRepositories,
            ILarkTokensRepository larkTokensRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _currentUserService = currentUserService;

            // Initialize all repositories
            ClientCredentials = clientCredentials;
            SmsLogRepositories = smsLogRepositories;
            ISmsRetryQueueRepositories = smsRetryQueueRepositories;
            ProvinceRepositories = provinceRepository;
            CommuneRepositories = communeRepository;
            MerchantRepositories = merchantRepositories;
            MerchantBranchRepositories = merchantBranchRepositories;
            PaymentTerminalRepositories = paymentTerminalRepositories;
            RolePermissionRepositories = rolePermissionRepositories;
            UserRoleAssignmentRepositories = userRoleAssignmentRepositories;
            UserRepositories = userRepositories;
            RoleRepositories = roleRepositories;
            PermissionRepositories = permissionRepositories;
            UserMerchantRepositories = userMerchantRepositories;

            ProductRepositories = productRepositories;
            ProductCategoryRepositories = productCategoryRepositories;
            ShopProductInventoryRepositories = shopProductInventoryRepositories;
            PartnerTransactionCallbackLogRepositories = partnerTransactionCallbackLogRepositories;
            PartnerMerchantStatusCallbackLogRepositories = partnerMerchantStatusCallbackLogRepositories;
            PartnerOrderRepositories = partnerOrderRepositories;
            LarkEmailLogRepositories = larkEmailLogRepositories;
            OtpCodeLogRepositories = otpCodeLogRepositories;
            LarkTokensRepository = larkTokensRepository;
        }

        /// <summary>
        /// Save changes to the database.
        /// </summary>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Execute an action within a database transaction.
        /// </summary>
        public async Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    await action();
                    await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    _logger.LogError(ex, "❌ Transaction failed and was rolled back.");
                    throw;
                }
            });
        }

        /// <summary>
        /// Execute an action within a database transaction and return a result.
        /// </summary>
        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    var result = await action();
                    await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return result;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    _logger.LogError(ex, "❌ Transaction failed and was rolled back.");
                    throw;
                }
            });
        }

        /// <summary>
        /// Execute an action with trace logging and optional after commit actions.
        /// </summary>
        public async Task ExecuteWithTraceAsync(
            Func<Task> action,
            string traceId,
            Func<Task>? afterCommit = null,
            CancellationToken cancellationToken = default)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    _logger.LogInformation("[{TraceId}] ➡️ Begin transaction", traceId);

                    await action();
                    await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                    _logger.LogInformation("[{TraceId}] ✅ Transaction committed", traceId);

                    if (afterCommit != null)
                    {
                        try
                        {
                            await afterCommit();
                            _logger.LogInformation("[{TraceId}] 🔁 afterCommit callback executed", traceId);
                        }
                        catch (Exception ipnEx)
                        {
                            _logger.LogError(ipnEx, "[{TraceId}] ❌ afterCommit failed", traceId);
                        }
                    }

                    if (_currentUserService?.UserId != null)
                    {
                        _logger.LogInformation("[{TraceId}] 🕵️ Audit by user: {UserId}", traceId, _currentUserService.UserId);
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    _logger.LogError(ex, "[{TraceId}] ❌ Transaction failed. Rolled back.", traceId);
                    throw;
                }
            });
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
