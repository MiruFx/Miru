namespace Miru;

public interface ISessionStore
{
    bool ContainsKey(string key);
        
    string GetString(string key);
        
    void SetString(string key, string value);

    void RemoveString(string key);
}