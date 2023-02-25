using System;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Pokepanion.Library.Extensions;

public static class HtmlNodeExtensions {

    public static HtmlNode GetNthChildElement(this HtmlNode node, int nthChild) {
        return node.ChildNodes
                    .Where(node => node.NodeType == HtmlNodeType.Element)
                    .Skip(nthChild)
                    .First();
    }

    public static bool HasStyle(this HtmlNode node, string propertyName, string propertyValue) {

        var styles = node.GetAttributeValue("style", string.Empty);

        if (string.IsNullOrEmpty(styles)) {
            return false;
        }

        var styleStrings = styles.Split(";", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        string pattern = $"^{Regex.Escape(propertyName.Trim())}\\s*:\\s*{Regex.Escape(propertyValue.Trim())}$";
        return styleStrings.Where(str => Regex.IsMatch(str, pattern)).Any();
    }
}
