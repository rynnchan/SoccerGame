using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerGame
{
    internal class Parameter
    {
        public int Kick { get; }
        public int Dribble { get; }
        public int Technique { get; }
        public int Block { get; }
        public int Catch { get; }

        public Parameter(int kick, int dribble, int technique, int block, int catchP)
        {
            Kick = kick;
            Dribble = dribble;
            Technique = technique;
            Block = block;
            Catch = catchP;
        }

        public override string ToString()
        {
            return $"キック:{Kick}, ドリブル:{Dribble}, テクニック:{Technique}, ブロック:{Block}, キャッチ:{Catch}";
        }
    }
}
