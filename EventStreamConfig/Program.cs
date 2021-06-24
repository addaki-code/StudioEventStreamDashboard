using System;
using System.Collections.Generic;
using Twilio;
using Twilio.EventStreamsHelpers;
using Twilio.Rest.Events.V1;

namespace EventStreamConfig
{
    class Program
    {

        // Accessors for Environment Variables
        public static string AccountSid => Environment.GetEnvironmentVariable("AccountSid"); 
        public static string AuthToken => Environment.GetEnvironmentVariable("AuthToken");
        public static string WebHookDestination => Environment.GetEnvironmentVariable("WebhookDestination");

        static void Main(string[] args)
        {

            // Configure Twilio SDK
            TwilioClient.Init(AccountSid, AuthToken);

            /*  HERE BE DRAGONS 
             *  Uncomment the below to trash any existing Subscriptions/Sinks in the account first.
             *  Be super careful with this if running on an account already using Event Streams.
            */
            DeleteExistingSinks();

            Console.WriteLine("Creating new Webhook Sink with target " + WebHookDestination);


            // Define Sync Config
            var syncConfig = new Dictionary<string, object>()
            {
                { "destination", WebHookDestination },
                { "method", "POST" },
                { "batch_events", false }
            };


            // Create Sink
            var sink = SinkResource.Create(
                description: "Studio Webhook Sink", 
                sinkConfiguration: syncConfig,
                sinkType: SinkResource.SinkTypeEnum.Webhook
            );

            Console.WriteLine(sink.Sid + " created");

            // Define the list of events that we want to subscribe to
            var types = new List<object> {
                new Dictionary<string, string>()
                {
                    { "type", EventTypes.Studio.ExecutionStarted }
                },
                new Dictionary<string, string>()
                {
                    { "type", EventTypes.Studio.ExecutionEnded }
                },
                new Dictionary<string, string>()
                {
                    { "type", EventTypes.Studio.StepEnded }
                }
            };

            // Setup subscription
            var subscription = SubscriptionResource.Create(
                description: "Studio Webhook Subscription",
                sinkSid: sink.Sid,
                types: types
            );

            Console.WriteLine("Subscription Created " + subscription.Sid);


        }

        /// <summary>
        /// Loops through all Sinks and Subscriptions on an account and deletes them.
        /// Be SUPER careful if using this
        /// </summary>
        private static void DeleteExistingSinks()
        {
            Console.WriteLine("Clearing old Sinks from Account...");

            var sinks = SinkResource.Read();
            foreach (var record in sinks)
            {
                var subs = SubscriptionResource.Read(sinkSid: record.Sid);
                foreach(var sub in subs)
                {
                    Console.WriteLine("Deleting Subscription " + sub.Sid);
                    SubscriptionResource.Delete(sub.Sid);
                    Console.WriteLine("Deleted");
                }

                Console.WriteLine("Deleting " + record.Sid);
                SinkResource.Delete(record.Sid);
            }

            Console.WriteLine("...Old sinks deleted");



        }
    }
}
