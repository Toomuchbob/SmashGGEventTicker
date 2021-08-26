using GraphQL.Query.Builder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SmashGGEventTicker
{
    static class DataAccessors
    {
        public static SmashGGResponse GetDataFromResponse(string responseString)
        {
            return JsonConvert.DeserializeObject<SmashGGResponse>(responseString);
        }

        public static async Task<string> GetEventsByTournamentStrive(HttpClient client, Uri endpoint, string token, int videogameId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var queryOptions = new QueryOptions() { Formatter = QueryFormatters.CamelCaseFormatter };

            // A dictionary of dictionaries is required for complex arguments

            var tournamentFilter = new Dictionary<string, object>();
            tournamentFilter.Add("videogameIds", new int[] { videogameId });
            tournamentFilter.Add("published", true);
            tournamentFilter.Add("hasOnlineEvents", true);
            tournamentFilter.Add("beforeDate", new DateTimeOffset(DateTime.Now).AddDays(14).ToUnixTimeSeconds());
            tournamentFilter.Add("afterDate", new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds());

            var tournamentArguments = new Dictionary<string, object>();
            tournamentArguments.Add("perPage", 500);
            tournamentArguments.Add("page", 1);
            tournamentArguments.Add("filter", tournamentFilter);

            var eventFilter = new Dictionary<string, object>();
            eventFilter.Add("videogameId", videogameId);

            var eventArguments = new Dictionary<string, object>();
            eventArguments.Add("filter", eventFilter);

            var query = new Query<Tournaments>("tournaments", queryOptions)
                .AddArgument("query", tournamentArguments)
                .AddField(f => f.Nodes,
                    sq => sq
                    .AddField(f => f.Id)
                    .AddField(f => f.Name)
                    .AddField(f => f.CountryCode)
                    .AddField(f => f.IsOnline)
                    .AddField(f => f.Slug)
                    .AddField(f => f.StartAt)
                    .AddField(f => f.IsRegistrationOpen)
                    .AddField(f => f.Hashtag)
                    .AddField(f => f.Events,
                        sq => sq
                        .AddArgument("filter", eventFilter)
                        .AddField(f => f.Id)
                        .AddField(f => f.Name)
                        .AddField(f => f.isOnline)
                        .AddField(f => f.NumEntrants)
                        .AddField(f => f.EntrantSizeMax)
                        .AddField(f => f.StartAt)
                        .AddField(f => f.Videogame,
                            sq => sq
                            .AddField(f => f.Id)
                            .AddField(f => f.Name)
                        )
                    )
                    .AddField(f => f.Streams,
                        sq => sq
                        .AddField(f => f.Id)
                        .AddField(f => f.IsOnline)
                        .AddField(f => f.StreamName)
                        .AddField(f => f.StreamSource)
                    )
                );

            Console.WriteLine("{" + query.Build() + "}");
            Console.WriteLine("");

            request.Content = new StringContent(JsonConvert.SerializeObject(new { query = "{" + query.Build() + "}" }));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }
    }
}



//{
//  tournaments(query: {
//      perPage: 500
//      page: 1
//      filter: {
//          videogameIds: 33945
//          published: true
//          hasOnlineEvents: true
//          isLeague: false
//          beforeDate: 1627698617
//          afterDate: 1625884217
//      }
//  }) {
//      nodes {
//          id
//          name
//          countryCode
//          isOnline
//          slug
//          startAt
//          isRegistrationOpen
//          hashtag
//          events(
//              filter: {
//                  videogameId: 33945
//              }) {
//                  id
//                  name
//                  isOnline
//                  numEntrants
//                  entrantSizeMax
//                  startAt
//                  videogame {
//                      id
//                      name
//                  }
//              }
//              streams {
//                  id
//                  isOnline
//                  streamName
//                  streamSource
//              }
//          }
//      }
//  }