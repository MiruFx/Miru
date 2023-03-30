namespace Miru.Security;

public class AuthorizationResult
{
    private static readonly AuthorizationResult SuccessResult = new(true, null);
    
    public bool IsAuthorized { get; }
    public string FailureMessage { get; }
    public object FeatureResult { get; }

    public AuthorizationResult()
    {
    }
        
    public AuthorizationResult(
        bool isAuthorized, 
        string failureMessage, 
        object featureResult = null)
    {
        IsAuthorized = isAuthorized;
        FailureMessage = failureMessage;
        FeatureResult = featureResult;
    }
        
    public static AuthorizationResult Fail()
    {
        return new AuthorizationResult(false, null);
    }

    public static AuthorizationResult Fail(string failureMessage, object featureResult = null)
    {
        return new AuthorizationResult(false, failureMessage, featureResult);
    }

    public static AuthorizationResult Succeed() => SuccessResult;
}