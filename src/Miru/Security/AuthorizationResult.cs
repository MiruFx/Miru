namespace Miru.Security;

public class AuthorizationResult
{
    private static readonly AuthorizationResult SuccessResult = new(true, null);
    
    public bool IsAuthorized { get; }
    public string FailureMessage { get; }

    public AuthorizationResult()
    {
    }
        
    private AuthorizationResult(bool isAuthorized, string failureMessage)
    {
        IsAuthorized = isAuthorized;
        FailureMessage = failureMessage;
    }
        
    public static AuthorizationResult Fail()
    {
        return new AuthorizationResult(false, null);
    }

    public static AuthorizationResult Fail(string failureMessage)
    {
        return new AuthorizationResult(false, failureMessage);
    }

    public static AuthorizationResult Succeed() => SuccessResult;
}