using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baseball
{
    class Field
    {
        private List<int> hitters = new List<int>();
        
        public Field()
        {
            Reset();
        }

        public void Reset()
        {
            hitters.Clear();
        }

        public int Hit(int n) //single, double, triple
        {
            int points = 0;
            hitters.Add(0);
            for (int i = 0; i < hitters.Count; i++)
            {
                hitters[i] += n;
                if(hitters[i] > 3)
                {
                    points++;
                    hitters.Remove(hitters[i]);
                    i--;
                }
            }
            return points;
        }

        public int Steal()
        {
            List<int> empty_bases = new List<int> { 2, 3, 4 };
            for (int i = 0; i < hitters.Count; i++)
            {
                empty_bases.Remove(hitters[i]);
            }
            for (int i = 0; i < empty_bases.Count; i++)
            {
                if (hitters.Contains(empty_bases.First()-1))
                {
                    int index = hitters.IndexOf(empty_bases.First() - 1);
                    hitters[index] = empty_bases.First();
                    if(hitters[index] > 3)
                    {
                        hitters.Remove(hitters[index]);
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    empty_bases.Remove(empty_bases.First());
                    i--;
                }
            }
            return 0;
        }

        public int Sacrifice()
        {
            int points = Hit(1);
            hitters.Remove(1);
            return points;
        }

        public int Double_Play()
        { 
            if(hitters.Count == 1)
            {
                Reset();
                return 2;
            }
            else if(hitters.Count > 1)
            {
                Reset();
                hitters.Add(1);
                return 2;
            }
            return 1;
        }

        public int Home_Run()
        {
            return Hit(4);
        }

        public int Grand_Slam()
        {
            Reset();
            return 4;
        }

        public int Get_Stats()
        {
            //To preserve the 100's place, 1000 is added so that 011 does not become 11. 
            int output = 1000;
            hitters.Sort();
            for (int i = 1; i < 4; i++)
            {
                if (hitters.Contains(i))
                {
                    output += (int)Math.Pow(10, (3 - i));
                }
            }
            return Convert.ToInt16(output);
        }
    }
}
