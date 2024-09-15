using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using PingApp.Models;

namespace PingApp.Controllers
{
    public class HomeController : Controller
    {
        private static PingResult _latestPingResult;

        public HomeController()
        {
            if (_latestPingResult == null)
            {
                _latestPingResult = new PingResult { LatencyMs = 0, Timestamp = DateTime.Now };
            }
        }

        public async Task<IActionResult> Index()
        {
            return View(_latestPingResult);
        }

        [HttpPost]
        public async Task<IActionResult> PingAmazon()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://www.amazon.com");
            }

            stopwatch.Stop();
            var latency = stopwatch.ElapsedMilliseconds;

            _latestPingResult.LatencyMs = latency;
            _latestPingResult.Timestamp = DateTime.Now;

            return RedirectToAction("Index");
        }
    }
}
