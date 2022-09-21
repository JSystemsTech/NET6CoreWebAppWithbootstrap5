using System.Collections.Specialized;

namespace NET6CoreWebAppWithbootstrap5.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool TryAdd(this HybridDictionary dictionary, object key, object? value)
        {
            try
            {
                dictionary.Add(key, value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool TryGet(this HybridDictionary dictionary, object key, out object? value)
        {
            try
            {
                value = dictionary[key];
                return true;
            }
            catch
            {
                value = null;
                return false;
            }
        }
        public static bool TrySet(this HybridDictionary dictionary, object key, object? value)
        {
            try
            {
                if (!dictionary.Contains(key))
                {
                    dictionary.TryAdd(key, value);
                }
                dictionary[key] = value;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
