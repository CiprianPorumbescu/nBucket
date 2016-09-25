using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nBucket.Client
{
    public class GetCommitsResponse
    {
        [JsonProperty("pagelen")]
        public int PageSize { get; set; }

        [JsonProperty("values")]
        public List<BitBucketCommit> Items { get; set; }
    }

    public class BitBucketCommit
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("author")]
        public BitBucketCommitAuthor Author { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("links")]
        public BitBucketLinks Links { get; set; }

        public class BitBucketLinks
        {
            [JsonProperty("self")]
            public BitBucketLink Self { get; set; }

            [JsonProperty("html")]
            public BitBucketLink Html { get; set; }

            [JsonProperty("diff")]
            public BitBucketLink Diff { get; set; }
        }
}

    public class BitBucketCommitAuthor
    {
        [JsonProperty("raw")]
        public string Raw { get; set; }

        [JsonProperty("user")]
        public BitBucketUser User { get; set; }
    }

}
