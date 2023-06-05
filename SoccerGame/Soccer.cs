using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace SoccerGame
{
    internal class Soccer
    {
        private int winScore;
        private static Random rnd = new Random();

        private static List<Team> teams = CreateTeams();
        private static string[] attackCommand = { "シュート", "ループシュート", "ヘディング" };
        private static string[] middleAttackCommand = { "とっぱ", "パス" };
        private static string[] middleDefenceCommand = { "タックル", "スライディング" };
        private static string[] defenceCommand = { "キャッチ", "パンチング" };

        public Soccer(int winScore)
        {
            this.winScore = winScore;
        }
        public void Play()
        {
            Team myTeam = ChooseTeam(null, "あなた");
            Team enemyTeam = ChooseTeam(myTeam, "相手");
            int state = 2; // 0:セーブ, 1:守備, 2:攻撃, 3:シュート
            bool kickoff = true;

            // Console.WriteLine($"あなたのチーム：{myTeam}, 相手のチーム：{enemyTeam}");

            Console.WriteLine($"{winScore}点先取！");
            Console.WriteLine("【試合開始！】");
            Console.WriteLine("--------------------------------------------");

            Player player = myTeam.GetMF();
            int superiority = 1; // 1:自チーム攻め, -1:敵チーム攻め

            while (true)
            {
                if (kickoff)
                {
                    Console.WriteLine("【KICK  OFF】");
                    kickoff = false;
                }

                switch (state)
                {
                    case 0: // セーブ
                        (superiority, player) = Defence(myTeam, enemyTeam, player);
                        break;
                    case 1: // 攻撃 or 守備
                    case 2:
                        (superiority, player) = superiority == 1 ? MiddleAttack(myTeam, enemyTeam, player, state) : MiddleDefence(myTeam, enemyTeam, player, state);
                        break;
                    case 3: // シュート
                        (superiority, player) = Attack(myTeam, enemyTeam, player);
                        break;
                }
                state += superiority;

                if(state == -1 || state == 4)
                {
                    Console.WriteLine("【G O A L】");
                    ShowScore(myTeam, enemyTeam);
                    kickoff = true;

                    if (myTeam.Score == winScore)
                    {
                        Console.WriteLine("あなたの勝利！");
                        break;
                    }
                    else if (enemyTeam.Score == winScore)
                    {
                        Console.WriteLine("あなたの負け^^");
                        break;
                    }
                    superiority = state == -1 ? 1 : -1;
                    state = (state + 3) % 3;
                }

                Console.WriteLine("--------------------------------------------");
            }
        }

        private static List<Team> CreateTeams()
        {
            string teamPath = @"C:\Users\ia\source\repos\SoccerGame\SoccerGame\team\";
            string[] files = Directory.GetFiles(teamPath, "*", SearchOption.AllDirectories);
            List<Team> teams = new List<Team>();
            foreach(string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                List<Player> players = new List<Player>();
                StreamReader sr = new StreamReader(file);
                while (sr.EndOfStream == false)
                {
                    string[] line = sr.ReadLine().Split(',');
                    players.Add(new Player(line[0], line[1], 
                        new Parameter(int.Parse(line[2]), int.Parse(line[3]), int.Parse(line[4]), int.Parse(line[5]), int.Parse(line[6]))));
                }
                sr.Close();
                teams.Add(new Team(fileName, players));
            }
            return teams;
        }

        private Team ChooseTeam(Team notTeam, string me)
        {
            Console.WriteLine($"{me}のチームを選んでね");
            for (int i = 0; i < teams.Count; i++)
            {
                if (teams[i] == notTeam) continue;
                Console.WriteLine($"{i} : {teams[i]}");
            }
            while (true)
            {
                try
                {
                    Console.Write(">> ");
                    int teamNo = int.Parse(Console.ReadLine());
                    if (teamNo >= 0 && teamNo < teams.Count && teams[teamNo] != notTeam)
                    {
                        teams[teamNo].ShowTeam();
                        Console.WriteLine("--------------------------------------------");
                        return teams[teamNo];
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
                Console.WriteLine($"書いてある通りに数字を入力しようネ");
            }
        }

        private void ShowScore(Team teamA, Team teamB)
        {
            Console.WriteLine($"【{teamA}】 {teamA.Score} : {teamB.Score} 【{teamB}】");
        }

        private int Input(string[] candidates)
        {
            Console.WriteLine($"選んでね");
            for (int i = 0; i < candidates.Length; i++)
            {
                Console.Write($"{i}:{candidates[i]} ");
            }
            Console.WriteLine();
            while (true)
            {
                try
                {
                    Console.Write(">>> ");
                    int input = int.Parse(Console.ReadLine());
                    if (input >= 0 && input < candidates.Length)
                    {
                        return input;
                    }
                }
                catch (Exception)
                {
                }
                Console.WriteLine($"書いてある通りに数字を入力するコケ");
            }
        }

        private (int, Player) Defence(Team teamA, Team teamB, Player player)
        {
            Console.WriteLine("せーぶでやんす");
            
            Player myGK = teamA.GetGK();

            int command = rnd.Next(attackCommand.Length);
            double enemyAction = Action(command, player, 3);

            Console.WriteLine($"{myGK} v.s. {player}");

            int input = Input(defenceCommand);
            double myAction = Action(input, myGK, 0);

            Console.WriteLine($"{myGK}の{defenceCommand[input]} v.s. {player}の{attackCommand[command]}");
            Console.WriteLine($"{myAction} : {enemyAction}");
            
            if (myAction > enemyAction)
            {
                return (1, teamA.GetDF());
            }
            else
            {
                teamB.Score++;
                return (-1, teamA.GetMF());
            }
        }

        private (int, Player) MiddleDefence(Team teamA, Team teamB, Player player, int state)
        {
            Console.WriteLine("しゅびだピヨ");

            Player myPlayer = state == 1 ? teamA.GetDF() : teamA.GetMF();

            int command = rnd.Next(middleAttackCommand.Length);
            double enemyAction = Action(command, player, 2);

            Console.WriteLine($"{myPlayer} v.s. {player}");

            int input = Input(middleDefenceCommand);
            double myAction = Action(input, myPlayer, 1);

            Console.WriteLine($"{myPlayer}の{middleDefenceCommand[input]} v.s. {player}の{middleAttackCommand[command]}");
            Console.WriteLine($"{myAction} : {enemyAction}");

            if (myAction > enemyAction)
            {
                return (1, myPlayer);
            }
            else
            {
                if (command == 1)
                {
                    player = player.Position == "DF" ? teamB.GetMF() : teamB.GetFW();
                }
                return (-1, player);
            }
        }
        private (int, Player) MiddleAttack(Team teamA, Team teamB, Player player, int state)
        {
            Console.WriteLine("せめるピヨ");

            Player enemyPlayer = state == 2 ? teamB.GetDF() : teamB.GetMF();
            int command = rnd.Next(middleDefenceCommand.Length);
            double enemyAction = Action(command, enemyPlayer, 1);

            Console.WriteLine($"{player} v.s. {enemyPlayer}");

            int input = Input(middleAttackCommand);
            double myAction = Action(input, player, 2);

            Console.WriteLine($"{player}の{middleAttackCommand[input]} v.s. {enemyPlayer}の{middleDefenceCommand[command]}");
            Console.WriteLine($"{myAction} : {enemyAction}");

            if (myAction > enemyAction)
            {
                if(input == 1)
                {
                    player = player.Position == "DF" ? teamA.GetMF() : teamA.GetFW();
                }
                return (1, player);
            }
            else
            {
                return (-1, enemyPlayer);
            }
        }

        private (int, Player) Attack(Team teamA, Team teamB, Player player)
        {
            Console.WriteLine("しゅーとコケー");

            Player enemyGK = teamB.GetGK();

            int command = rnd.Next(defenceCommand.Length);
            double enemyAction = Action(command, enemyGK, 0);

            Console.WriteLine($"{player} v.s. {enemyGK}");

            int input = Input(attackCommand);
            double myAction = Action(input, player, 3);

            Console.WriteLine($"{player}の{attackCommand[input]} v.s. {enemyGK}の{defenceCommand[command]}");
            Console.WriteLine($"{myAction} : {enemyAction}");

            if (myAction > enemyAction)
            {
                teamA.Score++;
                return (1, teamB.GetMF());
            }
            else
            {
                return (-1, teamB.GetDF());
            }
        }

        private double Action(int input, Player player, int state) // 0:セーブ, 1:守備, 2:攻撃, 3:シュート
        {
            switch(state)
            {
                case 0:
                    switch (input)
                    {
                        case 0:
                            return player.GetCatching();
                        case 1:
                            return player.GetPanching();
                    }
                    break;
                case 1:
                    switch (input)
                    {
                        case 0:
                            return player.GetTackle();
                        case 1:
                            return player.GetSliding();
                    }
                    break;
                case 2:
                    switch (input)
                    {
                        case 0:
                            return player.GetToppa();
                        case 1:
                            return player.GetPass();
                    }
                    break;
                case 3:
                    switch (input)
                    {
                        case 0:
                            return player.GetShot();
                        case 1:
                            return  player.GetChipShot();
                        case 2:
                            return player.GetHeadding();

                    }
                    break;
            }
            return 0;

        }
    }
}