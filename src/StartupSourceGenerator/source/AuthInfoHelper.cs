// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore
{
    using System.Collections.Generic;
    using Rixian.Extensions.Errors;
    using static Rixian.Extensions.Errors.Prelude;
    
    public class AuthInfoHelper
    {
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor;

        public AuthInfoHelper(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetClientId()
        {
            var context = this.httpContextAccessor.HttpContext;
            var claim = context.User.FindFirst("client_id");
            return claim?.Value;
        }

        public string GetSub()
        {
            var context = this.httpContextAccessor.HttpContext;
            var claim = context.User.FindFirst("sub");
            return claim?.Value;
        }
    }
}
