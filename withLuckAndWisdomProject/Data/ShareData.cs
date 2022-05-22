using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace withLuckAndWisdomProject.Data
{
    class ShareData
    {
        static HttpClient client = new HttpClient();

        static async Task<Uri> AddScoreToDatabase(Score score)
        {
            string data = 
                "{\"ScoreGet\":\"" + score.ScoreGet + "\","+
                "\"Distance\":\"" + score.Distance + "\"," +
                "\"TimePlay\":\"" + score.TimePlay.ToString(@"mm\:ss") + "\"," +
                "\"Timestamp\":\"" + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() + "\"," +
                "\"UserName\":\"" + Environment.UserName + "\"," +
                "\"MachineName\":\"" + Environment.MachineName + "\"," +
                "\"OSPlatform\":\"" + Environment.OSVersion.Platform.ToString() + "\"}";

            HttpResponseMessage response = await client.PostAsync(
                "sharescoredata.php", new StringContent(data, Encoding.UTF8));
            System.Diagnostics.Debug.WriteLine(data);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        public static async Task RunAsync(Score score)
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("https://prynt.000webhostapp.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var url = await AddScoreToDatabase(score);
                Console.WriteLine($"Created at {url}");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
