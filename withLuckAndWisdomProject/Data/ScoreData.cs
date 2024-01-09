using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using withLuckAndWisdomProject.Data;

namespace withLuckAndWisdomProject.Data
{
    [Serializable]
    public class Score
    {
        public int ScoreGet { get; set; }
        public float Distance { get; set; }
        public TimeSpan TimePlay { get;  set; }

        public Score(int score, float distance, TimeSpan time)
        {
            ScoreGet = score;
            TimePlay = time;
            Distance = distance;
        }
    
    }
    class ScoreData
    {
        private const string SAVE_FILE_NAME = "sav.dat";
        private const int SET_MAX_SAVE_SCORE = 50;

        public List<Score> ScoresTables { get; private set; }

        public ScoreData ()
        {
            if (ScoresTables == null)
                ScoresTables = new List<Score>();
            LoadSave();
        }
        public async void Add(Score score)
        {
            ScoresTables.Add(score);
            if (Singleton.Instance.IsShareDataToDev)
            {
                await Task.Run(() => ShareData.RunAsync(score));
            }
        }

        public void Sort()
        {
            ScoresTables.Sort(delegate(Score x, Score y) {
                return y.ScoreGet.CompareTo(x.ScoreGet);
            });

            if (ScoresTables.Count > SET_MAX_SAVE_SCORE)
                ScoresTables.RemoveRange(SET_MAX_SAVE_SCORE, ScoresTables.Count - SET_MAX_SAVE_SCORE);
        }

        public void SaveGame()
        {

            try
            {
                using FileStream fileStream = new FileStream(SAVE_FILE_NAME, FileMode.Create);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, ScoresTables);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred while saving the game: " + ex.Message);
            }

        }

        public void LoadSave()
        {
            try
            {
                using FileStream fileStream = new FileStream(SAVE_FILE_NAME, FileMode.OpenOrCreate);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                ScoresTables = binaryFormatter.Deserialize(fileStream) as List<Score>;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred while loading the game: " + ex.Message);
            }
        }
    }
}
