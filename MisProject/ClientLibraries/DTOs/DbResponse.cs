using ClientLibraries.Enums;

namespace ClientLibraries.DTOs;

public class DbResponse<TType, TError>
{
    public DbResponse()
    {
#pragma warning disable CS8714
        Errors = new Dictionary<TError, string>();
#pragma warning restore CS8714
    }

    public bool Success { get; set; } = true;

    public TType? Value { get; set; } = default(TType);

    public IDictionary<TError, string> Errors { get; set; }

    public void AddError(TError key, string value)
    {
        Errors.Add(key, value);
        Success = false;
    }
}