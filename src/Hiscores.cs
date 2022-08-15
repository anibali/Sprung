using System;
using System.Collections.Generic;
using System.IO;

namespace Sprung
{
    public class Hiscores
    {
        private static string path = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "sprung"), "hiscores");

        public static List<Score> Load()
        {
            List<Score> scores = new List<Score>();
            
            if(File.Exists(path))
            {
                foreach(string line in File.ReadAllLines(path))
                {
                    string[] parts = line.Split(' ');
                    
                    try
                    {
                        if(parts.Length == 3)
                            scores.Add(new Score(int.Parse(parts[0]), int.Parse(parts[1]), DateTime.FromFileTime(long.Parse(parts[2]))));
                    }
                    catch(FormatException ex)
                    {
                    }
                }
            }
            
            return scores;
        }
        
        public static int ProcessScore(int number, int floor)
        {
            List<Score> scores = Load();
            Score score = new Score(number, floor, DateTime.Now);
            scores.Add(score);
            scores.Sort(new ScoreComparer());
            if(scores.Count > 10) scores.RemoveRange(10, scores.Count - 10);
            
            string[] lines = new string[10];
            for(int i = 0; i < scores.Count; i++)
            {
                Score hiscore = scores[i];
                lines[i] = String.Format("{0:d} {1:d} {2:d}", hiscore.Number, hiscore.Floor, hiscore.Time.ToFileTime());
            }
            File.WriteAllLines(path, lines);
            
            return(scores.IndexOf(score) + 1);
        }
        
        public class Score
        {
            private int number, floor;
            private DateTime time;
            
            public int Number { get { return number; } }
            public int Floor { get { return floor; } }
            public DateTime Time { get { return time; } }
            
            public Score(int number, int floor, DateTime time)
            {
                this.number = number;
                this.time = time;
                this.floor = floor;
            }
        }
        
        class ScoreComparer : IComparer<Score>
        {
            public int Compare(Score x, Score y)
            {
                return(-x.Number.CompareTo(y.Number));
            }
        }
    }
}
