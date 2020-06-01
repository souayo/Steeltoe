﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Autofac;
using Microsoft.Extensions.Configuration;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Endpoint.Health;
using Steeltoe.Management.Endpoint.Health.Contributor;
using Steeltoe.Management.EndpointOwin.Health;
using System;
using System.Collections.Generic;

namespace Steeltoe.Management.EndpointOwinAutofac.Actuators
{
    public static class HealthContainerBuilderExtensions
    {
        /// <summary>
        /// Register the Health endpoint, OWIN middleware and options
        /// </summary>
        /// <param name="container">Autofac DI <see cref="ContainerBuilder"/></param>
        /// <param name="config">Your application's <see cref="IConfiguration"/></param>
        public static void RegisterHealthActuator(this ContainerBuilder container, IConfiguration config)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            container.RegisterHealthActuator(config, new DefaultHealthAggregator(), new Type[] { typeof(DiskSpaceContributor) });
        }

        /// <summary>
        /// Register the Health endpoint, OWIN middleware and options
        /// </summary>
        /// <param name="container">Autofac DI <see cref="ContainerBuilder"/></param>
        /// <param name="config">Your application's <see cref="IConfiguration"/></param>
        /// <param name="contributors">Types that implement <see cref="IHealthContributor"/></param>
        public static void RegisterHealthActuator(this ContainerBuilder container, IConfiguration config, params Type[] contributors)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            container.RegisterHealthActuator(config, new DefaultHealthAggregator(), contributors);
        }

        /// <summary>
        /// Register the Health endpoint, OWIN middleware and options
        /// </summary>
        /// <param name="container">Autofac DI <see cref="ContainerBuilder"/></param>
        /// <param name="config">Your application's <see cref="IConfiguration"/></param>
        /// <param name="aggregator">Your <see cref="IHealthAggregator"/></param>
        /// <param name="contributors">Types that implement <see cref="IHealthContributor"/></param>
        public static void RegisterHealthActuator(this ContainerBuilder container, IConfiguration config, IHealthAggregator aggregator, params Type[] contributors)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (aggregator == null)
            {
                aggregator = new DefaultHealthAggregator();
            }

            container.Register(c =>
            {
                var options = new HealthEndpointOptions(config);
                var mgmtOptions = c.Resolve<IEnumerable<IManagementOptions>>();

                foreach (var mgmt in mgmtOptions)
                {
                   mgmt.EndpointOptions.Add(options);
                }

                return options;
            }).As<IHealthOptions>().IfNotRegistered(typeof(IHealthOptions)).SingleInstance();

            container.RegisterInstance(aggregator).As<IHealthAggregator>().SingleInstance();
            foreach (var c in contributors)
            {
                container.RegisterType(c).As<IHealthContributor>();
            }

            container.RegisterType<HealthEndpoint>();
            container.RegisterType<HealthEndpointOwinMiddleware>();
        }
    }
}
