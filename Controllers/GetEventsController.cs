using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmashGGEventTicker.ViewModels;
using static SmashGGEventTicker.DataConverters;

// user visits page, page makes API call
// Call checks for data in cache, if no data, get it and make it, otherwise return the data available. Only get the data if the time has been longer than 30 seconds

namespace SmashGGEventTicker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetEventsController : ControllerBase
    {

        private readonly ILogger<GetEventsController> _logger;
        private readonly SmashGGInMemoryCache _cache;

        public GetEventsController(ILogger<GetEventsController> logger, SmashGGInMemoryCache cache)
        {
            _cache = cache;
            _logger = logger;
        }

        [HttpGet]
        public List<EventView> Get(int videogameId)
        {
            SmashGGResponse response = _cache.TryGetCachedData(videogameId).Result;
            _logger.LogInformation($"Tournament Name: {response.Data.Tournaments.Nodes.First().Name}");
            var e = ResponseToEventView(response);
            return e;
        }
    }
}
