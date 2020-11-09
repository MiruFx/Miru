namespace Miru.Foundation
{
    public interface IJsonConverter
    {
        T Deserialize<T>(string json);
        
        string Serialize<T>(T cart);
    }
}