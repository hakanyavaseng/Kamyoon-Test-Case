namespace ProductManagement.Core.DTOs.ApiResponses;

public record ErrorDto
{
    /// <summary>
    ///     Creates an instance of ErrorDto with a single error message.
    /// </summary>
    /// <param name="error">Error message</param>
    /// <param name="isShow">Will the message be shown</param>
    public ErrorDto(string error, bool isShow = true)
    {
        Errors = new List<string> { error };
        IsShow = isShow;
    }

    /// <summary>
    ///     Creates an instance of ErrorDto with a list of error messages.
    /// </summary>
    /// <param name="error">Error messages</param>
    /// <param name="isShow">Will the message be shown</param>
    public ErrorDto(List<string> errors, bool isShow = true)
    {
        Errors = errors;
        IsShow = isShow;
    }

    /// <summary>
    ///     Error messages
    /// </summary>
    public List<string> Errors { get; init; }

    /// <summary>
    ///     Will the message be shown to the user
    /// </summary>
    public bool IsShow { get; init; }
}