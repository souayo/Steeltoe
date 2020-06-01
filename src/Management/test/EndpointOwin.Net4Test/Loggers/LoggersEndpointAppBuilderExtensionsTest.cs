﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using Microsoft.Owin.Builder;
using Owin;
using Steeltoe.Management.Endpoint.Test;
using System;
using Xunit;

namespace Steeltoe.Management.EndpointOwin.Loggers.Test
{
    public class LoggersEndpointAppBuilderExtensionsTest : BaseTest
    {
        [Fact]
        public void UseLoggersActuator_ThrowsIfBuilderNull()
        {
            IAppBuilder builder = null;
            var config = new ConfigurationBuilder().Build();
            var exception = Assert.Throws<ArgumentNullException>(() => builder.UseLoggersActuator(config, null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void UseLoggersActuator_ThrowsIfConfigNull()
        {
            IAppBuilder builder = new AppBuilder();
            var exception = Assert.Throws<ArgumentNullException>(() => builder.UseLoggersActuator(null, null));
            Assert.Equal("config", exception.ParamName);
        }

        [Fact]
        public void UseLoggersActuator_ThrowsIfProviderNull()
        {
            IAppBuilder builder = new AppBuilder();
            var config = new ConfigurationBuilder().Build();
            var exception = Assert.Throws<ArgumentNullException>(() => builder.UseLoggersActuator(config, null));
            Assert.Equal("loggerProvider", exception.ParamName);
        }
    }
}
