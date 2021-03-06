﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschränkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Squidex.Infrastructure.Diagnostics;
using Squidex.Pipeline;
using Squidex.Pipeline.Diagnostics;
using Squidex.Pipeline.Robots;

namespace Squidex.Config.Web
{
    public static class WebExtensions
    {
        public static IApplicationBuilder UseMyLocalCache(this IApplicationBuilder app)
        {
            app.UseMiddleware<LocalCacheMiddleware>();

            return app;
        }

        public static IApplicationBuilder UseMyTracking(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestLogPerformanceMiddleware>();

            return app;
        }

        public static IApplicationBuilder UseMyHealthCheck(this IApplicationBuilder app)
        {
            app.Map("/readiness", builder =>
            {
                builder.UseMiddleware<HealthCheckMiddleware>(HealthCheckScopes.Any);
            });

            app.Map("/healthz", builder =>
            {
                builder.UseMiddleware<HealthCheckMiddleware>(HealthCheckScopes.Node);
            });

            app.Map("/cluster-healthz", builder =>
            {
                builder.UseMiddleware<HealthCheckMiddleware>(HealthCheckScopes.Cluster);
            });

            return app;
        }

        public static IApplicationBuilder UseMyRobotsTxt(this IApplicationBuilder app)
        {
            app.Map("/robots.txt", builder => builder.UseMiddleware<RobotsTxtMiddleware>());

            return app;
        }

        public static void UseMyCors(this IApplicationBuilder app)
        {
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
        }

        public static void UseMyForwardingRules(this IApplicationBuilder app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto,
                ForwardLimit = null,
                RequireHeaderSymmetry = false
            });

            app.UseMiddleware<EnforceHttpsMiddleware>();
        }
    }
}
