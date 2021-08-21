using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmashGGEventTicker.ViewModels
{
    public class EventView
    {
        public string EventName { get; set; }
        public long StartTime { get; set; }
        public string StreamLink { get; set; }
        public string Hashtag { get; set; }
        public int Entrants { get; set; }
        public string Slug { get; set; }
        public bool IsRegistrationOpen { get; set; }

    }
}
