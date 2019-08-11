using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Portal.Controllers
{
    public class HomeController : Controller
    {
        private const string API_ADDRESS = "http://localhost:5001/api/time";

        private static HttpClient httpClient = new HttpClient();

        public IActionResult Index()
        {
            if (!CheckIfThereIsThreadAvailable())
                return BadRequest("No thread avaiable");

            // Simulate a blocking call
            var result = httpClient
                .GetStringAsync(requestUri: API_ADDRESS)
                .Result;

            ViewData["DateTimeNow"] = result;

            return View();
        }

        public async Task<IActionResult> IndexAsync()
        {
            if (!CheckIfThereIsThreadAvailable())
                return BadRequest("No thread avaiable");

            // Call without blocking
            var result = await httpClient.GetStringAsync(requestUri: API_ADDRESS);

            ViewData["DateTimeNow"] = result;

            return View();
        }

        public IActionResult Information()
        {
            ThreadPool.GetAvailableThreads(
                workerThreads: out int wt,
                completionPortThreads: out int cp);

            ViewData["ThreadPoolInfo"] = $"Threads/IOCP Available: {wt}/{cp}";

            return View();
        }

        private bool CheckIfThereIsThreadAvailable()
        {
            ThreadPool.GetAvailableThreads(
                workerThreads: out int wt,
                completionPortThreads: out int cp);

            return wt < Startup.MAX_WORKER_THREAD_AVAILABLE;
        }
    }
}