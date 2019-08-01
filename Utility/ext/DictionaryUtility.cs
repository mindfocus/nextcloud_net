using System;
using System.Collections.Generic;
using System.Data;

namespace ext
{
    public static class DictionaryUtility
    {
        public static void AddRange<T, S>(this Dictionary<T, S> source, Dictionary<T, S> collection, bool duplicateKeyThrow=true)
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
    }
}