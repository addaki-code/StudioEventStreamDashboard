using Newtonsoft.Json;

namespace Twilio.EventStreamsHelpers
{
    public abstract class StudioExecutionPayloadBase : StudioEventPayloadBase
    {
        [JsonProperty("started_by")]
        public string StartedBy { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("flow_revision")]
        public int FlowRevision { get; set; }

        [JsonProperty("contact_channel_address")]
        public string ContactChannelAddress { get; set; }
    }


}
