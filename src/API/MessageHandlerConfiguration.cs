using MessageHandler.EventSourcing;
using MessageHandler.EventSourcing.AzureTableStorage;
using MessageHandler.EventSourcing.Outbox;
using MessageHandler.Runtime;
using MessageHandler.Runtime.TransactionalProcessing;

namespace MessageHandler.Samples.EventSourcing.AggregateRoot.API
{
    public static class MessageHandlerConfiguration
    {
        public const string EventSourceTableName = "orderbooking";
        public const string EventSourceStreamTypeName = "OrderBooking";
        public const string HandlerName = "orderbooking";
        public const string TopicName = "orderbooking.events";

        public static HandlerRuntimeConfiguration AddHandlerRuntime(this IServiceCollection services, IConfiguration configuration)
        {
            var runtimeConfiguration = new HandlerRuntimeConfiguration(services);
            runtimeConfiguration.HandlerName(HandlerName);

            return runtimeConfiguration;
        }

        public static IServiceCollection AddEventSource(this IServiceCollection services, HandlerRuntimeConfiguration runtimeConfiguration, IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("azurestoragedata")
                                        ?? throw new Exception("No 'azurestoragedata' connection string was provided. Use User Secrets or specify via environment variable.");

            string serviceBusConnectionString = configuration.GetValue<string>("servicebusnamespace")
                                        ?? throw new Exception("No 'servicebusnamespace' connection string was provided. Use User Secrets or specify via environment variable.");

            var eventsourcingConfiguration = new EventsourcingConfiguration(runtimeConfiguration);
            var eventSource = new AzureTableStorageEventSource(connectionString, EventSourceTableName);

            eventsourcingConfiguration.UseEventSource(eventSource);
            eventsourcingConfiguration.EnableOutbox(EventSourceStreamTypeName, HandlerName, pipeline =>
            {
                pipeline.RouteMessages(to => to.Topic(TopicName, serviceBusConnectionString));
            });

            eventsourcingConfiguration.RegisterEventsourcingRuntime();

            return services;
        }        

        public static HandlerRuntimeConfiguration AddHandlerRuntimeStartup(this IServiceCollection services, HandlerRuntimeConfiguration runtimeConfiguration)
        {
            runtimeConfiguration.UseHandlerRuntime();

            return runtimeConfiguration;
        }
    }
}
