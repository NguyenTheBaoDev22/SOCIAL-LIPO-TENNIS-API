using Core.Enumerables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Results;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize(Roles = RoleEnum.AdminDashboard)]
    public class DeviceController : ControllerBase
    {
        /// <summary>
        /// Lấy danh sách các loại thiết bị hợp lệ.
        /// </summary>
        [HttpGet("types")]
        public ActionResult<BaseResponse<IEnumerable<DeviceTypeResponse>>> GetDeviceTypes()
        {
            // Lấy danh sách các loại thiết bị hợp lệ với mã và tên
            var deviceTypes = DeviceTypeConstants.GetAllWithNames()
                .Select(device => new DeviceTypeResponse
                {
                    DeviceTypeCode = device.Key,
                    DeviceTypeName = device.Value
                });

            return Ok(BaseResponse<IEnumerable<DeviceTypeResponse>>.Success(deviceTypes.ToList()));
        }
    

    /// <summary>
    /// Lấy tên hiển thị của thiết bị theo mã loại thiết bị.
    /// </summary>
    [HttpGet("name/{deviceType}")]
        public ActionResult<BaseResponse<string>> GetDeviceName(string deviceType)
        {
            if (!DeviceTypeConstants.IsValid(deviceType))
            {
                return BadRequest(BaseResponse<string>.Error("Loại thiết bị không hợp lệ.", ErrorCodes.Merchant_InvalidCategoryCode));
            }

            var deviceName = DeviceTypeConstants.GetName(deviceType);
            return Ok(BaseResponse<string>.Success(deviceName));
        }

        /// <summary>
        /// Lấy tất cả các loại thiết bị với tên hiển thị.
        /// </summary>
        [HttpGet("all")]
        public ActionResult<BaseResponse<Dictionary<string, string>>> GetAllDevices()
        {
            // Lấy tất cả các thiết bị với tên hiển thị
            var allDevices = DeviceTypeConstants.GetAllWithNames();

            // Chuyển đổi IReadOnlyDictionary thành Dictionary
            var devicesAsDictionary = allDevices.ToDictionary(entry => entry.Key, entry => entry.Value);

            return Ok(BaseResponse<Dictionary<string, string>>.Success(devicesAsDictionary));
        }

    }
    // Lớp trả về thông tin loại thiết bị
    public class DeviceTypeResponse
    {
        public string DeviceTypeCode { get; set; }
        public string DeviceTypeName { get; set; }
    }
}
