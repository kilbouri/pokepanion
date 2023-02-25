using System.Collections.Generic;
using Pokepanion.Library.Pokedex;

namespace Pokepanion.Library.Uranium;

public class UraniumPokedex : BasePokedex<UraniumPokemonInfo, UraniumType, UraniumEffectiveness> {
    public UraniumPokedex(IEnumerable<UraniumPokemonInfo> initialValues) : base(initialValues) { }
}
