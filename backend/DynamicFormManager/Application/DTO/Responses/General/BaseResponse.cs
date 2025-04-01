using Application.DTO.Enums;

namespace Application.DTO.Responses.General;

public record BaseResponse
{
    private BaseResponse()
    {
    }

    public CustomStatusCodes StatusCode { get; init; }
    public bool IsSuccess => StatusCode is CustomStatusCodes.Ok or CustomStatusCodes.Created;

    public static implicit operator BaseResponse(CustomStatusCodes statusCode)
    {
        return new BaseResponse
        {
            StatusCode = statusCode
        };
    }
}

public record BaseResponse<TResponse>
{
    private BaseResponse()
    {
    }

    public required CustomStatusCodes StatusCode { get; init; }
    public bool IsSuccess => StatusCode == CustomStatusCodes.Ok;
    public TResponse? Response { get; init; }

    public static implicit operator BaseResponse<TResponse>(TResponse response)
    {
        return new BaseResponse<TResponse>
        {
            StatusCode = CustomStatusCodes.Ok,
            Response = response
        };
    }

    public static implicit operator BaseResponse<TResponse>(CustomStatusCodes statusCode)
    {
        return new BaseResponse<TResponse>
        {
            StatusCode = statusCode
        };
    }
}