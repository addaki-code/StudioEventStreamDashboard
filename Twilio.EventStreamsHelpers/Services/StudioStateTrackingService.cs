using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Twilio.EventStreamsHelpers.Services
{
    public class StudioStateTrackingService
    {

        public ConcurrentQueue<EventStreamEvent> Events { get; set; } = new ConcurrentQueue<EventStreamEvent>();

        public List<ExecutionStartedPayload> ExecutionsInProgress = new List<ExecutionStartedPayload>();
        public List<ExecutionEndedPayload> ExecutionsCompleted = new List<ExecutionEndedPayload>();
        public List<StepEndedPayload> Steps = new List<StepEndedPayload>();

        public void Reset()
        {
            Events.Clear();
            ExecutionsInProgress.Clear();
            ExecutionsCompleted.Clear();
            Steps.Clear();
        }

        public void ProcessEvent(EventStreamEvent eventToBeProcessed)
        {
            Events.Append(eventToBeProcessed);

            switch (eventToBeProcessed.Type)
            {
                case EventTypes.Studio.ExecutionStarted:
                {
                    var executionData = eventToBeProcessed.ParsedData as ExecutionStartedPayload;
                    ExecutionsInProgress.Add(executionData);
                    return;
                }

                case EventTypes.Studio.ExecutionEnded:
                {
                    var executionData = eventToBeProcessed.ParsedData as ExecutionEndedPayload;
                    ExecutionsCompleted.Add(executionData);
                    ExecutionsInProgress.RemoveAll(d => d.ExecutionSid == executionData.ExecutionSid);
                    return;
                }

                case EventTypes.Studio.StepEnded:
                {
                    var executionData = eventToBeProcessed.ParsedData as StepEndedPayload;
                    Steps.Add(executionData);
                    return;
                }
            }
        }
    }
}
