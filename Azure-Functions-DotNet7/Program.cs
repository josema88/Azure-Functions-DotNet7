using Azure_Functions_DotNet7.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(s =>
    {
        s.AddTransient<IDepartmentsRepository>(
            x => new DepartmentsRepository(Environment.GetEnvironmentVariable("MyDbConnection") ?? ""));
    })
    .Build();

host.Run();

