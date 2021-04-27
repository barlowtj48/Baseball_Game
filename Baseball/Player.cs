using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baseball
{
    class Player
    {
        private readonly string name;
        private int strikes = 0, balls = 0, score = 0, outs = 0, inning_score = 0;
        private Field field = new Field();
        public bool playing = false;
        private int points = 0;

        public Player(string m_name)
        {
            name = m_name;
        }

        public int Get_Score()
        {
            return score;
        }

        public void Strike()
        {
            strikes++;
            if(strikes == 3)
            {
                Add_Outs(1);
            }
        }
        public void Foul_Ball()
        {
            if(strikes < 2)
            {
                strikes++;
            }
        }
        public void Ball()
        {
            balls++;
            if(balls == 4)
            {
                Hit(1); //walk
            }
        }
        public void Sacrifice()
        {
            Add_Outs(1);
            points = field.Sacrifice();
            inning_score += points;
            score += points;
        }
        public void Double_Play()
        {
            Add_Outs(field.Double_Play());
        }
        public void Home_Run()
        {
            balls = 0;
            strikes = 0;
            points = field.Home_Run();
            inning_score += points;
            score += points;
            
        }
        public void Grand_Slam()
        {
            balls = 0;
            strikes = 0;
            points = field.Grand_Slam();
            inning_score += points;
            score += points;
        }
        public void Out()
        {
            Add_Outs(1);
        }
        /// <summary>
        /// Bases are taken, with or without a penalty to the number of outs the player has
        /// </summary>
        /// <param name="n">number of bases advanced by the hitter</param>
        /// <param name="p">number of outs caused by the hit, either 0, 1, or 2.</param>
        public void Hit(int n)
        {
            balls = 0;
            strikes = 0;
            points = field.Hit(n);
            inning_score += points;
            score += points;
        }

        public void Steal()
        {
            points = field.Steal();
            inning_score += points;
            score += points;
        }

        private void Add_Outs(int o)
        {
            balls = 0;
            strikes = 0;
            outs += o;
            if (outs >= 3)
            {
                End_Player_Inning();
                return;
            }
        }

        private void End_Player_Inning()
        {
            playing = false;
            strikes = 0;
            balls = 0;
            outs = 0;
            inning_score = 0;
            field.Reset();
        }

        public override string ToString()
        {
            return name;
        }

        public Dictionary<string, int> Get_Stats()
        {
            Dictionary<string, int> stats = new Dictionary<string, int>();
            stats.Add("strikes", strikes);
            stats.Add("balls", balls);
            stats.Add("outs", outs);
            stats.Add("field", field.Get_Stats());
            stats.Add("score", score);
            stats.Add("inning_score", inning_score);
            return stats;
        }
    }
}
