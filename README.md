# Designing an Aggregate Root

This project is part of the [MessageHandler processing patterns](https://www.messagehandler.net/patterns/) library.

MessageHandler is distributed under a commercial license, for more information on the terms and conditions refer to [our license page](https://www.messagehandler.net/license/).

## What you need to get started

- The [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download) should be installed
- The sample was created using [Visual Studio 2022 communicty edition](https://visualstudio.microsoft.com/vs/)
- A general purpose [azure storage account](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-create?tabs=azure-portal) is used to store events.
- To use the outbox an [azure service bus namespace](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-create-namespace-portal) or [eventhub](https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-create) is required.
- The **MessageHandler.EventSourcing.AzureTableStorage** package is available from [nuget.org](https://www.nuget.org/packages/MessageHandler.EventSourcing.AzureTableStorage/)

## What is an Aggregate Root

An aggregate is a cluster of domain objects that can be treated as a single unit. Any references from outside the aggregate should only go to the aggregate root. 
The root can thus ensure the integrity of the aggregate as a whole.

As an aggregate root is responsible for maintaining integrity of the whole, it is the primary responsible for deciding if a command can be executed on the aggregate or not.

This decission will be recorded as an event on an event stream and any internal state is updated in response to the recorded event.

![Aggregate Root](./img/aggregate-root.jpg)

## Usage of the Aggregate Root pattern

Basic usage of the aggregate root pattern involves the following steps:
- Build up the history as a list of events
- Restore the aggregate from that history
- Invoke commands on the aggregate root, which will result in pending events
- Commit the pending events, which will make them part of the history

```C#
var processId = Guid.NewGuid().ToString();
var history = new List<SourcedEvent>();

var process = new RegistrationProcess(processId);
process.RestoreFrom(history);

process.Start();
process.Finish();

var pendingEvents = process.Commit();
```

## Loading and persisting the aggregate from and to Azure Table Storage

To load and persist an aggregate from a table in Azure Table Storage the following steps need to be performed:
- And AzureTableStorageEventSource instance needs to be configured with the proper connectionstring and tablename.
- And registered as an event source on an instance of `EventsourcingConfiguration`.
- When the configuration is finalized it can be used to instantiate a runtime instance.
- Which in turn can be used to resolve, among others, a repository (resolution is not needed when an IoC container would have been integrated in the configuration)

```C#
var eventsourcingConfiguration = new EventsourcingConfiguration();
eventsourcingConfiguration.UseEventSource(new AzureTableStorageEventSource(connectionString, tableName));
var runtime = EventsourcingRuntime.Create(eventsourcingConfiguration);
var repository = runtime.CreateAggregateRepository();
```

With a reference to the repository, aggregate instances can be restored, used, and flushed back to the event source.

```C#
var processId = Guid.NewGuid().ToString();
var process = await repository.Get<RegistrationProcess>(processId);

process.Start();
process.Finish();

await repository.Flush();
```
