using System;
using System.Collections.Generic;

namespace Pokepanion.Library.Pokedex;

public interface IPokemonInfo<TType, TEffectiveness>
    where TType : struct, Enum
    where TEffectiveness : struct, Enum {

    uint Id { get; init; }
    string Name { get; init; }
    TType PrimaryType { get; init; }
    TType? SecondaryType { get; init; }

    Dictionary<TEffectiveness, TType[]> TypeEffectivenesses { get; init; }

}
