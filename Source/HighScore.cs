﻿using System;
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
            _bestScores = new List<int>{0,5550,150,50,50};
            LoadFromFile();
        }

        public void SortHighScore()
        {
            _bestScores.Sort((a, b) => a.CompareTo(b));
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
            var isoStream = new IsolatedStorageFileStream(Constants.HighScoreXml, FileMode.Create, _isoStore)
            var writer = new StreamWriter(isoStream);

            serializer.Serialize(writer, _bestScores);
            writer.Close();
        }
    }
}