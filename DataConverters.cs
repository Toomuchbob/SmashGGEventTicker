using SmashGGEventTicker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmashGGEventTicker
{
    public static class DataConverters
    {
        public static List<EventView> ResponseToEventView(SmashGGResponse response)
        {
            var eventList = new List<EventView>();

            foreach (var t in response.Data.Tournaments.Nodes)
            {
                foreach (var e in t.Events)
                {
                    var eventView = new EventView()
                    {
                        EventName = t.Name,
                        StartTime = DateTimeOffset.FromUnixTimeSeconds(e.StartAt).ToUnixTimeMilliseconds(),
                        StreamLink = t.Streams == null ? null : t.Streams.FirstOrDefault().StreamName,
                        Hashtag = t.Hashtag,
                        Entrants = e.NumEntrants.HasValue ? e.NumEntrants.Value : 0,
                        Slug = t.Slug,
                        IsRegistrationOpen = t.IsRegistrationOpen
                    };
                    eventList.Add(eventView);
                }
            }
            eventList.Sort((x, y) => x.StartTime.CompareTo(y.StartTime));

            return eventList;
        }
    }
}
