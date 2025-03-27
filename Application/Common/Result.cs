using Microsoft.AspNetCore.Identity;

namespace AuthControl.Application.Common;

public sealed class Result<T>
{
    public bool IsSuccess { get; }
    public string[]? Errors { get; }
    public T? Value { get; }
   
    private Result(bool isSuccess, T? value = default, string[]? errors = null)
    {
        IsSuccess = isSuccess;
        Value = value;
        Errors = errors;
    }
   
    public static Result<T> Success(T value) =>
        new Result<T>(true, value);
   
    public static Result<T> Success() =>
        new Result<T>(true);
   
    public static Result<T> Failure(params string[] errors) =>
        new Result<T>(false, default, errors.Length > 0 ? errors : null);
   
    public static Result<T> Failure(IEnumerable<IdentityError>? errors) =>
        new Result<T>(false, default, errors?.Select(e => e.Description).ToArray());
}