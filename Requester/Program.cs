using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Requester
{
    internal class Program
    {
        private const string PORTAL_URL_SYNC = "http://localhost:5000/Home/Index";
        private const string PORTAL_URL_ASYNC = "http://localhost:5000/Home/IndexAsync";
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly List<Task> requestsAsync = new List<Task>();
        private static readonly List<Task> requestsSync = new List<Task>();
        private static readonly CancellationTokenSource source = new CancellationTokenSource();
        private static readonly CancellationToken token = source.Token;

        private static void Main(string[] args)
        {
            Console.CancelKeyPress += (o, e) => source.Cancel();

            do
            {
                Menu();

                var key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.S:
                        Console.WriteLine("ync Call");
                        requestsSync.Add(item: DoRequest(url: PORTAL_URL_SYNC));
                        NotifyEnqueuedRequestSync();
                        break;

                    case ConsoleKey.A:
                        Console.WriteLine("sync Call");
                        requestsAsync.Add(item: DoRequest(url: PORTAL_URL_ASYNC));
                        NotifyEnqueuedRequestAsync();
                        break;

                    case ConsoleKey.W:
                        Console.WriteLine("ait All Tasks");
                        Task.WaitAll(tasks: requestsSync.ToArray(), cancellationToken: token);
                        Task.WaitAll(tasks: requestsAsync.ToArray(), cancellationToken: token);
                        requestsAsync.Clear();
                        break;

                    default:
                        Console.Clear();
                        break;
                }
            }
            while (!token.IsCancellationRequested);
        }

        public static Task DoRequest(string url)
            => Task.Run(async () =>
            {
                try
                {
                    await httpClient.GetStringAsync(requestUri: url);

                    Console.WriteLine($"{DateTime.Now} - Task completed.");
                }
                catch (System.Exception)
                {
                    Console.WriteLine($"{DateTime.Now} - Error - Inner Exception.");
                }
            });

        public static void NotifyEnqueuedRequestAsync()
            => Console.WriteLine(
                value: $"{DateTime.Now} - Request Async Enqueued");

        public static void NotifyEnqueuedRequestSync()
            => Console.WriteLine(
                value: $"{DateTime.Now} - Request Sync Enqueued");

        private static void Menu()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("S = Sync controller, A = Async controller, W = Wait task array, C = Clear screen");
            Console.WriteLine($"Total of requests Sync: {requestsSync.Count}");
            Console.WriteLine($"Total of requests Async: {requestsAsync.Count}");
            Console.WriteLine(Environment.NewLine);
        }
    }
}