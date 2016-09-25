using Microsoft.Owin;

namespace nBucket.Owin
{
    public class BitBucketMiddlewareOptions
    {
        /// <summary>
        /// The request path within the application's base path where the user-agent will be returned.
        /// Default value is "/signin-bitbucket".
        /// </summary>
        public PathString CallbackPath { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IBitBucketAuthenticationProvider"/> used to handle authentication events.
        /// </summary>
        public IBitBucketAuthenticationProvider Provider { get; set; }

        /// <summary>
        /// Gets or sets the type used to secure data handled by the middleware.
        /// </summary>
        //public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }

        public BitBucketMiddlewareOptions()
        {
            CallbackPath = new PathString("/signin-bitbucket");
            Provider = new BitBucketAuthenticationProvider();
        }
    }
}
