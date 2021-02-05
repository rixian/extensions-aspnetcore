// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

#nullable enable
namespace Rixian.Extensions.AspNetCore
{
    using System;
    using System.Text.RegularExpressions;
    using Microsoft.AspNetCore.Routing;
    using Rixian.Extensions.Errors;

    /// <summary>
    /// Replaces a capitalized string with a slugified one (foo-bar vs fooBar).
    /// </summary>
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        /// <summary>
        /// Transforms the parameter to a slug.
        /// </summary>
        /// <param name="value">The incoming string.</param>
        /// <returns>The slugified string.</returns>
        public string? TransformOutbound(object? value)
        {
            if (value == null)
            {
                return null;
            }

            var valueStr = value.ToString();
            if (valueStr == null)
            {
                return null;
            }

            var result = Regex.Replace(
                valueStr,
                "([a-z])([A-Z])",
                "$1-$2",
                RegexOptions.CultureInvariant,
                TimeSpan.FromMilliseconds(100)).ToLowerInvariant();

            if (Guid.TryParse(valueStr, out Guid valueGuid))
            {
                return valueGuid.ToString("N");
            }

            return result;
        }
    }
}
