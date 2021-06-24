using System;

namespace Twilio.EventStreamsHelpers
{
    public class EventStreamEvent
    {
        public string SpecVersion { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public string Id { get; set; }
        public string DataSchema { get; set; }
        public string DataContentType { get; set; }
        public DateTimeOffset Time { get; set; }
        public object Data { get; set; }
        public IEventPayload ParsedData { get; set; }
    }
}
