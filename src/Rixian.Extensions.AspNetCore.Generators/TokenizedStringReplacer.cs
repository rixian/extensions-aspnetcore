// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Replaces tokenized values of the form #{TOKEN_NAME}# with a specified value.
    /// </summary>
    internal static class TokenizedStringReplacer
    {
        private static Regex regex = new Regex(@"(?<full>\#{(?<token>.+)}\#)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary>
        /// replaces tokens with values.
        /// </summary>
        /// <param name="tokenizedString">The tokenized string.</param>
        /// <param name="tokenValues">The token values to replace.</param>
        /// <returns>The final string.</returns>
        public static string Replace(this string tokenizedString, IEnumerable<KeyValuePair<string, string>> tokenValues)
        {
            foreach (KeyValuePair<string, string> token in tokenValues)
            {
                tokenizedString = tokenizedString.Replace($"#{{{token.Key}}}#", token.Value);
            }

            MatchCollection? res = regex.Matches(tokenizedString);

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
