using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace nBucket.Client
{
    public class BitBucketClient : IDisposable
    {
        private readonly HttpClient _httpClient;

        public BitBucketAuthenticationOptions Options { get; set; }

        public BitBucketToken Token { get; set; }

        public BitBucketClient() { }

        public BitBucketClient(BitBucketAuthenticationOptions options)
        {
            Options = options ?? new BitBucketAuthenticationOptions();

            _httpClient = new HttpClient();
            _httpClient.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB
        }

        public string GetAuthorizationEndpoint()
        {
            // comma separated
            string scope = string.Join(",", Options.Scope);

            string authorizationEndpoint =
                Options.AuthorizationEndpoint +
                    "?response_type=code" +
                    "&client_id=" + Uri.EscapeDataString(Options.ConsumerKey) +
                    "&scope=" + Uri.EscapeDataString(scope);

            return authorizationEndpoint;
        }

        public async Task<BitBucketToken> RequestToken(string code)
        {
            var body = new List<KeyValuePair<string, string>>
                            {
                                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                                new KeyValuePair<string, string>("code", code)
                            };

            await GetToken(body);

            if (Options.OnAuthenticated != null)
            {
                Options.OnAuthenticated(Token);
            }

            return Token;
        }

        public async Task<BitBucketToken> RefreshToken()
        {
            var body = new List<KeyValuePair<string, string>>
                            {
                                new KeyValuePair<string, string>("grant_type", "refresh_token "),
                                new KeyValuePair<string, string>("refresh_token", Token.RefreshToken)
                            };

            await GetToken(body);

            if (Options.OnRefreshAuthenticated != null)
            {
                Options.OnAuthenticated(Token);
            }

            return Token;
        }

        private async Task<BitBucketToken> GetToken(List<KeyValuePair<string, string>> body)
        {
            var content = new FormUrlEncodedContent(body);

            var byteArray = new UTF8Encoding().GetBytes($"{Options.ConsumerKey}:{Options.ConsumerSecret}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            HttpResponseMessage tokenResponse = await _httpClient.PostAsync(Options.TokenEndpoint, content);
            tokenResponse.EnsureSuccessStatusCode();
            string text = await tokenResponse.Content.ReadAsStringAsync();

            JObject authReponse = JObject.Parse(text);

            Token = new BitBucketToken(authReponse);

            return Token;
        }

        public async Task<BitBucketUser> GetUser()
        {
            return await GetAsync<BitBucketUser>(Constants.UserInformationEndpoint);
        }

        public async Task<GetRepositoriesResponse> GetRepositories(string username)
        {
            return await GetAsync<GetRepositoriesResponse>(new Uri(new Uri(Constants.RepositoryEndpoint), $"{username}").ToString());
        }

        public async Task<GetCommitsResponse> GetCommits(string repoIdentifier)
        {
            return await GetAsync<GetCommitsResponse>(new Uri(new Uri(Constants.RepositoryEndpoint), $"{repoIdentifier}/commits").ToString());
        }

        public async Task<BitBucketCommit> GetCommit(string repoIdentifier, string hash)
        {
            return await GetAsync<BitBucketCommit>(new Uri(new Uri(Constants.RepositoryEndpoint), $"{repoIdentifier}/commit/{hash}").ToString());
        }

        public async Task<List<BitBucketCommitDiffStats>> GetCommitStats(string repoIdentifier, string hash)
        {
            return await GetAsync<List<BitBucketCommitDiffStats>>(new Uri(new Uri(Constants.V1RepositoryEndpoint), $"{repoIdentifier}/changesets/{hash}/diffstat").ToString());
        }

        public async Task<string> GetCommitDiff(string repoIdentifier, string hash)
        {
            return await GetAsync(new Uri(new Uri(Constants.RepositoryEndpoint), $"{repoIdentifier}/diff/{hash}").ToString());
        }

        public async Task<T> GetAsync<T>(string requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            return await MakeRequest<T>(request);
        }

        public async Task<string> GetAsync(string requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            return await MakeRequest(request);
        }

        public async Task<T> PostBasicAuthorizationAsync<T>(string requestUri, List<KeyValuePair<string, string>> body)
        {
            var content = new FormUrlEncodedContent(body);

            var byteArray = new UTF8Encoding().GetBytes($"{Options.ConsumerKey}:{Options.ConsumerSecret}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Content = content;

            return await MakeRequest<T>(request);
        }

        public async Task<T> MakeRequest<T>(HttpRequestMessage request)
        {
            return JsonConvert.DeserializeObject<T>(await MakeRequest(request));
        }

        public async Task<string> MakeRequest(HttpRequestMessage request)
        {
            if (Token.HasExpired)
            {
                await RefreshToken();
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.AccessToken);

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await RefreshToken();
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
