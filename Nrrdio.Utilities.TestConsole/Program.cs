using Microsoft.Extensions.Hosting;
using Nrrdio.Utilities.TestConsole;

await DatabaseLoggerTestHost.CreateHostBuilder(args).Build().RunAsync();