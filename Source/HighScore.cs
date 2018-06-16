using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;

namespace MonoGameCore
{
    public class HighScore
    {
        public List<int> _bestScores { get; set; }
        IsolatedStorageFile _isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
        
        public HighScore()
        {
            _bestScores = new List<int>{10000,8000,6000,4000,20000,5000000,11111111,78011111,41312321,51412};
            LoadFromFile();
            SortHighScore();
        }

        void SortHighScore()
        {
            _bestScores.Sort((a, b) => b.CompareTo(a));
        }
        
        public void LoadFromFile()
        {
            if( !_isoStore.FileExists(Constants.HighScoreXml) )
            {
                SaveToFile();
                return;
            }
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<int>));
            
            var isoStream =
                new IsolatedStorageFileStream(Constants.HighScoreXml, FileMode.Open, _isoStore);
            
            _bestScores = (List<int>)serializer.Deserialize(isoStream);
            isoStream.Close();
        }

        public void SaveToFile()
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<int>));
            var isoStream = new IsolatedStorageFileStream(Constants.HighScoreXml, FileMode.Create, _isoStore);
            var writer = new StreamWriter(isoStream);

            serializer.Serialize(writer, _bestScores);
            writer.Close();
        }

        public void HandleNewScore(int result)
        {
            _bestScores.Add(result);
            SortHighScore();
            _bestScores.RemoveAt(_bestScores.Count - 1);
            Console.Write(GetInStringFormat());
            SaveToFile();

        }

        public string GetInStringFormat()
        {
            string result = "HIGHSCORES\n";
            int counter = 1;
            foreach (var score in _bestScores)
            {
                result += Convert.ToString(counter) + ". " + Convert.ToString(score) + '\n';
                counter++;
            }

            return result;
        }
    }
}