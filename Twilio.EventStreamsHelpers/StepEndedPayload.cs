using Newtonsoft.Json;

namespace Twilio.EventStreamsHelpers
{

    public class StepEndedPayload : StudioEventPayloadBase
    {

        [JsonProperty("step_sid")]
        public string StepSid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("transitioned_to")]
        public string TransitionedTo { get; set; }

        [JsonProperty("transitioned_from")]
        public string TransitionedFrom { get; set; }
    }


}
