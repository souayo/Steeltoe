﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Steeltoe.Common.HealthChecks;
using System.Collections.Generic;

namespace Steeltoe.Management.Endpoint.Health
{
    public interface IHealthAggregator
    {
        HealthCheckResult Aggregate(IList<IHealthContributor> contributors);
    }
}
