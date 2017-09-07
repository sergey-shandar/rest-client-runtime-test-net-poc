using Microsoft.Rest.ClientRuntime.Test.JsonRpc;
using Microsoft.Rest.ClientRuntime.Test.TextRpc;
using Microsoft.Rest.ClientRuntime.Test.Utf8;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TestServer
{
    public class Program
    {
        public static async Task<int> MainAsync(string[] args)
        {
            var logPath = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "server.log");
            try
            {
                var input = Console.OpenStandardInput();
                var output = Console.OpenStandardOutput();
                var reader = new Reader(input);
                var writer = new Writer(output);
                var marshalling = new Marshalling(null, null);
                while (true)
                {
                    File.AppendAllLines(logPath, new[] { "Reading..." });
                    var request = await reader.ReadMessageAsync<Request>(marshalling);
                    File.AppendAllLines(logPath, new[] { $"Request: {request}" });
                    await writer.WriteMessageAsync(marshalling, Response.Create("0", request, null as Error<object>));
                }
            }
            catch (Exception e)
            {
                File.AppendAllLines(logPath, new[] { $"Error: {e}" });
                return -1;
            }
        }

        public static int Main(string[] args)
        {
            return Task.Run(() => MainAsync(args)).GetAwaiter().GetResult();
        }
    }
}
