using MessageHandler.EventSourcing;
using MessageHandler.EventSourcing.AzureTableStorage;
using MessageHandler.EventSourcing.Outbox;
using MessageHandler.Runtime;
using MessageHandler.Runtime.AtomicProcessing;
using MessageHandler.Quickstart.AggregateRoot;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
                 .AddEnvironmentVariables()
                 .AddUserSecrets<Program>()
                 .Build();

// Add services to the container.

var eventSourceTableName = nameof(OrderBooking);
var eventSourceStreamTypeName = nameof(OrderBooking);
var handlerName = "orderbooking";
var topicName = "orderbooking.events";

var storageConnectionString = builder.Configuration.GetValue<string>("azurestoragedata")
                                  ?? throw new Exception("No 'azurestoragedata' was provided. Use User Secrets or specify via environment variable.");

var serviceBusConnectionString = builder.Configuration.GetValue<string>("servicebusnamespace")
                               ?? throw new Exception("No 'servicebusnamespace' was provided. Use User Secrets or specify via environment variable.");

builder.Services.AddMessageHandler(handlerName, runtimeConfiguration =>
{   
    runtimeConfiguration.EventSourcing(source =>
    {
        source.Stream(eventSourceStreamTypeName,
            from => from.AzureTableStorage(storageConnectionString, eventSourceTableName),
            into =>
            {
                into.Aggregate<OrderBooking>()
                    .EnableOutbox(eventSourceStreamTypeName, handlerName, pipeline =>
                    {
                        pipeline.RouteMessages(to => to.Topic(topicName, serviceBusConnectionString));
                    });                
            });
    });
});

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
