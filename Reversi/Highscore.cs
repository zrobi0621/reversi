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
        public int Time { get; set; }
        public int WhitePoints { get; set; }
        public int BlackPoints { get; set; }

        public Highscore(string whitePlayer, string blackPlayer, int time, int whitePoints, int blackPoints)
        {
            WhitePlayer = whitePlayer;
            BlackPlayer = blackPlayer;
            Time = time;
            WhitePoints = whitePoints;
            BlackPoints = blackPoints;
        }

        public override string ToString()
        {
            return $"{WhitePlayer} {BlackPlayer} {Time} {WhitePoints} {BlackPoints}";
        }
    }
}
