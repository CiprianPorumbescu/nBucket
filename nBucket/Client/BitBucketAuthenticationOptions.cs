using System;
using System.Collections.Generic;

namespace nBucket.Client
{
    public class BitBucketAuthenticationOptions
    {
        /// <summary>
        /// Initializes a new <see cref="BitBucketAuthenticationOptions"/>
        /// </summary>
        public BitBucketAuthenticationOptions()
        {
            AuthorizationEndpoint = Constants.AuthorizationEndpoint;
            TokenEndpoint = Constants.TokenEndpoint;
            UserInformationEndpoint = Constants.UserInformationEndpoint;

            Scope = new List<string>();
        }

        /// <summary>
        /// Gets or sets the BitBucket-assigned appId
        /// </summary>
        public string ConsumerKey { get; set; }

        /// <summary>
        /// Gets or sets the BitBucket-assigned app secret
        /// </summary>
        public string ConsumerSecret { get; set; }

        /// <summary>
        /// Gets or sets the URI where the client will be redirected to authenticate.
        /// The default value is 'https://bitbucket.org/site/oauth2/authorize'.
        /// </summary>
        public string AuthorizationEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the URI the middleware will access to exchange the OAuth token.
        /// The default value is 'https://bitbucket.org/site/oauth2/access_token'.
        /// </summary>
        public string TokenEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the URI the middleware will access to obtain the user information.
        /// The default value is 'https://api.bitbucket.org/1.0/user'.
        /// </summary>
        public string UserInformationEndpoint { get; set; }

        /// <summary>
        /// A list of permissions to request.
        /// </summary>
        public IList<string> Scope { get; private set; }

        public Action<BitBucketToken> OnAuthenticated { get; set; }

        public Action<BitBucketToken> OnRefreshAuthenticated { get; set; }
    }
}
