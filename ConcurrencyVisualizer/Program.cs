using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConcurrencyVisualizer
{
    class Program
    {
        const string PORTAL_URL_SYNC = "http://localhost:5000/Home/Index";
        const int REQUESTS = 4;

        static readonly HttpClient httpClient = new HttpClient();

        static void Main(string[] args)
        {
            Async();

            Console.WriteLine("Done");
        }

        public static void Sequencial()
        {
            for (int i = 0; i < REQUESTS; i++)
            {
                using (var webClient = new WebClient())
                {
                    var result = webClient.DownloadString(address: PORTAL_URL_SYNC);
                }
            }
        }

        public static void ParallelLooping()
        {
            Parallel.For(fromInclusive: 0, toExclusive: REQUESTS, body: (i) =>
            {
                using (var webClient = new WebClient())
                {
                    var result = webClient.DownloadString(address: PORTAL_URL_SYNC);
                }
            });
        }

        public static void Async()
        {
            var tasks = new Task[REQUESTS];
            var httpClient = new HttpClient();

            for (int i = 0; i < REQUESTS; i++)
            {
                tasks[i] = httpClient.GetStringAsync(requestUri: PORTAL_URL_SYNC);
            }

            Task.WaitAll(tasks: tasks);
        }
    }
}
