using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmashGGEventTicker.ViewModels;
using static SmashGGEventTicker.DataConverters;

namespace SmashGGEventTicker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetEventsController : ControllerBase
    {

        private readonly ILogger<GetEventsController> _logger;

        public GetEventsController(ILogger<GetEventsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<EventView> Get()
        {
            SmashGGResponse response = SmashGGInMemoryCache.Instance.SmashGGResponse;
            _logger.LogInformation($"Tournament Name: {response.Data.Tournaments.Nodes.First().Name}");
            var e = ResponseToEventView(response);
            return e;
        }
    }
}
