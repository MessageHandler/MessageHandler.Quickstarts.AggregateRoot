# Designing an Aggregate Root

This project is part of the [MessageHandler processing patterns](https://www.messagehandler.net/patterns/) library.

MessageHandler is distributed under a commercial license, for more information on the terms and conditions refer to [our license page](https://www.messagehandler.net/license/).

## What is an Aggregate Root

An aggregate is a cluster of domain objects that is treated as a single unit. Any references from outside the aggregate should only go to the aggregate root. 
The root can thus ensure the integrity of the aggregate as a whole.

As an aggregate root is responsible for maintaining integrity of the whole, it is the primary responsible for deciding if a command can be executed on the aggregate or not.

This decission will be recorded as an event on an event stream and any internal state is updated in response to the recorded event.

![Aggregate Root](./img/aggregate-root.jpg)

## When to use it

Use this pattern every time a user, or an automated part of the system, wants to make a change.

Encapsulate the intent into a command and send it to the aggregate root, so that it can take a decission on how to respond to the intent.

## Scenario

The scenario for this quickstart is e-commerce process, where you can place a booking through an API, and the aggregate root will decide if it is a valid purchase order or not.

## What you need to get started

- The [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download) should be installed
- The sample was created using [Visual Studio 2022 community edition](https://visualstudio.microsoft.com/vs/)
- A general purpose [azure storage account](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-create?tabs=azure-portal) is used to store events.
- To use the outbox an [azure service bus namespace](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-create-namespace-portal) or [eventhub](https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-create) is required.
- The **MessageHandler.EventSourcing.AzureTableStorage** package is available from [nuget.org](https://www.nuget.org/packages/MessageHandler.EventSourcing.AzureTableStorage/)
- The optional **MessageHandler.EventSourcing.Outbox** package is also available from [nuget.org](https://www.nuget.org/packages/MessageHandler.EventSourcing.Outbox/)

## Running the sample

Prior to being able to run the sample, you need to [configure the user secrets file](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows#manage-user-secrets-with-visual-studio).

In the secrets file you must specify the following configuration values.

```JSON
{
  "azurestoragedata": "your azure storage connection string goes here",
  "servicebusnamespace": "your azure service bus connection string goes here"
}
```

Also ensure a topic named `orderbooking.events` is created up front in the service bus namespace.

Once configured you can start the API project or run the unittests.

## Designed with testing in mind

MessageHandler is intented to be test friendly.

This sample contains plenty of ideas on how to test an aggregate root without requiring a dependency on an actual storage account or servicebus namespace, and thus keep the tests fast.

- [Unit tests](/src/Tests/UnitTests): To test the actual logic in the aggregate root. Unit tests should make up the bulk of all tests in the system.
- [Component tests](/src/Tests/ComponentTests): To test the api used to expose the aggregate root.
- [Contract tests](/src/Tests/ContractTests): To verify that the test doubles used in the unit and component tests are behaving the same as an actual dependency would. Note: contract verification files are often shared between producers and consumers of the contract.

## How to implement it yourself

Check out [this how to guide](https://www.messagehandler.net/docs/guides/event-sourcing/aggregate-root/) to learn how to implement this pattern.