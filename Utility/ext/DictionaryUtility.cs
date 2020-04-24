using System;
using System.Collections.Generic;
using System.Data;

namespace ext
{
    public static class DictionaryUtility
    {
        public static void AddRange<T, S>(this IDictionary<T, S> source, IDictionary<T, S> collection, bool duplicateKeyThrow=true)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("Collection is null");
            }

            foreach (var item in collection)
            {
                if(!source.ContainsKey(item.Key)){
                    source.Add(item.Key, item.Value);
                }
                else
                {
                    if (duplicateKeyThrow)
                    {
                        throw new DuplicateNameException("Key 重复");
                    }
                }
            }
        }

        public static IDictionary<T, S> AddOrMerge<T, S>(this IDictionary<T, S> source, T key, S value)
        {
            if (source.ContainsKey(key))
            {
                source[key] = value;
            }
            else
            {
                source.Add(key, value);
            }

            return source;
        }
        public static IDictionary<T, S> AddOrMergeRange<T, S>(this IDictionary<T, S> source, IDictionary<T, S> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("Collection is null");
            }

            foreach (var tsPair in collection)
            {
                source.AddOrMerge(tsPair.Key, tsPair.Value);
            }
            
            return source;
        }
    }
}