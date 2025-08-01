using Applications.Features.Notifications.Commands;
using Applications.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Notifications.Handlers
{
    public class ResetOtpRequestCounterCommandHandler : IRequestHandler<ResetOtpRequestCounterCommand, BaseResponse<bool>>
    {
        private readonly IOtpRequestCounterRepository _otpCounterRepo;

        public ResetOtpRequestCounterCommandHandler(IOtpRequestCounterRepository otpCounterRepo)
        {
            _otpCounterRepo = otpCounterRepo;
        }

        public async Task<BaseResponse<bool>> Handle(ResetOtpRequestCounterCommand request, CancellationToken cancellationToken)
        {
            var record = await _otpCounterRepo.Query()
                .FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber && x.Purpose == request.Purpose, cancellationToken);

            if (record == null)
                return BaseResponse<bool>.Error("Không tìm thấy bộ đếm OTP tương ứng.");

            record.Count = 0;
            record.LastResetAt = DateTime.UtcNow;
            _otpCounterRepo.Update(record);
            await _otpCounterRepo.SaveChangesAsync(cancellationToken);

            return BaseResponse<bool>.Success(true, "Đã reset thành công bộ đếm OTP.");
        }
    }

}
