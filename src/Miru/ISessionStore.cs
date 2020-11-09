namespace Miru
{
    public interface ISessionStore
    {
        string GetString(string key);
        
        void SetString(string key, string value);
    }
}