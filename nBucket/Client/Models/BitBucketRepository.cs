using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nBucket.Client
{
    public class GetRepositoriesResponse
    {
        [JsonProperty("pagelen")]
        public int PageSize { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("values")]
        public List<BitBucketRepository> Items { get; set; }
    }

    public class BitBucketRepository
    {
        [JsonProperty("uuid")]
        public Guid Uuid { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("scm")]
        public string Scm { get; set; }

        [JsonProperty("created_on")]
        public DateTime CreatedOn { get; set; }
    }
}
