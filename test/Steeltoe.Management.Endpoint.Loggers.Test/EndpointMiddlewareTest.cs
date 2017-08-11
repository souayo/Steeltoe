﻿//
// Copyright 2017 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Steeltoe.Management.Endpoint.Loggers.Test
{
    public class EndpointMiddlewareTest  : BaseTest
    {
        [Fact]
        public async void LoggersActuator_ReturnsExpectedData()
        {

            var builder = new WebHostBuilder().UseStartup<Startup>();
            using (var server = new TestServer(builder))
            {
                var client = server.CreateClient();
                var result = await client.GetAsync("http://localhost/cloudfoundryapplication/loggers");
                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
                var json = await result.Content.ReadAsStringAsync();
               Assert.NotNull(json);
         
                var loggers = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                Assert.NotNull(loggers);
                Assert.True(loggers.ContainsKey("levels"));
                Assert.True(loggers.ContainsKey("loggers"));

            }
        }

        [Fact]
        public async void LoggersActuator_AcceptsPost()
        {

            var builder = new WebHostBuilder().UseStartup<Startup>();
            using (var server = new TestServer(builder))
            {
                var client = server.CreateClient();
                var result = await client.GetAsync("http://localhost/cloudfoundryapplication/loggers");
                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
                var json = await result.Content.ReadAsStringAsync();
                Assert.NotNull(json);

                var loggers = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                Assert.NotNull(loggers);
                Assert.True(loggers.ContainsKey("levels"));
                Assert.True(loggers.ContainsKey("loggers"));
                HttpContent content = new StringContent("{\"configuredLevel\":\"WARN\"}");
                var result2 = await client.PostAsync("http://localhost/cloudfoundryapplication/loggers/Steeltoe.Management.Endpoint.Loggers.LoggersEndpointMiddleware", content);
                Assert.Equal(HttpStatusCode.OK, result2.StatusCode);
            }
        }
    }
}