using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Pokepanion.Library.Extensions;
using Pokepanion.Library.Helpers;
using Pokepanion.Library.Uranium;

namespace Pokepanion.PokedexScraper.App;

public class UraniumPokedexScraper : BaseScraper<UraniumPokemonInfo> {

    private const string POKEDEX_TABLE_XPATH = @"/html/body/div[4]/div[3]/div[2]/main/div[3]/div[2]/div/table[1]/tbody";

    protected override Task<IEnumerable<UraniumPokemonInfo>> ScrapeAsync(HtmlDocument page, string baseUrl) {
        HtmlNode tableNode = page.DocumentNode.SelectSingleNode(POKEDEX_TABLE_XPATH);

        var data = tableNode.ChildNodes
                        .Where(node => node.NodeType == HtmlNodeType.Element)
                        .Skip(1)
                        .Select(node => ParseNodeAsPokemonInfo(node));

        return Task.FromResult(data);
    }

    private static UraniumPokemonInfo ParseNodeAsPokemonInfo(HtmlNode root) {

        // Get nodes for Od, Name, Primary and Secondary types
        var dexIdNode = root.GetNthChildElement(0).GetNthChildElement(0);
        var nameNode = root.GetNthChildElement(2).GetNthChildElement(0).GetNthChildElement(0);
        var primaryTypeNode = root.GetNthChildElement(3).GetNthChildElement(0).GetNthChildElement(0);
        var secondaryTypeNode = root.GetNthChildElement(4).GetNthChildElement(0).GetNthChildElement(0);

        // Resolve primary/secondary types from text
        var parsedPrimary = Enum.Parse<UraniumType>(primaryTypeNode.InnerText, ignoreCase: true);
        var parsedSecondary = Enum.Parse<UraniumType>(secondaryTypeNode.InnerText, ignoreCase: true);

        UraniumType? actualSecondary = parsedSecondary != parsedPrimary ? parsedSecondary : null;

        UraniumPokemonInfo info = new() {
            Id = uint.Parse(dexIdNode.InnerText[1..]), // first character is #, drop it
            Name = nameNode.InnerText,
            PrimaryType = parsedPrimary,
            SecondaryType = actualSecondary,
            TypeEffectivenesses = GetTypeEffectivenessMap(parsedPrimary, actualSecondary)
        };

        return info;
    }

    private static Dictionary<UraniumEffectiveness, UraniumType[]> GetTypeEffectivenessMap(UraniumType primary, UraniumType? secondary) {
        List<UraniumType> immune = new();
        List<UraniumType> doubleResisted = new();
        List<UraniumType> resisted = new();
        List<UraniumType> normal = new();
        List<UraniumType> weak = new();
        List<UraniumType> superWeak = new();

        foreach (var attackingType in Enum.GetValues<UraniumType>()) {
            var effectiveness = secondary.HasValue
                                    ? UraniumTypeHelper.GetCompoundEffectiveness(attackingType, primary, secondary.Value)
                                    : UraniumTypeHelper.GetTypeEffectiveness(attackingType, primary);

            (effectiveness switch {
                UraniumEffectiveness.Immune => immune,
                UraniumEffectiveness.DoubleResisted => doubleResisted,
                UraniumEffectiveness.Resisted => resisted,
                UraniumEffectiveness.Normal => normal,
                UraniumEffectiveness.Weak => weak,
                UraniumEffectiveness.SuperWeak => superWeak,
                _ => throw new Exception("Looks like there's a new UraniumEffectiveness value to account for")
            }).Add(attackingType);
        }

        return new Dictionary<UraniumEffectiveness, UraniumType[]>(6) {
            {UraniumEffectiveness.Immune, immune.ToArray()},
            {UraniumEffectiveness.DoubleResisted, doubleResisted.ToArray()},
            {UraniumEffectiveness.Resisted, resisted.ToArray()},
            {UraniumEffectiveness.Normal, normal.ToArray()},
            {UraniumEffectiveness.Weak, weak.ToArray()},
            {UraniumEffectiveness.SuperWeak, superWeak.ToArray()}
        };
    }
}
