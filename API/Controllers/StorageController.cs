using Applications.Features.FileUpload.Commands;
using Applications.Features.FileUpload.DTOs;
using Applications.Features.FileUpload.Queries;
using Core.Enumerables;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Results;

namespace API.Controllers;

[ApiController]
[Route("api/files")]
//[Authorize(Roles = $"{RoleEnum.AdminDashboard}, {RoleEnum.MobileShopManager}")]
public class FileController : ControllerBase
{
    private readonly IMediator _mediator;

    public FileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Upload file lên AWS S3
    /// </summary>
    /// <param name="file">File cần upload</param>
    /// <returns>URL hoặc tên file sau khi upload</returns>
    [HttpPost("upload")]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(BaseResponse<string>.Error("File không hợp lệ."));
        }

        var result = await _mediator.Send(new UploadFileCommand(file));

        return Ok(BaseResponse<UploadFileResponse>.Success(result, "Upload thành công"));

    }
    [HttpPost("upload-multiple")]
    [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadMultiple([FromForm] List<IFormFile> files)
    {
        if (files == null || !files.Any())
            return BadRequest(BaseResponse<string>.Error("Không có file nào được gửi lên."));

        var result = await _mediator.Send(new UploadMultipleFilesCommand(files));
        return Ok(BaseResponse<List<UploadFileResponse>>.Success(result, "Upload nhiều file thành công."));

    }
    /// <summary>
    /// Lấy signed URL từ fileKey
    /// </summary>
    [HttpGet("url")]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFileUrl([FromQuery] string fileKey)
    {
        if (string.IsNullOrWhiteSpace(fileKey))
        {
            return BadRequest(BaseResponse<string>.Error("Thiếu fileKey"));
        }

        var result = await _mediator.Send(new GetFileUrlQuery { FileKey = fileKey });
        return Ok(BaseResponse<string>.Success(result, "Lấy URL thành công"));
    }

    /// <summary>
    /// API upload media từ Zalo Mini App (openMediaPicker).
    /// Nhận file từ multipart/form-data với field name = "file".
    /// Trả về JSON theo định dạng Zalo yêu cầu.
    /// </summary>
    /// <remarks>
    /// SDK Zalo sẽ truyền lên theo multipart/form-data.
    /// Response JSON: 
    /// {
    ///   "error": 0,
    ///   "message": "Success",
    ///   "data": {
    ///     "urls": [ ... ]
    ///   }
    /// }
    /// </remarks>
    [HttpPost("upload-zalo")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadFromZalo([FromForm] IFormFileCollection file)
    {
        var result = await _mediator.Send(new UploadZaloMediaCommand { Files = file.ToList() });

        return StatusCode(Convert.ToInt32( result.Code) == 0 ? 200 : 400, new
        {
            error = result.Code,
            message = result.Message,
            data = result.Data == null ? null : new { result.Data.Urls }
        });
    }
}



