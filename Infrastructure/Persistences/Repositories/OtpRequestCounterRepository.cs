using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using Shared.Results;

namespace Infrastructure.Persistences.Repositories;

/// <summary>
/// Repository xử lý giới hạn số lần gửi OTP trong khoảng thời gian.
/// </summary>
public class OtpRequestCounterRepository : BaseRepository<OtpRequestCounter>, IOtpRequestCounterRepository
{
    private readonly ILogger<OtpRequestCounterRepository> _logger;

    public OtpRequestCounterRepository(
        AppDbContext context,
        IMapper mapper,
        ICurrentUserService currentUser,
        ILogger<BaseRepository<OtpRequestCounter>> baseLogger,
        ILogger<OtpRequestCounterRepository> logger)
        : base(context, mapper, currentUser, baseLogger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Lấy bộ đếm OTP theo số điện thoại và mục đích.
    /// </summary>
    public async Task<OtpRequestCounter?> GetAsync(string phoneNumber, string purpose, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("🔍 Tìm OtpRequestCounter với Phone: {Phone}, Purpose: {Purpose}", phoneNumber, purpose);
        return await _dbSet
            .FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber && x.Purpose == purpose && !x.IsDeleted, cancellationToken);
    }

    /// <summary>
    /// Tăng bộ đếm nếu chưa vượt giới hạn.
    /// Nếu vượt giới hạn thì trả về false.
    /// </summary>
    public async Task<bool> TryIncreaseCountAsync(string phoneNumber, string purpose, int maxPerDay = 3, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var record = await _dbSet.FirstOrDefaultAsync(
            x => x.PhoneNumber == phoneNumber && x.Purpose == purpose && !x.IsDeleted, cancellationToken);

        if (record == null)
        {
            _logger.LogInformation("➕ Tạo mới OtpRequestCounter cho {Phone}, Purpose: {Purpose}", phoneNumber, purpose);
            var newCounter = new OtpRequestCounter
            {
                PhoneNumber = phoneNumber,
                Purpose = purpose,
                Count = 1,
                LastResetAt = now
            };
            await AddAsync(newCounter, cancellationToken);
            return true;
        }

        // Nếu đã hơn 24h kể từ lần reset, reset lại
        if ((now - record.LastResetAt).TotalHours >= 24)
        {
            _logger.LogInformation("🔁 Reset OtpRequestCounter cho {Phone} (quá 24h)", phoneNumber);
            record.Count = 1;
            record.LastResetAt = now;
            await UpdateAsync(record, cancellationToken);
            return true;
        }

        // Nếu chưa quá 24h nhưng vượt giới hạn
        if (record.Count >= maxPerDay)
        {
            _logger.LogWarning("⚠️ Vượt giới hạn gửi OTP trong 24h. Phone: {Phone}, Count: {Count}", phoneNumber, record.Count);
            return false;
        }

        // Tăng đếm
        record.Count++;
        await UpdateAsync(record, cancellationToken);
        _logger.LogInformation("🔢 Tăng bộ đếm OTP. Phone: {Phone}, Count: {Count}", phoneNumber, record.Count);
        return true;
    }

    /// <summary>
    /// Reset thủ công bộ đếm OTP (dùng bởi Admin).
    /// </summary>
    public async Task<BaseResponse<bool>> ResetCounterAsync(string phoneNumber, string purpose, CancellationToken cancellationToken = default)
    {
        var record = await _dbSet.FirstOrDefaultAsync(
            x => x.PhoneNumber == phoneNumber && x.Purpose == purpose && !x.IsDeleted, cancellationToken);

        if (record == null)
        {
            _logger.LogWarning("❌ Không tìm thấy OtpRequestCounter để reset. Phone: {Phone}, Purpose: {Purpose}", phoneNumber, purpose);
            return BaseResponse<bool>.Error("Không tìm thấy bộ đếm OTP để reset.");
        }

        record.Count = 0;
        record.LastResetAt = DateTime.UtcNow;
        await UpdateAsync(record, cancellationToken);

        _logger.LogInformation("✅ Đã reset bộ đếm OTP cho {Phone}, Purpose: {Purpose}", phoneNumber, purpose);
        return BaseResponse<bool>.Success(true, "Reset bộ đếm thành công");
    }

    /// <summary>
    /// Thêm mới hoặc cập nhật bộ đếm (tăng số lần).
    /// </summary>
    public async Task AddOrUpdateAsync(OtpRequestCounter entity, CancellationToken cancellationToken = default)
    {
        var existing = await GetAsync(entity.PhoneNumber, entity.Purpose, cancellationToken);
        if (existing == null)
        {
            _logger.LogInformation("➕ Add mới bộ đếm OTP cho Phone: {Phone}, Purpose: {Purpose}", entity.PhoneNumber, entity.Purpose);
            await AddAsync(entity, cancellationToken);
        }
        else
        {
            _logger.LogInformation("✏️ Cập nhật bộ đếm OTP cho Phone: {Phone}, Count: {Count}", entity.PhoneNumber, entity.Count);
            existing.Count = entity.Count;
            existing.LastResetAt = entity.LastResetAt;
            await UpdateAsync(existing, cancellationToken);
        }
    }
}
