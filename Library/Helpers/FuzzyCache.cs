using System.Collections.Generic;
using System.Linq;

namespace Pokepanion.Library.Helpers;

public abstract class FuzzyCache<K, V> where K : notnull {

    protected readonly Dictionary<K, V> cache;

    protected FuzzyCache(IEnumerable<KeyValuePair<K, V>> initialValues) {
        cache = initialValues.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    /// <summary>
    /// Determines how good of a match <paramref name="actualKey" /> is for
    /// <paramref name="desiredKey" />.
    /// </summary>
    /// <param name="desiredKey">The key given by the user.</param>
    /// <param name="actualKey">The key in the dictionary to test.</param>
    /// <returns>A float between 0 and 1 representing how close <paramref name="actualKey" />
    /// is to <paramref name="desiredKey" />. Higher is better.</returns>
    public abstract float GetKeyConfidence(K desiredKey, K actualKey);

    /// <summary>
    /// Gets the value associated with the key that has the best (greatest) score
    /// based on <see cref="GetKeyConfidence" />.
    /// </summary>
    /// <param name="key">The key to access.</param>
    /// <returns>The value of the key with the best score for <paramref name="key" />.</returns>
    public V GetBest(K key, out float confidence) {
        if (cache.TryGetValue(key, out V? value)) {
            confidence = 1.0f;
            return value;
        }

        var bestMatch = cache
                        .Select(kvp => (value: kvp.Value, score: GetKeyConfidence(key, kvp.Key)))
                        .MaxBy((keyScorePair) => keyScorePair.score);

        confidence = bestMatch.score;
        return bestMatch.value;
    }

    /// <summary>
    /// Gets the value associated with the given key. No key-guessing occurs.
    /// </summary>
    /// <param name="key">The key to get.</param>
    /// <returns>The value associated with the key.</returns>
    public V GetExact(K key) => cache[key];

    /// <summary>
    /// Adds the given key and value to the cache.
    /// </summary>
    /// <param name="key">The key to add.</param>
    /// <param name="value">The value to associate with the given key.</param>
    public void Add(K key, V value) => cache.Add(key, value);
}
