using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MultiInput.Internal
{
    // code from: https://stackoverflow.com/questions/35734143/converting-dictionarytkey-listtvalue-to-readonlydictionarytkey-readonlyco
    
    /// <summary>
    /// Provides a <see cref="AsReadOnly{TKey, TValue, TReadOnlyValue}(IDictionary{TKey, TValue})"/>
    /// method on any generic dictionary.
    /// </summary>
    public static class DictionaryExtension
    {
        class ReadOnlyDictionaryWrapper<TKey, TValue, TReadOnlyValue> : IReadOnlyDictionary<TKey, TReadOnlyValue>
            where TValue : TReadOnlyValue
            where TKey : notnull
        {
            private IReadOnlyDictionary<TKey, TValue> _dictionary;

            public ReadOnlyDictionaryWrapper(IReadOnlyDictionary<TKey, TValue> dictionary)
            {
                if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
                _dictionary = dictionary;
            }

            public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

            public IEnumerable<TKey> Keys => _dictionary.Keys;

            public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TReadOnlyValue value)
            {
                var r = _dictionary.TryGetValue(key, out var v);
                value = v!;
                return r;
            }

            public IEnumerable<TReadOnlyValue> Values => _dictionary.Values.Cast<TReadOnlyValue>();

            public TReadOnlyValue this[TKey key] => _dictionary[key];

            public int Count => _dictionary.Count;

            public IEnumerator<KeyValuePair<TKey, TReadOnlyValue>> GetEnumerator() => _dictionary
                .Select(x => new KeyValuePair<TKey, TReadOnlyValue>(x.Key, x.Value)).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        /// <summary>
        /// Creates a wrapper on a dictionary that adapts the type of the values.
        /// </summary>
        /// <typeparam name="TKey">The dictionary key.</typeparam>
        /// <typeparam name="TValue">The dictionary value.</typeparam>
        /// <typeparam name="TReadOnlyValue">The base type of the <typeparamref name="TValue"/>.</typeparam>
        /// <param name="this">This dictionary.</param>
        /// <returns>A dictionary where values are a base type of this dictionary.</returns>
        public static IReadOnlyDictionary<TKey, TReadOnlyValue> AsReadOnly<TKey, TValue, TReadOnlyValue>(
            this IReadOnlyDictionary<TKey, TValue> @this)
            where TValue : TReadOnlyValue
            where TKey : notnull
        {
            return new ReadOnlyDictionaryWrapper<TKey, TValue, TReadOnlyValue>(@this);
        }
    }
}