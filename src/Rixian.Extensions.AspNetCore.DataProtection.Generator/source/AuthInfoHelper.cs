// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license.

#nullable enable
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

        public string? GetClientId()
        {
            Microsoft.AspNetCore.Http.HttpContext? context = this.httpContextAccessor.HttpContext;
            System.Security.Claims.Claim? claim = context?.User?.FindFirst("client_id");
            return claim?.Value;
        }

        public string? GetSub()
        {
            Microsoft.AspNetCore.Http.HttpContext? context = this.httpContextAccessor.HttpContext;
            System.Security.Claims.Claim? claim = context?.User?.FindFirst("sub");
            return claim?.Value;
        }
    }
}
