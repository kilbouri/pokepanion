using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySharp;
using Pokepanion.Library.Helpers;

namespace Pokepanion.Library.Pokedex;

public abstract class BasePokedex<TPokemonInfo, TType, TEffectivness> : FuzzyCache<string, TPokemonInfo>
    where TPokemonInfo : IPokemonInfo<TType, TEffectivness>
    where TType : struct, Enum
    where TEffectivness : struct, Enum {

    public BasePokedex(IEnumerable<TPokemonInfo> initialValues)
        : base(initialValues.Select(info => new KeyValuePair<string, TPokemonInfo>(info.Name, info))) { }

    public sealed override float GetKeyConfidence(string desiredKey, string actualKey) {
        float closeness = Fuzz.WeightedRatio(actualKey, desiredKey) * 0.01f;
        float firstLetter = desiredKey[0] == actualKey[0] ? 1.0f : 0.0f;
        float length = desiredKey.Length == actualKey.Length ? 1.0f : 0.0f;

        return (0.85f * closeness) + (0.10f * firstLetter) + (0.05f * length);
    }
}
