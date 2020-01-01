// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

using Xunit;
using Xunit.Abstractions;

public class CalculatorTests
{
    private readonly ITestOutputHelper logger;

    public CalculatorTests(ITestOutputHelper logger)
    {
        this.logger = logger;
    }

    [Fact]
    public void AddOrSubtract()
    {
        Assert.Equal(-1, -1);
    }
}
