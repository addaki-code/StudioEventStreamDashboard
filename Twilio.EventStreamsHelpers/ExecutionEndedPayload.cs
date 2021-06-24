using System;
using Newtonsoft.Json;

namespace Twilio.EventStreamsHelpers
{
    public class ExecutionEndedPayload : StudioExecutionPayloadBase
    {

        [JsonProperty("date_updated")]
        public DateTimeOffset DateUpdated { get; set; }

        [JsonProperty("ended_reason")]
        public string EndedReason { get; set; }
    }


}
