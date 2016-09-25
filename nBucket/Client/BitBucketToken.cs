using nBucket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;

namespace nBucket.Client
{
    public class BitBucketToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        public TimeSpan? ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("scopes")]
        public string Scopes { get; set; }

        public DateTime? ExpireDate { get; set; }

        public BitBucketToken() { }

        public BitBucketToken(JObject authResponse)
        {
            AccessToken = authResponse.TryGetValue("access_token");

            string expires = authResponse.TryGetValue("expires_in");

            int expiresValue;
            if (Int32.TryParse(expires, NumberStyles.Integer, CultureInfo.InvariantCulture, out expiresValue))
            {
                ExpiresIn = TimeSpan.FromSeconds(expiresValue);
                ExpireDate = DateTime.Now.Add(ExpiresIn.Value);
            }

            RefreshToken = authResponse.TryGetValue("refresh_token");

            TokenType = authResponse.TryGetValue("token_type");

            Scopes = authResponse.TryGetValue("scopes");

        }

        public bool HasExpired { get { return ExpireDate.HasValue ? ExpireDate < DateTime.Now : false; } }
    }
}
