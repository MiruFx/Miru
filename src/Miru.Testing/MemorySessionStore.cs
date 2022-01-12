using System.Collections.Generic;

namespace Miru.Testing
{
    public class MemorySessionStore : ISessionStore
    {
        private readonly Dictionary<string, string> _store = new Dictionary<string, string>();

        public bool ContainsKey(string key)
        {
            return _store.ContainsKey(key);
        }

        public string GetString(string key)
        {
            if (_store.TryGetValue(key, out var value))
                return value;

            return string.Empty;
        }

        public void SetString(string key, string value)
        {
            _store.AddOrUpdate(key, value);
        }
    }
}