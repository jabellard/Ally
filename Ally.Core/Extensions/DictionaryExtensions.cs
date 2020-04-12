using System.Collections.Generic;
using System.Collections.Specialized;

namespace Ally.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static NameValueCollection ToNameValueCollection<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            var nameValueCollection = new NameValueCollection();
            foreach (var pair in dictionary)
            {
                string value = null;
                if (pair.Value != null)
                    value = pair.Value.ToString();
                nameValueCollection.Add(pair.Key.ToString(), value);
            }
            return nameValueCollection;
        }
    }
}