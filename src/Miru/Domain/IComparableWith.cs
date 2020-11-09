namespace Miru.Domain
{
    public interface IComparableWith<in TOther> where TOther : class
    {
        bool IsSameAs(TOther other);
    }
}