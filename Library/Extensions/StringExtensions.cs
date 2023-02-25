using System;
using System.Linq;
using System.Text;

namespace Pokepanion.Library.Extensions;

public static class StringExtensions
{

	/// <summary>
	/// Multiplies the string a given number of times. Negative values also reverse the string.
	/// </summary>
	/// <param name="multiplier">The number of times to multiply the string. 0 returns an empty string,
	/// and a negative number also reverses the string.</param>
	/// <returns>The string multiplied <paramref name="multiplier" /> times.</returns>
	public static string Multiply(this string source, int multiplier)
	{
		// str * 0 = ""
		if (multiplier == 0)
		{
			return string.Empty;
		}

		var toConcat = source;

		// str * -1 = rev(str)
		if (multiplier < 0)
		{
			toConcat = string.Concat(source.Reverse());
		}

		int absMultiplier = Math.Abs(multiplier);
		StringBuilder builder = new(toConcat.Length * absMultiplier);

		for (int i = 0; i < absMultiplier; i++)
		{
			builder.Append(toConcat);
		}

		return builder.ToString();
	}

}
