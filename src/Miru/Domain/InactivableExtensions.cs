namespace Miru.Domain;

public static class InactivableExtensions
{
    public static bool IsActive(this IInactivable inactivable) => 
        inactivable.IsInactive == false;

    public static void ActivateOrInactivate(this IInactivable inactivable) => 
        inactivable.IsInactive = !inactivable.IsInactive;

    public static void Inactivate(this IInactivable inactivable) =>
        inactivable.IsInactive = true;
}