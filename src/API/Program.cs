using MessageHandler.Samples.EventSourcing.AggregateRoot.API;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
                 .AddEnvironmentVariables()
                 .AddUserSecrets<Program>()
                 .Build();

// Add services to the container.

var runtimeConfiguration = builder.Services.AddHandlerRuntime(configuration);
builder.Services.AddEventSource(runtimeConfiguration, configuration);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHandlerRuntimeStartup(runtimeConfiguration);

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
