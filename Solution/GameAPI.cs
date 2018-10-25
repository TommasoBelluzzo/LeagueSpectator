#region Using Directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
#endregion

namespace LeagueSpectator
{
    public static class GameAPI
    {
        #region Constants
        private const String KEY = ""; // Valid API Key
        private const String VERSION = "v3";
        #endregion

        #region Members
        private static readonly Dictionary<String,Int64> s_SummonerIdsCache = new Dictionary<String,Int64>();
        #endregion

        #region Methods
        private static async Task<T> GetResponse<T>(String url) where T : GameDTO
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.AcceptEncoding.Clear();
                request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                request.Headers.Add("X-Riot-Token", KEY);

                HttpResponseMessage response;

                using (HttpClient client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip }) { Timeout = TimeSpan.FromSeconds(200) })
                    response = await client.SendAsync(request).ConfigureAwait(false);

                if ((Int32)response.StatusCode == 429)
                {
                    Int32 delay;

                    if (response.Headers.TryGetValues("Retry-After", out IEnumerable<String> values))
                        delay = Int32.Parse(values.First());
                    else
                        delay = 1;

                    Task.Delay(delay * 1000).Wait();

                    return await GetResponse<T>(url);
                }

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream content = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                       
                    return (T)serializer.ReadObject(content);
                }

                return default(T);
            }
            catch
            {
                return default(T);
            }
        }

        public static async Task<GameInfo> GetGameInfo(String endPoint, Int64 summonerId)
        {
            String url = $"https://{endPoint}.api.riotgames.com/lol/spectator/{VERSION}/active-games/by-summoner/{summonerId}";
            return await GetResponse<GameInfo>(url).ConfigureAwait(false);
        }

        public static async Task<Int64> GetSummonerId(String endPoint, String summonerName)
        {
            String summonerIdKey = $"{endPoint}-{summonerName}";

            if (s_SummonerIdsCache.TryGetValue(summonerIdKey, out Int64 summonerId))
                return summonerId;

            String url = $"https://{endPoint}.api.riotgames.com/lol/summoner/{VERSION}/summoners/by-name/{summonerName}";
            Summoner summoner = await GetResponse<Summoner>(url).ConfigureAwait(false);

            if (summoner == null)
                return -1L;

            return (s_SummonerIdsCache[summonerIdKey] = summoner.Id);
        }
        #endregion
    }
}
