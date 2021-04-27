using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baseball
{
    class Baseball_Game
    {
        private int inning = 0;
        private Player[] players;
        private Player current_player;
        private int[,] scoreboard;

        public Baseball_Game(string[] player_names)
        {
            players = new Player[player_names.Length];
            for (int i = 0; i < player_names.Length; i++)
            {
                players[i] = new Player(player_names[i]);
            }
            scoreboard = new int[player_names.Length, 9];
            current_player = players[0];
            current_player.playing = true;
        }

        public string Get_Winner()
        {
            string winner = "No winner yet.";
            if(inning == 9) //zero index, so this means 10th inning. No current solution for ongoing tie
            {
                int highest_score = int.MinValue;
                foreach(var player in players)
                {
                    if (player.Get_Score() > highest_score)
                    {
                        highest_score = player.Get_Score();
                        winner = player.ToString();
                    }
                }
            }
            return winner;
        }

        public Dictionary<string, int> Do(Actions action)
        {
            switch (action)
            {
                case (Actions.Single):
                    current_player.Hit(1);
                    break;
                case (Actions.Double):
                    current_player.Hit(2);
                    break;
                case (Actions.Triple):
                    current_player.Hit(3);
                    break;
                case (Actions.Strike):
                    current_player.Strike();
                    break;
                case (Actions.Foul):
                    current_player.Foul_Ball();
                    break;
                case (Actions.Ball):
                    current_player.Ball();
                    break;
                case (Actions.Stolen_Base):
                    current_player.Steal();
                    break;
                case (Actions.Sacrifice):
                    current_player.Sacrifice();
                    break;
                case (Actions.Double_Play):
                    current_player.Double_Play();
                    break;
                case (Actions.Out):
                    current_player.Out();
                    break;
                case (Actions.Home_Run):
                    current_player.Home_Run();
                    break;
                case (Actions.Grand_Slam):
                    current_player.Grand_Slam();
                    break;
            }
            if (!current_player.playing)
            {
                int next_index = players.ToList().IndexOf(current_player) + 1;
                if (next_index >= players.Length)
                {
                    inning++;
                    next_index = 0;
                }
                current_player = players[next_index];
                current_player.playing = true;
            }
            var stats = current_player.Get_Stats();
            stats.Add("inning", inning);
            stats.Add("current_player", players.ToList().IndexOf(current_player));
            return stats;
        }
    }
}
