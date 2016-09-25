using Newtonsoft.Json;

namespace nBucket.Client
{
    public class BitBucketCommitDiffStats
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("file")]
        public string File { get; set; }

        [JsonProperty("diffstat")]
        public DiffStats Stats { get; set; }

        public class DiffStats
        {
            [JsonProperty("removed")]
            public int Removed { get; set; }

            [JsonProperty("added")]
            public int Added { get; set; }
        }
    }
}
