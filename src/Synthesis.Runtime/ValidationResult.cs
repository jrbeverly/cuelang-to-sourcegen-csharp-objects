namespace Synthesis.Runtime;

public sealed class ValidationResult
{
    public static readonly ValidationResult Ok = new(true, null);
    public bool IsValid { get; }
    public string? Error { get; }
    private ValidationResult(bool isValid, string? error) { IsValid = isValid; Error = error; }
    public static ValidationResult Fail(string error) => new(false, error);
}
