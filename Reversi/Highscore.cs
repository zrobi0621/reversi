using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi
{
    public class Highscore
    {
        public string WhitePlayer { get; set; }
        public string BlackPlayer { get; set; }
        public string Time { get; set; }
        public int WhitePoints { get; set; }
        public int BlackPoints { get; set; }
        public string Date { get; set; }

        public Highscore()
        {

        }

        public Highscore(string whitePlayer, string blackPlayer, string time, int whitePoints, int blackPoints, string date)
        {
            WhitePlayer = whitePlayer;
            BlackPlayer = blackPlayer;
            Time = time;
            WhitePoints = whitePoints;
            BlackPoints = blackPoints;
            Date = date;
        }

        public override string ToString()
        {
            return $"{WhitePlayer} {BlackPlayer} {Time} {WhitePoints} {BlackPoints} {Date}";
        }
    }
}
