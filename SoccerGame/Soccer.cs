using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

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
            ShowTitle();
            Team myTeam = ChooseTeam(null, "あなた");
            Team enemyTeam = ChooseTeam(myTeam, "相手");
            int state = 2; // 0:セーブ, 1:守備, 2:攻撃, 3:シュート
            bool kickoff = true;

            //ShowScore(myTeam, enemyTeam);
            // Console.WriteLine($"あなたのチーム：{myTeam}, 相手のチーム：{enemyTeam}");
            Console.ReadKey();
            Console.WriteLine($"{winScore}点先取！");
            Console.WriteLine("【試合開始！】");
            Console.WriteLine("--------------------------------------------------------");
            Console.ReadKey();

            Player player = myTeam.GetMF();
            int superiority = 1; // 1:自チーム攻め, -1:敵チーム攻め

            while (true)
            {
                if (kickoff)
                {
                    ShowSquareText("KICK  OFF", ConsoleColor.Yellow);
                    Console.WriteLine();
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
                Thread.Sleep(1000);

                if (state == -1 || state == 4)
                {
                    //Console.WriteLine("【G O A L】");
                    ShowScore(myTeam, enemyTeam);
                    kickoff = true;

                    if (myTeam.Score == winScore)
                    {
                        ShowSquareText("あなたの勝利！", ConsoleColor.Cyan);
                        //Console.WriteLine("あなたの勝利！");
                        break;
                    }
                    else if (enemyTeam.Score == winScore)
                    {
                        ShowSquareText("あなたの負け^^", ConsoleColor.Magenta);
                        //Console.WriteLine("あなたの負け^^");
                        break;
                    }
                    superiority = state == -1 ? 1 : -1;
                    state = (state + 3) % 3;
                    Console.ReadKey();
                }

                Console.WriteLine("--------------------------------------------------------");
            }
        }

        public static void ShowTitle()
        {
            Console.Write(
                "         ---------------------------------------------------------\n"+
                "        |                たのしいさっかーげーむ                   |\n" +
                "        | 　　　   　∩＿∩　　　　　　　　　 ∩＿∩　　　　　    |\n" +
                "        |         と( ^(ｪ)^)                 (・(ｪ)・)            |\n" +
                "        |           / と_ﾉ                     と＿ノヽ           |\n" +
　　　 　       "        |         （＿⌒ヽ                       (⌒ヽ ＼         |\n"+
                "        |           ﾉノ `J        三 ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("○");
            Console.ResetColor();
            Console.WriteLine("　　　　　 `ー\"＼_）       |\n"+
                "        |                                                         |\n" +
                "         ---------------------------------------------------------");
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
                Console.Write($"{i+1} : {teams[i]}    ");
            }
            Console.WriteLine();
            while (true)
            {
                try
                {
                    Console.Write(">>> ");
                    int teamNo = int.Parse(Console.ReadLine())-1;
                    if (teamNo >= 0 && teamNo < teams.Count && teams[teamNo] != notTeam)
                    {
                        Console.ForegroundColor = notTeam == null ? ConsoleColor.Green : ConsoleColor.Red;
                        teams[teamNo].ShowTeam();
                        Console.ResetColor();
                        Console.WriteLine("--------------------------------------------------------");
                        return teams[teamNo];
                    }
                }
                catch(Exception ex)
                {
                    //Console.WriteLine(ex);
                }
                Console.WriteLine($"書いてある通りに数字を入力しようネ");
            }
        }

        private void ShowScore(Team teamA, Team teamB)
        {
            //Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write(
                " -------------------------------------------------------\n" +
                "|                                                       |\n|        ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("★      G      O      A      L      ★");
            Console.ResetColor();
            Console.Write("         |\n|                                                       |\n|          ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(teamA);
            Console.ResetColor();
            Console.Write($"  { teamA.Score} : { teamB.Score}  ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(teamB);
            Console.ResetColor();
            Console.Write("            |\n|                                                       |\n" +
                " -------------------------------------------------------\n");
            //Console.WriteLine("【G O A L】");
            //Console.WriteLine($"{teamA} {teamA.Score} : {teamB.Score} {teamB}");
        }

        private void ShowVS(string playerA, string playerB)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(playerA);
            Console.ResetColor();
            Console.Write(" v.s. ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(playerB);
            Console.ResetColor();
        }

        private void ShowSquareText(string str, ConsoleColor color)
        {
            Console.Write("         -----");
            Console.Write(new String('-', Encoding.GetEncoding("shift_jis").GetByteCount(str)));
            Console.WriteLine("-----");
            Console.Write("        |     ");
            Console.ForegroundColor = color;
            Console.Write(str);
            Console.ResetColor();
            Console.WriteLine("     |");
            Console.Write("         -----");
            Console.Write(new String('-', Encoding.GetEncoding("shift_jis").GetByteCount(str)));
            Console.WriteLine("-----");
        }

        private int Input(string[] candidates)
        {
            //Console.WriteLine($"選んでね");
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
            Console.WriteLine("せーぶでやんす！");
            Console.WriteLine();

            Player myGK = teamA.GetGK();

            int command = rnd.Next(attackCommand.Length);
            double enemyAction = Action(command, player, 3);

            ShowVS(myGK.ToString(), player.ToString());
            Console.WriteLine();

            int input = Input(defenceCommand);
            double myAction = Action(input, myGK, 0);
            Console.WriteLine();

            ShowVS($"{myGK}の{defenceCommand[input]}",$"{player}の{attackCommand[command]}");
            //Console.WriteLine($"{myGK}の{defenceCommand[input]} v.s. {player}の{attackCommand[command]}");
            Thread.Sleep(1000);
            ShowVS(Math.Round(myAction, 3).ToString(), Math.Round(enemyAction, 3).ToString());
            //Console.WriteLine($"{myAction} : {enemyAction}");
            Thread.Sleep(1000);

            if (myAction > enemyAction)
            {
                ShowSquareText("成功！", ConsoleColor.White);
                Console.WriteLine();
                Thread.Sleep(1000);
                Console.WriteLine($"{myGK}は{defenceCommand[input]}をした！");
                Console.WriteLine();
                return (1, teamA.GetDF());
            }
            else
            {
                ShowSquareText("失敗...", ConsoleColor.White);
                Console.WriteLine();
                Thread.Sleep(1000);
                Console.WriteLine($"{player}の{attackCommand[command]}が決まった！");
                Console.WriteLine();
                teamB.Score++;
                return (-1, teamA.GetMF());
            }
        }

        private (int, Player) MiddleDefence(Team teamA, Team teamB, Player player, int state)
        {
            Console.WriteLine("しゅびだピヨ！");
            Console.WriteLine();

            Player myPlayer = state == 1 ? teamA.GetDF() : teamA.GetMF();

            int command = rnd.Next(middleAttackCommand.Length);
            double enemyAction = Action(command, player, 2);

            ShowVS(myPlayer.ToString(), player.ToString());
            Console.WriteLine();

            int input = Input(middleDefenceCommand);
            double myAction = Action(input, myPlayer, 1);
            Console.WriteLine();

            ShowVS($"{myPlayer}の{middleDefenceCommand[input]}", $"{player}の{middleAttackCommand[command]}");
            //Console.WriteLine($"{myPlayer}の{middleDefenceCommand[input]} v.s. {player}の{middleAttackCommand[command]}");
            Thread.Sleep(1000);
            ShowVS(Math.Round(myAction, 3).ToString(), Math.Round(enemyAction, 3).ToString());
            //Console.WriteLine($"{myAction} : {enemyAction}");
            Thread.Sleep(1000);

            if (myAction > enemyAction)
            {
                ShowSquareText("成功！", ConsoleColor.White);
                Console.WriteLine();
                Thread.Sleep(1000);
                Console.WriteLine($"{myPlayer}の{middleDefenceCommand[input]}が決まる！");
                Console.WriteLine();
                return (1, myPlayer);
            }
            else
            {
                ShowSquareText("失敗...", ConsoleColor.White);
                Console.WriteLine();
                Thread.Sleep(1000);
                if (command == 1)
                {
                    Player tmpPlayer = player;
                    player = player.Position == "DF" ? teamB.GetMF() : teamB.GetFW();
                    Console.WriteLine($"{tmpPlayer}は{player}に{middleAttackCommand[command]}をした！");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"{player}は{middleAttackCommand[command]}で前に進む！");
                    Console.WriteLine();
                }
                return (-1, player);
            }
        }
        private (int, Player) MiddleAttack(Team teamA, Team teamB, Player player, int state)
        {
            Console.WriteLine("せめるピヨ！");
            Console.WriteLine();

            Player enemyPlayer = state == 2 ? teamB.GetDF() : teamB.GetMF();
            int command = rnd.Next(middleDefenceCommand.Length);
            double enemyAction = Action(command, enemyPlayer, 1);

            ShowVS(player.ToString(), enemyPlayer.ToString());
            Console.WriteLine();

            int input = Input(middleAttackCommand);
            double myAction = Action(input, player, 2);
            Console.WriteLine();

            ShowVS($"{player}の{middleAttackCommand[input]}", $"{enemyPlayer}の{middleDefenceCommand[command]}");
            //Console.WriteLine($"{player}の{middleAttackCommand[input]} v.s. {enemyPlayer}の{middleDefenceCommand[command]}");
            Thread.Sleep(1000);
            ShowVS(Math.Round(myAction, 3).ToString(), Math.Round(enemyAction, 3).ToString());
            //Console.WriteLine($"{myAction} : {enemyAction}");
            Thread.Sleep(1000);

            if (myAction > enemyAction)
            {
                ShowSquareText("成功！", ConsoleColor.White);
                Console.WriteLine();
                Thread.Sleep(1000);
                if (input == 1)
                {
                    Player tmpPlayer = player;
                    player = player.Position == "DF" ? teamA.GetMF() : teamA.GetFW();
                    Console.WriteLine($"{tmpPlayer}は{player}に{middleAttackCommand[input]}をした！");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"{player}は{middleAttackCommand[input]}で前に進む！");
                    Console.WriteLine();
                }
                return (1, player);
            }
            else
            {
                ShowSquareText("失敗...", ConsoleColor.White);
                Console.WriteLine();
                Thread.Sleep(1000);
                Console.WriteLine($"{enemyPlayer}の{middleDefenceCommand[command]}が決まる！");
                Console.WriteLine();
                return (-1, enemyPlayer);
            }
        }

        private (int, Player) Attack(Team teamA, Team teamB, Player player)
        {
            Console.WriteLine("しゅーとコケー！");
            Console.WriteLine();

            Player enemyGK = teamB.GetGK();

            int command = rnd.Next(defenceCommand.Length);
            double enemyAction = Action(command, enemyGK, 0);

            ShowVS(player.ToString(), enemyGK.ToString());
            Console.WriteLine();

            int input = Input(attackCommand);
            double myAction = Action(input, player, 3);
            Console.WriteLine();

            ShowVS($"{player}の{attackCommand[input]}", $"{enemyGK}の{defenceCommand[command]}");
            //Console.WriteLine($"{player}の{attackCommand[input]} v.s. {enemyGK}の{defenceCommand[command]}");
            Thread.Sleep(1000);
            ShowVS(Math.Round(myAction, 3).ToString(), Math.Round(enemyAction, 3).ToString());
            //Console.WriteLine($"{myAction} : {enemyAction}");
            Thread.Sleep(1000);

            if (myAction > enemyAction)
            {
                ShowSquareText("成功！", ConsoleColor.White);
                Console.WriteLine();
                Thread.Sleep(1000);
                Console.WriteLine($"{player}の{attackCommand[input]}が決まった！");
                Console.WriteLine();
                teamA.Score++;
                return (1, teamB.GetMF());
            }
            else
            {
                ShowSquareText("失敗...", ConsoleColor.White);
                Console.WriteLine();
                Thread.Sleep(1000);
                Console.WriteLine($"{enemyGK}は{defenceCommand[command]}をした！");
                Console.WriteLine();
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