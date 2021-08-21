using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SmashGGEventTicker
{
    public sealed class SmashGGInMemoryCache
    {
        private static readonly SmashGGInMemoryCache instance = new SmashGGInMemoryCache();
        private static HttpClient Client = new HttpClient();

        private static readonly string _token = ConfigurationManager.AppSettings["token"];
        private static readonly string _apiEndpoint = ConfigurationManager.ConnectionStrings["apiEndpoint"].ToString();
        static Uri requestUri = new Uri($"{_apiEndpoint}");

        static SmashGGInMemoryCache() { }
        private SmashGGInMemoryCache()
        {
            SetThreadingTimer();
        }

        public static SmashGGInMemoryCache Instance
        {
            get
            {
                return instance;
            }
        }

        public SmashGGResponse SmashGGResponse { get; private set; }

        private async Task RefreshData()
        {
            Console.WriteLine($"RefreshData() is called: {DateTime.Now}");
            instance.SmashGGResponse = DataAccessors.GetDataFromResponse(await DataAccessors.GetEventsByTournamentStrive(Client, requestUri, _token));
        }

        public void SetThreadingTimer()
        {
            Console.WriteLine($"SetThreadingTimer() is called: {DateTime.Now}");
            var autoEvent = new System.Threading.AutoResetEvent(true);
            var timer = new System.Threading.Timer(async (o) => await instance.RefreshData(), autoEvent, 0, 30000);
        }
    }
}
