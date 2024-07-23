using backendfepon.ErrorsModels;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;


public class BaseController : ControllerBase
{
    protected ErrorResponse GenerateValidationErrorResponse(ValidationResult validationResult)
    {
        var errorResponse = new ErrorResponse
        {
            StatusCode = 400,
            Message = "Validation error"
        };

        foreach (var error in validationResult.Errors)
        {
            errorResponse.Errors.Add(new ErrorDetail
            {
                Field = error.PropertyName,
                Message = error.ErrorMessage
            });
        }

        return errorResponse;
    }

    protected ErrorResponse GenerateErrorResponse(int statusCode, string message, Exception ex)
    {
        return new ErrorResponse
        {
            StatusCode = statusCode,
            Message = message,
            Exception = ex.Message
        };
    }

    protected ErrorResponse GenerateErrorResponse(int statusCode, string message)
    {
        return new ErrorResponse
        {
            StatusCode = statusCode,
            Message = message,

        };
    }
}




