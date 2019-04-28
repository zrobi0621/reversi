using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Configuration;
using Dapper;

namespace Reversi
{
    class SQLiteDataAccess
    {
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        //LOAD
        public static List<Highscore> LoadHighscores()
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Query<Highscore>("SELECT * FROM Highscores", new DynamicParameters());
                return output.ToList();
            }
        }

        //INSERT
        public static void AddHighscore(Highscore highscore)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Execute("INSERT INTO Highscores (WhitePlayer, BlackPlayer, Time, WhitePoints, BlackPoints, Date) VALUES (@WhitePlayer, @BlackPlayer, @Time, @WhitePoints, @BlackPoints, @Date)", highscore);
            }
        }

        //DELETE
        public static void DeleteHighscores()
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Execute("DELETE FROM Highscores");
                connection.Execute("VACUUM");
            }
        }
    }
}
