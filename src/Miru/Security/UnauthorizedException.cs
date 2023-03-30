namespace Miru.Security;

public class UnauthorizedException : Exception
{
    public object FeatureResult { get; }

    public UnauthorizedException(
        string message = null, 
        object featureResult = null) : base(message)
    {
        FeatureResult = featureResult;
    }
}