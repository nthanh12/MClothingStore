using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Consts.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.WebAPIBase
{
    public class ApiControllerBase : ControllerBase
    {
        protected ILogger _logger;
        public ApiControllerBase(ILogger logger)
        {
            _logger = logger;
        }

        [NonAction]
        public ApiResponse OkException(Exception ex)
        {
            var errorMessage = ex.Message.ToString();
            var errorCode = ErrorCode.GetErrorCode(errorMessage);
            if (errorCode.ToString() != null || errorCode == 0)
            {
                return new ApiResponse(
                        WebAPIBase.StatusCode.Error,
                        null,
                        errorCode,
                        errorMessage
                );
            }
            else
            {
                _logger?.LogError(ex, $"{ex.GetType()}: {ex}, ErrorCode = {ErrorCode.InternalServerError}, Message: {ErrorMessage.InternalServerError}");
            }
            return new ApiResponse(
                WebAPIBase.StatusCode.Error,
                null,
                ErrorCode.InternalServerError,
                ErrorMessage.InternalServerError
                );
        }
    }
}
