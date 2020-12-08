using Microsoft.Extensions.DependencyInjection;
using Nrrdio.Utilities;

var services = new ServiceCollection();
services.AddScoped<JsonFileLogger>();

var logger = services
    .BuildServiceProvider()
    .GetService<JsonFileLogger>();

