using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PingApp.Models;

public class PingService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private static PingResult _latestPingResult = new PingResult { LatencyMs = 0, Timestamp = DateTime.Now };

    public PingService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public static PingResult LatestPingResult => _latestPingResult;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://my.la.gov");
            }

            stopwatch.Stop();
            var latency = stopwatch.ElapsedMilliseconds;

            _latestPingResult.LatencyMs = latency;
            _latestPingResult.Timestamp = DateTime.Now;

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
