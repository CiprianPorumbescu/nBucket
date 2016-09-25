using Newtonsoft.Json;
using System;

namespace nBucket.Client
{
    public class BitBucketUser
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("uuid")]
        public Guid Uuid { get; set; }

        [JsonProperty("links")]
        public BitBucketLinks Links { get; set; }
        

        public class BitBucketLinks
        {
            [JsonProperty("self")]
            public BitBucketLink Self { get; set; }

            [JsonProperty("html")]
            public BitBucketLink Html { get; set; }

            [JsonProperty("avatar")]
            public BitBucketLink Avatar { get; set; }
        }
    }

    public class BitBucketLink
    {
        [JsonProperty("href")]
        public string Url { get; set; }
    }
}
