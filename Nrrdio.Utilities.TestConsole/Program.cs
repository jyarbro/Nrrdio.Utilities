using Microsoft.Extensions.Hosting;
using Nrrdio.Utilities.TestConsole;

await JsonFileLoggerTestHost.CreateHostBuilder(args).Build().RunAsync();