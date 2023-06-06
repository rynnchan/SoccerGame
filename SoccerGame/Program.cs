using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Soccer soccer = new Soccer(1);
            soccer.Play();
        }
    }
}
