using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SmashGGEventTicker
{
    public class SmashGGInMemoryCache
    {
        private HttpClient _client;

        private static readonly string _token = ConfigurationManager.AppSettings["token"];
        private static readonly string _apiEndpoint = ConfigurationManager.ConnectionStrings["apiEndpoint"].ToString();
        static Uri requestUri = new Uri($"{_apiEndpoint}");

        private readonly ILogger<SmashGGInMemoryCache> _logger;

        private readonly IMemoryCache _memoryCache;

        public SmashGGInMemoryCache(ILogger<SmashGGInMemoryCache> logger, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _client = new HttpClient();
            _logger = logger;
        }

        public async Task<SmashGGResponse> TryGetCachedData(int videogameId)
        {
            _logger.LogInformation($"Attempting to get cached data: {DateTime.Now}");

            SmashGGResponse response;

            if (!_memoryCache.TryGetValue(videogameId, out response))
            {
                _logger.LogInformation($"No data in cache (or cache expired), requesting new data...");

                // TODO - Change hard coded game ID
                response = DataAccessors.GetDataFromResponse(await DataAccessors.GetEventsByTournamentStrive(_client, requestUri, _token, videogameId));

                var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMilliseconds(30000));

                _memoryCache.Set<SmashGGResponse>(videogameId, response, cacheOptions);
            }

            return response;
        }
    }
}
