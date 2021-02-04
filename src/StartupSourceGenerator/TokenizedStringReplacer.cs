using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace StartupSourceGenerator
{
	public static class TokenizedStringReplacer
	{
		public static Regex regex = new Regex(@"(?<full>\#{(?<token>.+)}\#)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

		public static string Replace(this string tokenizedString, IEnumerable<KeyValuePair<string, string>> tokenValues)
		{
			foreach (var token in tokenValues)
			{
				tokenizedString = tokenizedString.Replace($"#{{{token.Key}}}#", token.Value);
			}

			var res = regex.Matches(tokenizedString);

			var tokenGroups = res.Cast<Match>()
				.Select(m => (m.Groups["full"], m.Groups["token"]))
				.Select(g => new
				{
					Name = g.Item2.Value,
					Index = g.Item1.Index,
					Length = g.Item1.Length,
				})
				.ToList();

			var values = tokenValues.ToDictionary(t => t.Key, t => t.Value, StringComparer.OrdinalIgnoreCase);
			foreach (var group in tokenGroups.OrderByDescending(g => g.Index))
			{
				tokenizedString = tokenizedString.Remove(group.Index, group.Length);
			}

			return tokenizedString;
		}
	}
}
