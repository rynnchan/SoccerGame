using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerGame
{
    internal class Team
    {
        private static Random rnd = new Random();
        public string Name { get;}
        public List<Player> Players { get;}
        public int Score { get; set; } = 0;
        
        public Team(string name, List<Player> players)
        {
            Name = name;
            Players = players;
        }

        public override string ToString()
        {
            return Name;
        }

        public void ShowTeam()
        {
            Console.WriteLine(Name);
            var position_num = Players.GroupBy(x => x.Position).Select(x =>  x.Count()).ToList();
            int max_position_num = position_num.Max();

            Console.Write("     " + new String(' ', (max_position_num - position_num[3]) * 4));
            foreach (Player p in GetFWs())
            {
                Console.Write($"{p.ToString()} ");
            }
            Console.WriteLine();
            Console.Write("     " + new String(' ', (max_position_num - position_num[2]) * 4));
            foreach (Player p in GetMFs())
            {
                Console.Write($"{p.ToString()} ");
            }
            Console.WriteLine();
            Console.Write("     " + new String(' ', (max_position_num - position_num[1]) * 4));
            foreach (Player p in GetDFs())
            {
                Console.Write($"{p.ToString()} ");
            }
            Console.WriteLine();
            Console.Write("     " + new String(' ', (max_position_num - position_num[0]) * 4));
            foreach (Player p in GetGKs())
            {
                Console.Write(p.ToString());
            }
            Console.WriteLine();
        }

        public List<Player> GetGKs()
        {
            return Players.FindAll(p => p.Position == "GK");
        }
        public List<Player> GetDFs()
        {
            return Players.FindAll(p => p.Position == "DF");
        }
        public List<Player> GetMFs()
        {
            return Players.FindAll(p => p.Position == "MF");
        }
        public List<Player> GetFWs()
        {
            return Players.FindAll(p => p.Position == "FW");
        }

        public Player GetGK()
        {
            List<Player> GKs = GetGKs();
            return GKs[rnd.Next(GKs.Count)];
        }
        public Player GetDF()
        {
            List<Player> DFs = GetDFs();
            return DFs[rnd.Next(DFs.Count)];
        }
        public Player GetMF()
        {
            List<Player> MFs = GetMFs();
            return MFs[rnd.Next(MFs.Count)];
        }
        public Player GetFW()
        {
            List<Player> FWs = GetFWs();
            return FWs[rnd.Next(FWs.Count)];
        }

    }
}
