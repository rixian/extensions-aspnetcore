// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore
{
    using System;
    using System.Text.RegularExpressions;
    using Microsoft.AspNetCore.Routing;
    using Rixian.Extensions.Errors;

    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            if (value == null)
            {
                return null;
            }

            var valueStr = value.ToString();
            var result = Regex.Replace(valueStr,
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
