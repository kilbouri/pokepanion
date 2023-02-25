using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Pokepanion.Library.Pokedex;

namespace Pokepanion.Library.Uranium;

[JsonConverter(typeof(StringEnumConverter))]
public enum UraniumType {
    Bug, Dark, Dragon, Electric, Fairy,
    Fighting, Fire, Flying, Ghost, Grass,
    Ground, Ice, Normal, Nuclear, Poison,
    Psychic, Rock, Steel, Water
}

/// <remark>
/// The integers are chosen such that the value divided by 100 gives the damage
/// multiplier the effectiveness applies. Ie, DoubleResisted = 25 => a pokemon
/// with DoubleResisted takes 25% damage or damage * 0.25.
/// </remark>
[JsonConverter(typeof(StringEnumConverter))]
public enum UraniumEffectiveness {
    Immune = 0,
    DoubleResisted = 25,
    Resisted = 50,
    Normal = 100,
    Weak = 200,
    SuperWeak = 400
}

public readonly struct UraniumPokemonInfo : IPokemonInfo<UraniumType, UraniumEffectiveness> {
    public uint Id { get; init; }
    public string Name { get; init; }
    public UraniumType PrimaryType { get; init; }
    public UraniumType? SecondaryType { get; init; }

    public Dictionary<UraniumEffectiveness, UraniumType[]> TypeEffectivenesses { get; init; }
}

public static class UraniumTypeHelper {

    /// <summary>
    /// Determines the effectiveness of an attack of type <paramref name="attacking" /> against 
    /// a pokemon whose only type is <paramref name="defending" />.
    /// </summary>
    /// <param name="attacking">The type of the attacking move</param>
    /// <param name="defending">The type of the defending pokemon</param>
    /// <returns>The effectiveness of the move against the defender</returns>
    public static UraniumEffectiveness GetTypeEffectiveness(UraniumType attacking, UraniumType defending)
        => (attacking, defending) switch {
            // <defending type> is <effectiveness> against <attacking type>
            (UraniumType.Normal, UraniumType.Nuclear) => UraniumEffectiveness.Weak,
            (UraniumType.Normal, UraniumType.Rock or UraniumType.Steel) => UraniumEffectiveness.Resisted,
            (UraniumType.Normal, UraniumType.Ghost) => UraniumEffectiveness.Immune,

            (UraniumType.Fire, UraniumType.Bug or UraniumType.Grass or UraniumType.Ice or UraniumType.Steel or UraniumType.Nuclear) => UraniumEffectiveness.Weak,
            (UraniumType.Fire, UraniumType.Dragon or UraniumType.Fire or UraniumType.Rock or UraniumType.Water) => UraniumEffectiveness.Resisted,

            (UraniumType.Fighting, UraniumType.Dark or UraniumType.Ice or UraniumType.Normal or UraniumType.Rock or UraniumType.Steel or UraniumType.Nuclear) => UraniumEffectiveness.Weak,
            (UraniumType.Fighting, UraniumType.Bug or UraniumType.Flying or UraniumType.Poison or UraniumType.Psychic or UraniumType.Fairy) => UraniumEffectiveness.Resisted,
            (UraniumType.Fighting, UraniumType.Ghost) => UraniumEffectiveness.Immune,

            (UraniumType.Water, UraniumType.Fire or UraniumType.Fire or UraniumType.Rock or UraniumType.Nuclear) => UraniumEffectiveness.Weak,
            (UraniumType.Water, UraniumType.Dragon or UraniumType.Grass or UraniumType.Water) => UraniumEffectiveness.Resisted,

            (UraniumType.Flying, UraniumType.Bug or UraniumType.Fighting or UraniumType.Grass or UraniumType.Nuclear) => UraniumEffectiveness.Weak,
            (UraniumType.Flying, UraniumType.Electric or UraniumType.Rock or UraniumType.Steel) => UraniumEffectiveness.Resisted,

            (UraniumType.Grass, UraniumType.Ground or UraniumType.Rock or UraniumType.Water or UraniumType.Nuclear) => UraniumEffectiveness.Weak,
            (UraniumType.Grass, UraniumType.Bug or UraniumType.Dragon or UraniumType.Fire or UraniumType.Flying or UraniumType.Grass or UraniumType.Poison or UraniumType.Steel) => UraniumEffectiveness.Resisted,

            (UraniumType.Poison, UraniumType.Grass or UraniumType.Fairy or UraniumType.Nuclear) => UraniumEffectiveness.Weak,
            (UraniumType.Poison, UraniumType.Ghost or UraniumType.Ground or UraniumType.Poison or UraniumType.Rock) => UraniumEffectiveness.Resisted,
            (UraniumType.Poison, UraniumType.Steel) => UraniumEffectiveness.Immune,

            (UraniumType.Electric, UraniumType.Flying or UraniumType.Water or UraniumType.Nuclear) => UraniumEffectiveness.Weak,
            (UraniumType.Electric, UraniumType.Dragon or UraniumType.Electric or UraniumType.Grass) => UraniumEffectiveness.Resisted,
            (UraniumType.Electric, UraniumType.Ground) => UraniumEffectiveness.Immune,

            (UraniumType.Ground, UraniumType.Electric or UraniumType.Fire or UraniumType.Poison or UraniumType.Rock or UraniumType.Steel or UraniumType.Nuclear) => UraniumEffectiveness.Weak,
            (UraniumType.Ground, UraniumType.Bug or UraniumType.Grass) => UraniumEffectiveness.Resisted,
            (UraniumType.Ground, UraniumType.Flying) => UraniumEffectiveness.Immune,

            (UraniumType.Psychic, UraniumType.Fighting or UraniumType.Poison or UraniumType.Nuclear) => UraniumEffectiveness.Weak,
            (UraniumType.Psychic, UraniumType.Psychic or UraniumType.Steel) => UraniumEffectiveness.Resisted,
            (UraniumType.Psychic, UraniumType.Dark) => UraniumEffectiveness.Immune,

            (UraniumType.Rock, UraniumType.Bug or UraniumType.Fire or UraniumType.Flying or UraniumType.Ice or UraniumType.Nuclear) => UraniumEffectiveness.Weak,
            (UraniumType.Rock, UraniumType.Fighting or UraniumType.Ground or UraniumType.Steel) => UraniumEffectiveness.Resisted,

            (UraniumType.Ice, UraniumType.Dragon or UraniumType.Flying or UraniumType.Grass or UraniumType.Ground or UraniumType.Nuclear) => UraniumEffectiveness.Weak,
            (UraniumType.Ice, UraniumType.Fire or UraniumType.Ice or UraniumType.Steel or UraniumType.Water) => UraniumEffectiveness.Resisted,

            (UraniumType.Bug, UraniumType.Dark or UraniumType.Grass or UraniumType.Psychic or UraniumType.Nuclear) => UraniumEffectiveness.Weak,
            (UraniumType.Bug, UraniumType.Fighting or UraniumType.Fire or UraniumType.Flying or UraniumType.Ghost or UraniumType.Poison or UraniumType.Steel or UraniumType.Fairy) => UraniumEffectiveness.Resisted,

            (UraniumType.Dragon, UraniumType.Dragon or UraniumType.Nuclear) => UraniumEffectiveness.Weak,
            (UraniumType.Dragon, UraniumType.Steel) => UraniumEffectiveness.Resisted,
            (UraniumType.Dragon, UraniumType.Fairy) => UraniumEffectiveness.Immune,

            (UraniumType.Ghost, UraniumType.Ghost or UraniumType.Psychic or UraniumType.Nuclear) => UraniumEffectiveness.Weak,
            (UraniumType.Ghost, UraniumType.Dark) => UraniumEffectiveness.Resisted,
            (UraniumType.Ghost, UraniumType.Normal) => UraniumEffectiveness.Immune,

            (UraniumType.Dark, UraniumType.Ghost or UraniumType.Psychic or UraniumType.Nuclear) => UraniumEffectiveness.Weak,
            (UraniumType.Dark, UraniumType.Dark or UraniumType.Fighting or UraniumType.Fairy) => UraniumEffectiveness.Resisted,

            (UraniumType.Steel, UraniumType.Ice or UraniumType.Rock or UraniumType.Fairy or UraniumType.Nuclear) => UraniumEffectiveness.Weak,
            (UraniumType.Steel, UraniumType.Electric or UraniumType.Fire or UraniumType.Steel or UraniumType.Water) => UraniumEffectiveness.Resisted,

            (UraniumType.Fairy, UraniumType.Dark or UraniumType.Dragon or UraniumType.Fighting or UraniumType.Nuclear) => UraniumEffectiveness.Weak,
            (UraniumType.Fairy, UraniumType.Fire or UraniumType.Poison or UraniumType.Steel) => UraniumEffectiveness.Resisted,

            (UraniumType.Nuclear, UraniumType.Normal or UraniumType.Fire or UraniumType.Fighting or UraniumType.Water or UraniumType.Flying or UraniumType.Grass or UraniumType.Poison
                                    or UraniumType.Electric or UraniumType.Ground or UraniumType.Psychic or UraniumType.Rock or UraniumType.Ice or UraniumType.Bug
                                    or UraniumType.Dragon or UraniumType.Ghost or UraniumType.Dark or UraniumType.Fairy)
                                    => UraniumEffectiveness.Weak,
            (UraniumType.Nuclear, UraniumType.Steel or UraniumType.Nuclear) => UraniumEffectiveness.Resisted,

            _ => UraniumEffectiveness.Normal
        };

    /// <summary>
    /// Determines the effectiveness of an attack of type <paramref name="attacking" /> against 
    /// a pokemon whose types are <paramref name="primaryDefending" /> and <paramref name="secondaryDefending" />.
    /// </summary>
    /// <param name="attacking">The type of the attacking move</param>
    /// <param name="primaryDefending">The primary type of the defending pokemon</param>
    /// <param name="secondaryDefending">The secondary type of the defending pokemon</param>
    /// <returns>The effectiveness of the move against the defender</returns>
    public static UraniumEffectiveness GetCompoundEffectiveness(UraniumType attacking, UraniumType primaryDefending, UraniumType secondaryDefending) {
        var primary = GetTypeEffectiveness(attacking, primaryDefending);
        var secondary = GetTypeEffectiveness(attacking, secondaryDefending);

        int primaryInt = (int)primary;
        int secondaryInt = (int)secondary;

        int finalInt = primaryInt * secondaryInt / 100;

        if (!Enum.IsDefined((UraniumEffectiveness)finalInt)) {
            throw new InvalidOperationException("Given combination of types results in an effectiveness which does not exist");
        }

        return (UraniumEffectiveness)finalInt;
    }
}
