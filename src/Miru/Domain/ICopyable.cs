namespace Miru.Domain
{
    public interface ICopyable<T>
    {
        T Copy();
    }
}