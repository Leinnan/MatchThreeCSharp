using System;
using System.IO;
using System.Collections.Generic;

namespace MonoGameCore
{
    public class HighScore
    {
        public List<int> _bestScores { get; set; };
        
        public HighScore()
        {
            _bestScores = new List<int>{0,0,0,0,0,0,0,0,0,0};
            LoadFromFile();
        }

        public void SortHighScore()
        {
            _bestScores.OrderByDescending();
        }
        
        public void LoadFromFile()
        {
            if( !File.Exist("myFileName.xml")
            {
                SaveToFile();
                return;
            }
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<int>));
            var myFileStream = new FileStream("myFileName.xml", FileMode.Open);
            
            _bestScores = (List<int>)serializer.Deserialize(myFileStream)
            myFileStream.Close();
        }

        public void SaveToFile()
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<int>));
            var writer = new StreamWriter("myFileName.xml");

            mySerializer.Serialize(writer, _bestScores);
            writer.Close();
        }
    }
}