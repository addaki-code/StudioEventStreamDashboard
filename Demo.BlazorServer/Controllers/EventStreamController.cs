using System;
using System.Threading.Tasks;
using Demo.BlazorServer.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Twilio.EventStreamsHelpers;
using Twilio.EventStreamsHelpers.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Demo.BlazorServer.Controllers
{
    public class EventStreamController : Controller
    {
        private readonly IHubContext<PushHub> _pushHubContext;
        private readonly StudioStateTrackingService _studioStateTrackingService;

        public EventStreamController(IHubContext<PushHub> hubContext, StudioStateTrackingService studioStateTrackingService)
        {
            _pushHubContext = hubContext;
            _studioStateTrackingService = studioStateTrackingService;
        }

        // POST: /<controller>/
        [HttpPost]
        public async Task<IActionResult> EventStreamWebhook([FromBody] object[] payload)
        {
            // Super not thread safe. Needs work, perhaps a ConcurrentQueue with background process popping records one at a time. 
            foreach(var e in payload)
            {
                var eventEnvelope = JsonConvert.DeserializeObject<EventStreamEvent>(e.ToString());

                switch (eventEnvelope.Type + "v" + eventEnvelope.SpecVersion)
                {
                    case EventTypes.Studio.StepEnded + "v1.0":
                        eventEnvelope.ParsedData = JsonConvert.DeserializeObject<StepEndedPayload>(eventEnvelope.Data.ToString());
                        break;
                    case EventTypes.Studio.ExecutionStarted + "v1.0":
                        eventEnvelope.ParsedData = JsonConvert.DeserializeObject<ExecutionStartedPayload>(eventEnvelope.Data.ToString());
                        break;
                    case EventTypes.Studio.ExecutionEnded + "v1.0":
                        eventEnvelope.ParsedData = JsonConvert.DeserializeObject<ExecutionEndedPayload>(eventEnvelope.Data.ToString());
                        break;
                    default:
                        Console.WriteLine("Event not fully parsed: Unknown Type + Version combo: " + eventEnvelope.Type + ":" + eventEnvelope.SpecVersion);
                        break;
                }

                _studioStateTrackingService.ProcessEvent(eventEnvelope);
            }

            await _pushHubContext.Clients.All.SendAsync("Redraw");
            return Ok();
        }


    }
}
