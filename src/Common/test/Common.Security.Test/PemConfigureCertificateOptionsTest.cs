﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Xunit;

namespace Steeltoe.Common.Security.Test
{
    public class PemConfigureCertificateOptionsTest
    {
        // possibly related https://github.com/dotnet/corefx/issues/11046
        [Fact]
        [Trait("Category", "SkipOnLinux")]
        [Trait("Category", "SkipOnMacOS")]
        public void AddPemFiles_ReadsFiles_CreatesCertificate()
        {
            var config = new ConfigurationBuilder()
                .AddPemFiles("instance.crt", "instance.key")
                .Build();
            Assert.NotNull(config["certificate"]);
            Assert.NotNull(config["privateKey"]);
            var pemConfig = new PemConfigureCertificateOptions(config);
            CertificateOptions opts = new CertificateOptions();
            pemConfig.Configure(opts);
            Assert.NotNull(opts.Certificate);
            Assert.Equal(Options.DefaultName, opts.Name);
            Assert.True(opts.Certificate.HasPrivateKey);
        }
    }
}
