// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore
{
    using System;
    using System.Collections.Generic;
    using Rixian.Extensions.Errors;
    using static Rixian.Extensions.Errors.Prelude;


    public class OpenIdConnectConfig
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
    }
}
