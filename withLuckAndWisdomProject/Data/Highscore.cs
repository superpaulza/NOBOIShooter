using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using withLuckAndWisdomProject.Manager;

namespace withLuckAndWisdomProject.Data
{
    public static class Communications
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<Highscore> getHighscore()
        {
            Highscore data = null;

            try
            {
                string responseBody;

                HttpResponseMessage response = await client.PostAsync("Data/ScoreTable", new StringContent(""));
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return data;
        }

        public static void Initialize()
        {
            client.BaseAddress = new Uri("http://localhost:5000/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }



    }

    public class Highscore
    {
        public List<Score> ScoresTables { get; private set; }
    }
}
