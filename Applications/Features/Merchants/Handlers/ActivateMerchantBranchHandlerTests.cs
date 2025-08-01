//using Applications.Features.MerchantBranches.Commands;
//using Applications.Features.MerchantBranches.Dtos;
//using Applications.Features.MerchantBranches.Handlers;
//using Applications.Interfaces.Repositories;
//using Applications.Interfaces.Services;
//using Applications.Services.Interfaces;
//using Core.Entities.AppUsers;
//using Core.Entities;
//using Core.Enumerables;
//using Microsoft.Extensions.Options;
//using Moq;
//using Shared.Constants;
//using Shared.Results;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;
//using Serilog;
//using System.Linq.Expressions;

//namespace Applications.Features.Merchants.Handlers
//{
//    public class ActivateMerchantBranchHandlerTests
//    {
//        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
//        private readonly Mock<ITokenService> _tokenServiceMock = new();
//        private readonly Mock<INotificationService> _notificationServiceMock = new();
//        private readonly Mock<IOptions<AppSettings>> _appSettingsMock = new();

//        private readonly ActivateMerchantBranchHandler _handler;

//        public ActivateMerchantBranchHandlerTests()
//        {
//            var appSettings = new AppSettings { FrontendUrl = "https://merchant.zenshop.vn" };
//            _appSettingsMock.Setup(x => x.Value).Returns(appSettings);

//            _handler = new ActivateMerchantBranchHandler(
//                _unitOfWorkMock.Object,
//                _tokenServiceMock.Object,
//                _notificationServiceMock.Object,
//                _appSettingsMock.Object
//            );
//        }

//        // Test: Handle Branch Not Found
//        [Fact]
//        public async Task Should_Return_Error_When_Branch_Not_Found()
//        {
//            // Arrange
//            var command = new ActivateMerchantBranchCommand
//            {
//                MerchantBranchId = Guid.NewGuid(),
//                MerchantBranchCode = "BR001"
//            };

//            _unitOfWorkMock.Setup(u => u.MerchantBranchRepositories.GetWithMerchantByIdAsync(command.MerchantBranchId))
//                .ReturnsAsync((MerchantBranch)null!);

//            _unitOfWorkMock.Setup(u => u.ExecuteInTransactionAsync(It.IsAny<Func<Task<BaseResponse<MerchantBranchStatusRes>>>>(), default))
//                .Returns<Func<Task<BaseResponse<MerchantBranchStatusRes>>>, CancellationToken>((func, _) => func());

//            // Act
//            Log.Information("Running test: {TestName}", nameof(Should_Return_Error_When_Branch_Not_Found));
//            var result = await _handler.Handle(command, default);

//            // Assert
//            Assert.False(result.IsSuccess);
//            Assert.Equal("Merchant Branch not found.", result.Message);
//            Log.Information("Test completed: {TestName}, result: {Result}", nameof(Should_Return_Error_When_Branch_Not_Found), result.Message);
//        }

//        // Test: Handle Branch Already Active
//        [Fact]
//        public async Task Should_Return_Success_If_Already_Active()
//        {
//            // Arrange
//            var branchId = Guid.NewGuid();
//            var merchant = new Merchant { Id = Guid.NewGuid(), IsActive = true };
//            var branch = new MerchantBranch
//            {
//                Id = branchId,
//                Merchant = merchant,
//                Status = EBranchStatus.Active
//            };

//            _unitOfWorkMock.Setup(u => u.MerchantBranchRepositories.GetWithMerchantByIdAsync(branchId))
//                .ReturnsAsync(branch);

//            _unitOfWorkMock.Setup(u => u.ExecuteInTransactionAsync(It.IsAny<Func<Task<BaseResponse<MerchantBranchStatusRes>>>>(), default))
//                .Returns<Func<Task<BaseResponse<MerchantBranchStatusRes>>>, CancellationToken>((func, _) => func());

//            // Act
//            Log.Information("Running test: {TestName}", nameof(Should_Return_Success_If_Already_Active));
//            var result = await _handler.Handle(new ActivateMerchantBranchCommand { MerchantBranchId = branchId }, default);

//            // Assert
//            Assert.True(result.IsSuccess);
//            Assert.Equal("Already active", result.Message);
//            Log.Information("Test completed: {TestName}, result: {Result}", nameof(Should_Return_Success_If_Already_Active), result.Message);
//        }

//        // Test: Handle Branch Activation and User Creation
//        [Fact]
//        public async Task Should_Activate_Branch_And_Create_User()
//        {
//            // Arrange
//            var branchId = Guid.NewGuid();
//            var tenantId = Guid.NewGuid();
//            var merchant = new Merchant
//            {
//                Id = Guid.NewGuid(),
//                TenantId = tenantId,
//                IsActive = false,
//                RepresentativePhoneNumber = "0123456789",
//                RepresentativeEmail = "test@example.com",
//                MerchantName = "Test Merchant"
//            };

//            var branch = new MerchantBranch
//            {
//                Id = branchId,
//                Merchant = merchant,
//                Status = EBranchStatus.Inactive
//            };

//            var role = new Role { Id = Guid.NewGuid(), Name = "MerchantOwner", TenantId = tenantId };

//            _unitOfWorkMock.Setup(u => u.MerchantBranchRepositories.GetWithMerchantByIdAsync(branchId))
//                .ReturnsAsync(branch);

//            _unitOfWorkMock.Setup(u => u.UserRepositories.GetQueryable(It.IsAny<Expression<Func<User, bool>>>()))
//                .Returns(Array.Empty<User>().AsQueryable());

//            _unitOfWorkMock.Setup(u => u.RoleRepositories.GetQueryable(It.IsAny<Expression<Func<Role, bool>>>()))
//                .Returns(new[] { role }.AsQueryable());

//            _unitOfWorkMock.Setup(u => u.ExecuteInTransactionAsync(It.IsAny<Func<Task<BaseResponse<MerchantBranchStatusRes>>>>(), default))
//                .Returns<Func<Task<BaseResponse<MerchantBranchStatusRes>>>, CancellationToken>((func, _) => func());

//            _unitOfWorkMock.Setup(u => u.UserRepositories.GetQueryable(It.IsAny<Expression<Func<User, bool>>>()))
//                .Returns(Array.Empty<User>().AsQueryable());  // Không sử dụng optional arguments

//            // Mock thêm UserMerchant vào repository
//            _unitOfWorkMock.Setup(u => u.UserMerchantRepositories.AddAsync(It.Is<UserMerchant>(x => x != null)))
//                .Returns(Task.CompletedTask); // Đảm bảo thêm đúng đối tượng mà không sử dụng optional arguments



//            // Mock thêm UserRoleAssignment vào repository
//            _unitOfWorkMock.Setup(u => u.UserRoleAssignmentRepositories.AddAsync(It.Is<UserRoleAssignment>(x => x != null)))
//                .Returns(Task.CompletedTask);  // Đảm bảo thêm đúng đối tượng mà không sử dụng optional arguments


//            // Mock việc tạo ResetPasswordToken cho user
//            _tokenServiceMock.Setup(t => t.GenerateResetPasswordToken(It.IsAny<Guid>()))
//                .Returns("mock-token");  // Trả về một token giả cho bất kỳ Guid nào
//            _notificationServiceMock.Setup(n => n.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
//                .Returns(Task.CompletedTask);

//            // Act
//            Log.Information("Running test: {TestName}", nameof(Should_Activate_Branch_And_Create_User));
//            var result = await _handler.Handle(new ActivateMerchantBranchCommand { MerchantBranchId = branchId }, default);

//            // Assert
//            Assert.True(result.IsSuccess);
//            Assert.Equal("Branch activated and user created", result.Message);

//            _unitOfWorkMock.Verify(u => u.UserRepositories.AddAsync(It.IsAny<User>()), Times.Once);
//            _unitOfWorkMock.Verify(u => u.UserMerchantRepositories.AddAsync(It.IsAny<UserMerchant>()), Times.Once);
//            _unitOfWorkMock.Verify(u => u.UserRoleAssignmentRepositories.AddAsync(It.IsAny<UserRoleAssignment>()), Times.Once);

//            _notificationServiceMock.Verify(n => n.SendEmailAsync(
//                It.Is<string>(e => e == "test@example.com"),
//                It.IsAny<string>(),
//                It.Is<string>(content => content.Contains("reset-password?token="))
//            ), Times.Once);

//            Log.Information("Test completed: {TestName}, result: {Result}", nameof(Should_Activate_Branch_And_Create_User), result.Message);
//        }
//    }
//}
