using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using ZodiacClient;

namespace ZodiacClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Horoscope.HoroscopeClient(channel);

            //var responseGetAll = await client.GetAllZodiacsAsync(new Empty());
            //Console.WriteLine("Intervals from server: " + responseGetAll);

            var cancellationToken = new CancellationTokenSource(Timeout.Infinite);

            while (!cancellationToken.IsCancellationRequested)
            {
                Console.Write("Date: ");

                var date = Console.ReadLine();

                if (!ValidateDate(date)) { return; }

                var zodiacToBeAdded = new Zodiac()
                { Date = date != null && date.Trim().Length > 0 ? date : "Invalid Date" };
                var response = await client.AddZodiacAsync(new AddZodiacRequest { Zodiac = zodiacToBeAdded });

                switch (response.Status)
                {
                    case AddZodiacResponse.Types.Status.Success:
                        Console.WriteLine($"\nResponse Status: {response.Status}\n\nSign: {response.Sign}\n");
                        break;
                    case AddZodiacResponse.Types.Status.Error:
                        Console.WriteLine($"\nResponse Status: {response.Status}\n\nInvalid Date!");
                        break;
                    default:
                        Console.WriteLine($"\nResponse Status: {response.Status}\n\nSomething Went Wrong!");
                        break;
                }
            }
        }

        public static bool ValidateDate(string date)
        {
            var month = int.Parse(date.Substring(0, 2));
            var day = int.Parse(date.Substring(3, 2));
            var year = int.Parse(date.Substring(6, 4));

            try
            {
                var dateTime = new DateTime(year, month, day);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Invalid Date - Does Not Exist!");
                return false;
            }

            var regex = new Regex("([0-9]{2})/([0-9]{2})/([0-9]{4})");
            var match = regex.Match(date);

            return match.Success;
        }
    }
}