﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Steeltoe.CloudFoundry.Connector.Relational;
using Steeltoe.CloudFoundry.Connector.Relational.SqlServer;
using Steeltoe.CloudFoundry.Connector.Services;
using Steeltoe.Common.HealthChecks;
using System;
using System.Data;

namespace Steeltoe.CloudFoundry.Connector.SqlServer
{
    public static class SqlServerServiceCollectionExtensions
    {
        /// <summary>
        /// Add an IHealthContributor to a ServiceCollection for SqlServer
        /// </summary>
        /// <param name="services">Service collection to add to</param>
        /// <param name="config">App configuration</param>
        /// <param name="contextLifetime">Lifetime of the service to inject</param>
        /// <param name="logFactory">logger factory</param>
        /// <returns>IServiceCollection for chaining</returns>
        public static IServiceCollection AddSqlServerHealthContributor(this IServiceCollection services, IConfiguration config, ServiceLifetime contextLifetime = ServiceLifetime.Singleton, ILoggerFactory logFactory = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            SqlServerServiceInfo info = config.GetSingletonServiceInfo<SqlServerServiceInfo>();

            DoAdd(services, info, config, contextLifetime);
            return services;
        }

        /// <summary>
        /// Add an IHealthContributor to a ServiceCollection for SqlServer
        /// </summary>
        /// <param name="services">Service collection to add to</param>
        /// <param name="config">App configuration</param>
        /// <param name="serviceName">cloud foundry service name binding</param>
        /// <param name="contextLifetime">Lifetime of the service to inject</param>
        /// <param name="logFactory">logger factory</param>
        /// <returns>IServiceCollection for chaining</returns>
        public static IServiceCollection AddSqlServerHealthContributor(this IServiceCollection services, IConfiguration config, string serviceName, ServiceLifetime contextLifetime = ServiceLifetime.Singleton, ILoggerFactory logFactory = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (string.IsNullOrEmpty(serviceName))
            {
                throw new ArgumentNullException(nameof(serviceName));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            SqlServerServiceInfo info = config.GetRequiredServiceInfo<SqlServerServiceInfo>(serviceName);

            DoAdd(services, info, config, contextLifetime);
            return services;
        }

        private static void DoAdd(IServiceCollection services, SqlServerServiceInfo info, IConfiguration config, ServiceLifetime contextLifetime)
        {
            var sqlServerConfig = new SqlServerProviderConnectorOptions(config);
            var factory = new SqlServerProviderConnectorFactory(info, sqlServerConfig, SqlServerTypeLocator.SqlConnection);
            services.Add(new ServiceDescriptor(typeof(IHealthContributor), ctx => new RelationalHealthContributor((IDbConnection)factory.Create(ctx), ctx.GetService<ILogger<RelationalHealthContributor>>()), contextLifetime));
        }
    }
}
