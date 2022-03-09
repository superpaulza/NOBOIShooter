using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace NOBOIShooter.Controls
{
    public class Score
    {
            public int Highscore { get; set; }
            public DateTime HighscoreDate { get; set; }
    
    }
    class ScoreData
    {
        private const string SAVE_FILE_NAME = "sav.dat";
        public List<Score> ScoresTables { get; set; }

        public void Add(Score score)
        {
            ScoresTables.Add(score);
        }

        public void Sort()
        {
            ScoresTables.Sort();
        }

        public void SaveGame()
        {


            try
            {
                using (FileStream fileStream = new FileStream(SAVE_FILE_NAME, FileMode.Create))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(fileStream, ScoresTables);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred while saving the game: " + ex.Message);
            }

        }

        public void LoadSaveState()
        {
            try
            {
                using (FileStream fileStream = new FileStream(SAVE_FILE_NAME, FileMode.OpenOrCreate))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    ScoresTables = binaryFormatter.Deserialize(fileStream) as List<Score>;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred while loading the game: " + ex.Message);
            }
        }
    }
}
