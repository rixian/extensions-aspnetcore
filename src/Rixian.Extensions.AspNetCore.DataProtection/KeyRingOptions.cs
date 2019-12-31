// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.DataProtection
{
    public class KeyRingOptions
    {
        public string KeyName { get; set; }

        public string KeyIdentifier { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }
    }
}
