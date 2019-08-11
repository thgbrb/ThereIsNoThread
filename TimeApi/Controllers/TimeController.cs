using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;

namespace TimeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeController : ControllerBase
    {
        private volatile static int _requestCounter = 0;

        public string Get()
        {
            Interlocked.Increment(location: ref _requestCounter);

            var delay = new Random().Next(
                minValue: 10000,
                maxValue: 20000);

            var time = DateTime.Now.ToString(format: "dddd, dd MMMM yyyy HH:mm:ss");

            Thread.Sleep(millisecondsTimeout: delay);

            return time;
        }

        [Route("threadpoolinformation")]
        [HttpGet]
        public string ThreadPoolInformation()
        {
            ThreadPool.GetAvailableThreads(
                workerThreads: out int wt,
                completionPortThreads: out int cp);

            return $"Threads/IOCP Available: {wt}/{cp} | Requests Received: {_requestCounter}";
        }
    }
}