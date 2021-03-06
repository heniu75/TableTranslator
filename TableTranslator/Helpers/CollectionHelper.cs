using System;
using System.Collections.Generic;
using System.Linq;
using TableTranslator.Engines;

namespace TableTranslator.Helpers
{
    internal static class CollectionHelper
    {
        /// <summary>
        /// Removes all items from the dictionary using the provided predicate
        /// </summary>
        /// <typeparam name="TKey">Type of the key</typeparam>
        /// <typeparam name="TValue">Type of the value</typeparam>
        /// <param name="dictionary">Dictionary to perform removal on</param>
        /// <param name="predicate">Predicate for which items to remove from the dictionary</param>
        internal static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<TKey, TValue, bool> predicate)
        {
            var keys = dictionary.Keys.Where(k => predicate(k, dictionary[k])).ToList();
            foreach (var key in keys)
            {
                dictionary.Remove(key);
            }
        }

        /// <summary>
        /// Simple way to get a particular engine for a list of translation engines
        /// </summary>
        /// <typeparam name="TEngine">Type of engine you want</typeparam>
        /// <param name="engines"></param>
        /// <returns></returns>
        internal static TEngine GetEngine<TEngine>(this IEnumerable<TranslationEngine> engines) where TEngine : TranslationEngine
        {
            return engines.FirstOrDefault(x => x.GetType() == typeof(TEngine)) as TEngine;
        }
    }
}