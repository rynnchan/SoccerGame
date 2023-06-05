using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerGame
{
    internal class Player
    {
        public string Name { get;}
        public string Position { get;}
        public Parameter Param { get;}

        private static Random rnd = new Random();

        public Player(string name, string position, Parameter param)
        {
            Name = name;
            Position = position;
            Param = param; // Kick, Dribble, Technique, Block, Catch
        }

        public override string ToString()
        {
            return Name;
        }

        public double GetCatching() // キャッチ
        {
            double catchPer5 = Param.Catch * 0.2;
            return Param.Catch * 0.8 + Param.Block * 0.2 + (catchPer5 * 2 * rnd.NextDouble() - catchPer5);
        }
        public double GetPanching() // パンチング
        {
            double catchPer5 = Param.Catch * 0.2;
            return Param.Catch * 0.8 + Param.Technique * 0.2 + (catchPer5 * 2 * rnd.NextDouble() - catchPer5);
        }
        public double GetTackle() // タックル
        {
            double blockPer5 = Param.Block * 0.2;
            return Param.Block * 0.8 + Param.Technique * 0.2 + (blockPer5 * 2 * rnd.NextDouble() - blockPer5);
        }
        public double GetSliding() // スライディング
        {
            double blockPer5 = Param.Block * 0.2;
            return Param.Block * 0.8 + Param.Kick * 0.2 + (blockPer5 * 2 * rnd.NextDouble() - blockPer5);
        }
        public double GetToppa() // とっぱ
        {
            double dribblePer5 = Param.Dribble * 0.2;
            return Param.Dribble * 0.8 + Param.Technique * 0.2 + (dribblePer5 * 2 * rnd.NextDouble() - dribblePer5);
        }
        public double GetPass() // パス
        {
            double techniquePer5 = Param.Technique * 0.2;
            return Param.Technique * 0.8 + Param.Dribble * 0.2 + (techniquePer5 * 2 * rnd.NextDouble() - techniquePer5);
        }
        public double GetShot() // シュート
        {
            double kickPer5 = Param.Kick * 0.2;
            return Param.Kick * 0.8 + Param.Dribble * 0.2 + (kickPer5 * 2 * rnd.NextDouble() - kickPer5);
        }
        public double GetChipShot() // ループシュート
        {
            double kickPer5 = Param.Kick * 0.2;
            return Param.Kick * 0.8 + Param.Technique * 0.2 + (kickPer5 * 2 * rnd.NextDouble() - kickPer5);
        }
        public double GetHeadding() // ヘディング
        {
            double kickPer5 = Param.Kick * 0.2;
            return Param.Kick * 0.8 + Param.Block * 0.2 + (kickPer5 * 2 * rnd.NextDouble() - kickPer5);
        }
    }
}
