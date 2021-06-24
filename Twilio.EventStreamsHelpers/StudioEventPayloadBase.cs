using System;
using Newtonsoft.Json;

namespace Twilio.EventStreamsHelpers
{
    public abstract class StudioEventPayloadBase : IEventPayload
    {
        [JsonProperty("date_created")]
        public DateTimeOffset DateCreated { get; set; }

        [JsonProperty("account_sid")]
        public string AccountSid { get; set; }

        [JsonProperty("flow_sid")]
        public string FlowSid { get; set; }

        [JsonProperty("execution_sid")]
        public string ExecutionSid { get; set; }
    }


}
