//namespace Applications.Interfaces.Repositories
//{
//    //public interface IUnitOfWork : IDisposable
//    //{
//    //    IClientCredentialRepository ClientCredentials { get; }
//    //    IProvinceRepository ProvinceRepositories { get; }
//    //    ICommuneRepository CommuneRepositories { get; }
//    //    public IMerchantRepository MerchantRepositories { get; }
//    //    public IMerchantBranchRepository MerchantBranchRepositories { get; }
//    //    public IPaymentTerminalRepository PaymentTerminalRepositories { get; }


//    //    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
//    //    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
//    //    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
//    //    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
//    //}
//}

//using Applications.Interfaces.Repositories.Partners;
//using Core.Entities.Partners;

//namespace Applications.Interfaces.Repositories
//{
//    public interface IUnitOfWork : IDisposable
//    {
//        IClientCredentialRepository ClientCredentials { get; }
//        ISmsLogRepository SmsLogRepositories { get; }
//        ISmsRetryQueueRepository ISmsRetryQueueRepositories { get; }
//        IProvinceRepository ProvinceRepositories { get; }
//        ICommuneRepository CommuneRepositories { get; }
//        IMerchantRepository MerchantRepositories { get; }
//        IMerchantBranchRepository MerchantBranchRepositories { get; }
//        IPaymentTerminalRepository PaymentTerminalRepositories { get; }

//        IRolePermissionRepository RolePermissionRepositories { get; }
//        IUserRoleAssignmentRepository UserRoleAssignmentRepositories { get; }
//        IUserRepository UserRepositories { get; }
//        IRoleRepository RoleRepositories { get; }
//        IPermissionRepository PermissionRepositories { get; }
//        IUserMerchantRepository UserMerchantRepositories { get; }
//        IPartnerTransactionCallbackLogRepository PartnerTransactionCallbackLogRepositories { get; }
//        IPartnerMerchantStatusCallbackLogRepository PartnerMerchantStatusCallbackLogRepositories { get; }
//        IPartnerOrderRepository PartnerOrderRepositories { get; }
//        public IProductRepository ProductRepositories { get; }
//        public IProductCategoryRepository ProductCategoryRepositories { get; }
//        public IShopProductInventoryRepository ShopProductInventoryRepositories { get; }
//        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

//        Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default);
//        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default);

//        Task ExecuteWithTraceAsync(
//            Func<Task> action,
//            string traceId,
//            Func<Task>? afterCommit = null,
//            CancellationToken cancellationToken = default);
//    }
//}
using System;
using System.Threading;
using System.Threading.Tasks;
using Applications.Interfaces.Repositories.Partners;
using Core.Entities;
using Shared.Interfaces;

namespace Applications.Interfaces.Repositories
{
    /// <summary>
    /// Interface for Unit of Work pattern, coordinating repository interactions and managing transactions.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Access the current user for audit/logging.
        /// </summary>
        ICurrentUserService? CurrentUser { get; }

        /// <summary>
        /// Save changes to the database.
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute an action within a database transaction.
        /// </summary>
        Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute an action within a database transaction and return a result.
        /// </summary>
        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute an action with tracing and optional after commit actions.
        /// </summary>
        Task ExecuteWithTraceAsync(Func<Task> action, string traceId, Func<Task>? afterCommit = null, CancellationToken cancellationToken = default);

        // Repositories for various entities
        IClientCredentialRepository ClientCredentials { get; }
        ISmsLogRepository SmsLogRepositories { get; }
        ISmsRetryQueueRepository ISmsRetryQueueRepositories { get; }
        IProvinceRepository ProvinceRepositories { get; }
        ICommuneRepository CommuneRepositories { get; }
        IMerchantRepository MerchantRepositories { get; }
        IMerchantBranchRepository MerchantBranchRepositories { get; }
        IPaymentTerminalRepository PaymentTerminalRepositories { get; }
        IRolePermissionRepository RolePermissionRepositories { get; }
        IUserRoleAssignmentRepository UserRoleAssignmentRepositories { get; }
        IUserRepository UserRepositories { get; }
        IRoleRepository RoleRepositories { get; }
        IPermissionRepository PermissionRepositories { get; }
        IUserMerchantRepository UserMerchantRepositories { get; }

        IProductRepository ProductRepositories { get; }
        IProductCategoryRepository ProductCategoryRepositories { get; }
        IShopProductInventoryRepository ShopProductInventoryRepositories { get; }
        IPartnerTransactionCallbackLogRepository PartnerTransactionCallbackLogRepositories { get; }
        IPartnerMerchantStatusCallbackLogRepository PartnerMerchantStatusCallbackLogRepositories { get; }
        IPartnerOrderRepository PartnerOrderRepositories { get; }
        ILarkEmailLogRepository LarkEmailLogRepositories { get; }
        ILarkTokensRepository LarkTokensRepository { get; }
    }
}
