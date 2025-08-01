using Microsoft.AspNetCore.Http;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Services.Interfaces
{
    public interface IAdministrativeDataService
    {
        Task<BaseResponse<object>> ImportDataFromJsonFileAsync(IFormFile file);
    }
}
