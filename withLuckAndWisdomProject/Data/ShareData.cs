using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace withLuckAndWisdomProject.Data
{
    class ShareData
    {
        static HttpClient client = new HttpClient();

        // static async Task<Uri> AddScoreToDatabase(Score score)
        // {
        //
        //     HttpResponseMessage response = await client.PostAsync(
        //         "sharescoredata.php", new StringContent(data, Encoding.UTF8));
        //     System.Diagnostics.Debug.WriteLine(data);
        //     response.EnsureSuccessStatusCode();
        //
        //     // return URI of the created resource.
        //     return response.Headers.Location;
        // }

        public static async Task RunAsync(Score score)
        {
            var ip = await client.GetStringAsync("https://api.ipify.org");
            string data =
                "{\"ScoreGet\":\"" + score.ScoreGet + "\"," +
                "\"Distance\":\"" + score.Distance + "\"," +
                "\"TimePlay\":\"" + score.TimePlay.ToString(@"mm\:ss") + "\"," +
                "\"Timestamp\":\"" + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() + "\"," +
                "\"UserName\":\"" + Environment.UserName + "\"," +
                "\"MachineName\":\"" + Environment.MachineName + "\"," +
                "\"IPAddress\":\"" + ip + "\"," +
                "\"OSPlatform\":\"" + Environment.OSVersion.Platform.ToString() + "\"}";

            var request = new HttpRequestMessage(HttpMethod.Post, "https://prynt.000webhostapp.com/sharescoredata.php");
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = new StringContent(data, Encoding.UTF8, "application/json");
            // client.BaseAddress = new Uri("https://prynt.000webhostapp.com/");
            // client.DefaultRequestHeaders.Accept.Clear();
            // client.DefaultRequestHeaders.Accept.Add(
            //     new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                //
                // var url = await AddScoreToDatabase(score);
                // Console.WriteLine($"Created at {url}");

                var response = await client.SendAsync(request, CancellationToken.None);
                response.EnsureSuccessStatusCode();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
