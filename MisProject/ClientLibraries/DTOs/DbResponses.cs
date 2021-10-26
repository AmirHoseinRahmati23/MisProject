using ClientLibraries.Enums;

namespace ClientLibraries.DTOs;

public class DbResponses<TType, TError>
{
    public bool Success { get; set; } = true;

    public TType? Value { get; set; } = default(TType);

    public IDictionary<TError, string> Errors => new Dictionary<TError, string>();

    public void AddError(TError key, string value)
    {
        Errors.Add(key, value);
        Success = false;
    }
}