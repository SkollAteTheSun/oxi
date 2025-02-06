using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using Kp.Ms.Oxi.Exceptions;
using Kp.Ms.Oxi.Models.Response;

namespace Kp.Ms.Oxi.Controllers;

public class BaseController : Controller
{
    protected readonly ILogger _logger;

    private IDictionary<string, object> _request;

    public BaseController(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null");
    }

    protected WebApiResponse<TResponse> ExecuteRequest<TRequest, TResponse>(TRequest? requestModel, Func<TResponse> requestHandler)
    {
        _logger.LogDebug($"{HttpContext.Request.Method} {HttpContext.Request.Path}{HttpContext.Request.QueryString}: {(requestModel != null ? JsonConvert.SerializeObject(requestModel) : "-")}");

        var result = requestHandler.Invoke();
        var response = new WebApiResponse<TResponse>()
        {
            Success = true,
            Response = result
        };

        return response;
    }

    protected async ValueTask<WebApiResponse<TResponse>> ExecuteRequestAsync<TRequest, TResponse>(TRequest? requestModel, Func<ValueTask<TResponse>> requestHandler)
    {
        _logger.LogDebug($"{HttpContext.Request.Method} {HttpContext.Request.Path}{HttpContext.Request.QueryString}: {(requestModel != null ? JsonConvert.SerializeObject(requestModel) : "-")}");

        var result = await requestHandler.Invoke();
        var response = new WebApiResponse<TResponse>()
        {
            Success = true,
            Response = result
        };

        return response;
    }

    private Dictionary<Type, HttpStatusCode> _typedErrors = new Dictionary<Type, HttpStatusCode>
        {
            { typeof(NotSupportedException), HttpStatusCode.NotImplemented},
            { typeof(ApiValidationException), HttpStatusCode.BadRequest},
            { typeof(ArgumentNullException), HttpStatusCode.BadRequest},
            { typeof(ArgumentException), HttpStatusCode.BadRequest},
            { typeof(NotFoundException), HttpStatusCode.NotFound},
            { typeof(Exception), HttpStatusCode.InternalServerError }
        };

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _request = context.ActionArguments;
        if (!context.ModelState.IsValid)
        {
            var errorContract = GetErrorResult<bool>(new ApiValidationException(ErrorsMessages.MODEL_IS_INVALID));
            context.Result = Json(errorContract);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            _logger.LogError($"Model is invalid: {JsonConvert.SerializeObject(context.ModelState.Values.SelectMany(value => value.Errors))}");

            var request = new
            {
                request = _request,
                context.HttpContext.Request.Path,
                context.HttpContext.Request.QueryString.Value
            };
            var response = errorContract;

            _logger.LogDebug(JsonConvert.SerializeObject(
                new { request, response }));
        }
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var request = new
        {
            request = _request,
            context.HttpContext.Request.Path,
            context.HttpContext.Request.QueryString.Value
        };
        if (context.Exception != null)
        {
            var hostEnv = (IWebHostEnvironment)context.HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment));
            var exception = context.Exception;

            var errorContract = GetErrorResult<bool>(exception, true);

            _logger.LogError(GetDevErrorMessage(context.Exception));

            context.HttpContext.Response.StatusCode = (int)GetTypedStatusCode(exception);
            context.Result = Json(errorContract);
            context.ExceptionHandled = true;

            _logger.LogError(JsonConvert.SerializeObject(
                new { request, errorContract }));
        }
    }

    private WebApiResponse<TResponse> GetErrorResult<TResponse>(Exception exception, bool createByLastInner = false)
    {
        var exceptionForContract = createByLastInner
            ? GetLastInner(exception)
            : exception;

        var existsCode = _typedErrors.TryGetValue(exception.GetType(), out var errorCode);
        var code = existsCode ? errorCode : HttpStatusCode.InternalServerError;

        var errorContract = new WebApiResponse<TResponse>()
        {
            Success = false,
            ErrorMessage = exception.Message
        };
        var typedMessage = exceptionForContract.Message;

        return errorContract;
    }

    private Exception GetLastInner(Exception exception)
    {
        var result = exception;
        while (result?.InnerException != null)
        {
            result = result.InnerException;
        }

        return result;
    }

    private HttpStatusCode GetTypedStatusCode(Exception exception)
    {
        var exceptionType = exception.GetType();

        return _typedErrors.ContainsKey(exceptionType)
            ? _typedErrors[exceptionType]
            : _typedErrors[typeof(Exception)];
    }

    private string GetDevErrorMessage(Exception exception)
    {
        var message = string.Empty;
        var inner = exception;
        while (inner != null)
        {
            message += $"{inner.Message}.\r\n";
            inner = inner.InnerException;
        }

        return message;
    }
}
