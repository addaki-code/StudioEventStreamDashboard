using System;
namespace Twilio.EventStreamsHelpers
{
    public struct EventTypes
    {
        public struct Studio
        {
            public const string ExecutionStarted = "com.twilio.studio.flow.execution.started";
            public const string ExecutionEnded = "com.twilio.studio.flow.execution.ended";
            public const string StepEnded = "com.twilio.studio.flow.step.ended";
        }
    }
}
