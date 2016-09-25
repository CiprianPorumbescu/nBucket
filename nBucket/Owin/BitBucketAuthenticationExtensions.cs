// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using nBucket;
using nBucket.Owin;
using System;

namespace Owin
{
    /// <summary>
    /// Extension methods for using <see cref="BitBucketAuthenticationMiddleware"/>
    /// </summary>
    public static class BitBucketAuthenticationExtensions
    {
        /// <summary>
        /// Authenticate users using BitBucket
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder"/> passed to the configuration method</param>
        /// <param name="options">Middleware configuration options</param>
        /// <returns>The updated <see cref="IAppBuilder"/></returns>
        public static IAppBuilder UseBitBucketAuthentication(this IAppBuilder app, BitBucketMiddlewareOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            app.Use(typeof(BitBucketAuthenticationMiddleware), app, options);
            return app;
        }
    }
}
