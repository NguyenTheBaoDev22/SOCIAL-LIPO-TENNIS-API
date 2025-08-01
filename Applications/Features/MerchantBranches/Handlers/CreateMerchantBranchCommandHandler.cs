

using Applications.Features.MerchantBranches.Commands;
using Applications.Features.MerchantBranches.Dtos;
using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities;
using MediatR;
using Serilog;  // Import Serilog
using Shared.Results; // Import the BaseResponse class

namespace Applications.Features.MerchantBranches.Handlers
{
    public class CreateMerchantBranchCommandHandler : IRequestHandler<CreateMerchantBranchCommand, BaseResponse<CreateMerchantBranchRes>>
    {
        private readonly IMerchantBranchRepository _repository;
        private readonly IMerchantRepository _merchantRepository;
        private readonly IMapper _mapper;

        public CreateMerchantBranchCommandHandler(IMerchantBranchRepository repository, IMerchantRepository merchantRepository, IMapper mapper)
        {
            _repository = repository;
            _merchantRepository = merchantRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<CreateMerchantBranchRes>> Handle(CreateMerchantBranchCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Log the start of the method execution
                Log.Information("Processing CreateMerchantBranchCommand for MerchantId: {MerchantId}", request.MerchantId);

                // Verify the MerchantId exists
                var merchant = await _merchantRepository.GetByIdAsync(request.MerchantId, cancellationToken);

                if (merchant == null)
                {
                    // Log and return an error response if the merchant is not found
                    Log.Warning("Merchant with ID {MerchantId} not found.", request.MerchantId);
                    return BaseResponse<CreateMerchantBranchRes>.Error("Merchant with the provided ID does not exist.");
                }

                // Check if the MerchantCode matches the MerchantId
                if (merchant.MerchantCode != request.MerchantCode)
                {
                    // Log and return an error response if the MerchantCode doesn't match
                    Log.Warning("MerchantCode mismatch for MerchantId {MerchantId}. Expected MerchantCode: {MerchantCode}, Provided MerchantCode: {RequestMerchantCode}.",
                                request.MerchantId, merchant.MerchantCode, request.MerchantCode);
                    return BaseResponse<CreateMerchantBranchRes>.Error("The MerchantCode provided does not match the MerchantId.");
                }

                // Log the creation of the merchant branch
                Log.Information("Creating new MerchantBranch for MerchantId: {MerchantId}", request.MerchantId);

                // Create the new MerchantBranch entity
                var merchantBranch = new MerchantBranch
                {
                    BranchName = request.BranchName,
                    MerchantCode = request.MerchantCode,
                    MerchantId = request.MerchantId,
                    BranchEmail = request.BranchEmail,
                    ExteriorImages = request.ExteriorImages,
                    InteriorImages = request.InteriorImages,
                    ProvinceCode = request.ProvinceCode,
                    ProvinceName = request.ProvinceName,
                    CommuneCode = request.CommuneCode,
                    CommuneName = request.CommuneName,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    BranchAddress = request.BranchAddress
                };

                // Save the new MerchantBranch
                await _repository.AddAsync(merchantBranch, cancellationToken);
                await _repository.SaveChangesAsync(cancellationToken);

                // Log the successful creation of the branch
                Log.Information("MerchantBranch created successfully for MerchantId: {MerchantId}.", request.MerchantId);

                // Ensure the DTO is correctly mapped and not null
                var merchantBranchDto = _mapper.Map<CreateMerchantBranchRes>(merchantBranch);
                if (merchantBranchDto == null)
                {
                    Log.Error("Failed to map MerchantBranch entity to DTO.");
                    return BaseResponse<CreateMerchantBranchRes>.Error("Failed to map the entity to DTO.");
                }

                return BaseResponse<CreateMerchantBranchRes>.Success(merchantBranchDto);
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error response
                Log.Error(ex, "An unexpected error occurred while processing CreateMerchantBranchCommand for MerchantId: {MerchantId}", request.MerchantId);
                return BaseResponse<CreateMerchantBranchRes>.Error("An unexpected error occurred.");
            }
        }

    }
}

