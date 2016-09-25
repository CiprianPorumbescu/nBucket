using Microsoft.Owin;
using nBucket.Client;

namespace nBucket
{
    public class BitBucketAuthenticatedContext
    {
        public IOwinContext OwinContext { get; set; }

        public BitBucketToken Token { get; set; }
    }
}
