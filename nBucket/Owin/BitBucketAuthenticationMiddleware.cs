using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Logging;
using nBucket.Client;
using Owin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nBucket.Owin
{
    public class BitBucketAuthenticationMiddleware : OwinMiddleware
    {
        private readonly ILogger _logger;

        public BitBucketMiddlewareOptions Options { get; set; }

        public BitBucketAuthenticationMiddleware(OwinMiddleware next, IAppBuilder app, BitBucketMiddlewareOptions options)
            : base(next)
        {
            Options = options;
            _logger = app.CreateLogger<BitBucketAuthenticationMiddleware>();
        }

        protected async Task AuthenticateAsync(IOwinContext context)
        {
            var request = context.Request;

            try
            {
                string code = null;

                IReadableStringCollection query = request.Query;

                IList<string> values = query.GetValues("error");
                if (values != null && values.Count >= 1)
                {
                    _logger.WriteVerbose("Remote server returned an error: " + request.QueryString);
                    return;
                }

                values = query.GetValues("code");
                if (values != null && values.Count == 1)
                {
                    code = values[0];
                }

                if (code == null)
                {
                    return;
                }

                var bitBucketClient = context.Get<BitBucketClient>();

                var token = await bitBucketClient.RequestToken(code);

                if (token == null)
                {
                    return;
                }

                var authContext = new BitBucketAuthenticatedContext { OwinContext = context, Token = token };

                await Options.Provider.Authenticated(authContext);

            }
            catch (Exception ex)
            {
                _logger.WriteError("Authentication failed", ex);
            }
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (Options.CallbackPath.HasValue && Options.CallbackPath == context.Request.Path)
            {
                await AuthenticateAsync(context);

                var returnContext = new BitBucketReturnEndpointContext { OwinContext = context };

                await Options.Provider.ReturnEndpoint(returnContext);

                context.Response.Redirect("/");

                return;
            }

            await Next.Invoke(context);
        }
    }
}
