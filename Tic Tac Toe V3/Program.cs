using System;
using System.Management;
using System.Threading;

namespace Tic_Tac_Toe
{
    class Program
    {
        static Player p1 = new Player { Id = 1 };
        static Player p2 = new Player { Id = 2 };
        static Player Winner;
        static int Round = 0;
        static bool anothergame;
        static char[] Cell = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        static string[] Board = {$"     |     |     ",
                                 $"  1  |  2  |  3  ", // R2C3, C9, C15
                                 $"_____|_____|_____",
                                 $"     |     |     ",
                                 $"  4  |  5  |  6  ", // R5C3, C9, C15
                                 $"_____|_____|_____",
                                 $"     |     |     ",
                                 $"  7  |  8  |  9  ", // R8C3, C9, C15
                                 $"     |     |     "};

        //static int choice;
        static int winFlag = 0; //NOTE: 0 still going, 1 we have a winner, -1 draw 
        static bool Friend = false;
        static Random rnd = new Random();
        static int Left = Console.WindowWidth / 3;
        static int Top = 0;

        static void Main(string[] args)
        {
            BuildPlayers();

            do
            {
                Game();
                anothergame = AnotherGame();
            } while (anothergame);

            ShowScore();
            Console.ResetColor();
            Console.WriteLine("Press Any Key To Exit...");
            Console.ReadKey();
        }

        static void PrintBoard()
        {
            Console.Clear();
            Left = Console.WindowWidth / 3;
            Top = 0;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(Left, Top);
            Console.WriteLine("Welcome to Tic Tac Toe By Dor Zvulun");
            Console.ResetColor();
            Console.SetCursorPosition(Left, ++Top);
            Console.Write($"\n{p1.Soldier} = {p1.Name}\n{p2.Soldier} = {p2.Name}\n");
            Console.WriteLine($"Round {Round}\n\n");
            Console.ForegroundColor = ConsoleColor.Green;

            Board = new string[]{$"     |     |     ",
                                 $"  {Cell[1]}  |  {Cell[2]}  |  {Cell[3]}  ", // R2 C3, C9, C15
                                 $"_____|_____|_____",
                                 $"     |     |     ",
                                 $"  {Cell[4]}  |  {Cell[5]}  |  {Cell[6]}  ", // R5 C3, C9, C15
                                 $"_____|_____|_____",
                                 $"     |     |     ",
                                 $"  {Cell[7]}  |  {Cell[8]}  |  {Cell[9]}  ", // R8 C3, C9, C15
                                 $"     |     |     "};

            for (int i = 0; i < Board.Length; i++)
            {
                for (int j = 0; j < Board[i].Length; j++)
                {
                    if (Board[i][j] == 'X')
                        Console.ForegroundColor = ConsoleColor.Green;
                    if (Board[i][j] == 'O')
                        Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(Board[i][j]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
            }
            Console.ResetColor();

        }

        static void BuildPlayers()
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(Left, Top);
            Console.WriteLine("Welcome to Tic Tac Toe by Dor Zvulun");
            Console.ResetColor();

            Console.Write("\nPlayer 1 Please Enter Your Name: ");
            p1.Name = Console.ReadLine();

            do
            {
                Console.Write("\nSelect Your Symbol X or O :");
                p1.Soldier = (char)Console.ReadKey().Key; //NOTE: This way I always get the caps of input less validations (X or O).
                if (p1.Soldier != 'X' && p1.Soldier != 'O')
                    Console.WriteLine("\nSelect only X or O ");

            } while (p1.Soldier != 'X' && p1.Soldier != 'O');

            if (p1.Soldier == 'X')
                p2.Soldier = 'O';
            else p2.Soldier = 'X';

            ConsoleKeyInfo key;
            do
            {
                Console.Write($"\n\n{p1.Name} do you want to play against the Computer = 0 or your friend = 1? ");
                key = Console.ReadKey();
            } while (key.KeyChar != '0' && key.KeyChar != '1');

            if (key.KeyChar == '1')
                Friend = true;
            else Friend = false;

            if (!Friend)
            {
                Console.WriteLine($"\n\nAsking the Computer's Name...");
                ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                foreach (ManagementObject mo in mos.Get())
                    p2.Name += (mo["Name"]);
            }

            else
            {
                Console.Write("\n\nPlayer 2 Please Enter Your Name: ");
                p2.Name = Console.ReadLine();
            }
        }

        static void Game()
        {
            do
            {
                ++Round;
                PrintBoard();
                if (Round % 2 == 0)
                {
                    if (Friend)
                        UserChoice(p2);
                    else ComputerChoice();
                }
                else
                    UserChoice(p1);

                Winner = CheckWin();
               
            } while (winFlag == 0);

            PrintBoard();
            ShowWinner();
        }

        static void ComputerChoice()
        {
            bool ok = true;
            int ComputerChoice;
            do
            {
                ComputerChoice = rnd.Next(1, 10);
                if (Cell[ComputerChoice] == 'X' || Cell[ComputerChoice] == 'O')
                    ok = false;
                else ok = true;
            } while (!ok);

            Cell[ComputerChoice] = p2.Soldier;

            Console.Write("\nComputer is Thinking ");
            for (int i = 0; i < rnd.Next(1, 15); i++)
            {
                Console.Write("|");
                Thread.Sleep(50);
                Console.Write("\b/");
                Thread.Sleep(50);
                Console.Write("\b-");
                Thread.Sleep(50);
                Console.Write("\b\\");
                Thread.Sleep(50);
                Console.Write("\b|");
                Thread.Sleep(50);
                Console.Write("\b/");
                Thread.Sleep(50);
                Console.Write("\b-");
                Thread.Sleep(50);
                Console.Write("\b\\");
                Thread.Sleep(50);
                Console.Write("\b");
            }
        }

        static void UserChoice(Player p)
        {
            Console.Write($"\n{p.Name} Enter Your Selection between 1 and 9 [Enter to Submit]: (ex. 1) ");
            char temp;
            bool inputok = char.TryParse(Console.ReadLine(), out temp);
            int check = (int)temp;

            do
            {
                while (check < 49 || check > 57 || inputok == false)
                {
                    Console.Write("\nSelect a position between 1 and 9 only [Enter to Submit] ");
                    inputok = char.TryParse(Console.ReadLine(), out temp);
                    check = (int)temp;
                }

                if (Cell[check - 48] == 'X' || Cell[check - 48] == 'O')
                {
                    inputok = false;
                    Console.Write("\nSelection is already taken...");
                }

            } while (!inputok);

            Cell[check - 48] = p.Soldier;
        }

        static Player CheckWin()
        {
            int sum;

            //NOTE: Horizontal Win
            for (int i = 1; i <= 9; i += 3)
            {
                int x = Cell[i] * 3;
                sum = 0;
                for (int j = 0; j < 3; j++)
                {
                    sum += Cell[i + j];
                }
                if (sum == x)
                {
                    winFlag = 1;
                    break;
                }
            }

            //NOTE: Vertial Win
            for (int i = 1; i <= 3; i++)
            {
                int x = Cell[i] * 3;
                sum = 0;
                for (int j = 0; j < 9; j += 3)
                {
                    sum += Cell[i + j];
                }
                if (sum == x)
                {
                    winFlag = 1;
                    break;
                }
            }

            //NOTE: Diagonal win
            for (int i = 1; i <= 3; i += 2)
            {
                int x = Cell[i] * 3;
                sum = 0;
                if (i % 3 == 0)
                    for (int j = 0; j < 5; j += 2)
                        sum += Cell[i + j];
                else
                    for (int j = 0; j < 9; j += 4)
                        sum += Cell[i + j];
                if (sum == x)
                    winFlag = 1;
            }

            //NOTE: Who won?
            if (winFlag == 1)
            {
                if (Round % 2 == 0)
                {
                    p2.Score++;
                    return p2;
                }
                else
                {
                    p1.Score++;
                    return p1;
                }
            }

            if (Round > 9)
            {
                winFlag = -1;
                return null;
            }

            return null;
        }

        private static void ShowWinner()
        {
            if (winFlag == 1)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"The Winner of this match is {Winner.Name} ");
                Console.ResetColor();
            }
            else if (winFlag == -1)
                Console.WriteLine($"This match finished with a draw\n");
            ShowScore();
        }

        static void ShowScore()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n\nScore \n{p1.Name}: {p1.Score}\n{p2.Name}: {p2.Score}");
            Console.ResetColor();
            if (!anothergame && p1.Score+p2.Score>1)
            {
                int finalwinner = p2.Score - p1.Score;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"\nWinner Of The Game Is ");
                if (finalwinner > 0)
                {
                    Console.WriteLine($"{p2.Name}");
                }
                else if (finalwinner < 0)
                {

                    Console.WriteLine($"{p1.Name}");
                }
                else Console.WriteLine($"No One... The Game Ended With a Draw");
            }
        }

        static bool AnotherGame()
        {
            do
            {
                Console.Write($"\n\nDo you want to play another game? y/n ");
                string more = Console.ReadLine().ToLower();
                if (more != "y" && more != "n")
                    Console.Write($"\n\ny/n only! ");
                else if (more == "y")
                {
                    ClearBoard();
                    return true;
                }
                else return false;
            } while (true);
        }

        static void ClearBoard()
        {
            for (int i = 0; i < Cell.Length; i++)
                Cell[i] = (char)(i+48);

            Round = 0;
            winFlag = 0;
        }

    }
}
