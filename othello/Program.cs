using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace othello
{
    class Program
    {
        // initialize board
        static string[,] board = new string[8, 8] 
            { { "", "r", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "" }, { "", "", "", "b", "r", "", "", "" }, 
                { "", "", "", "r", "b", "", "", "" }, { "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "" } };

        // initialize important variables
        static string turn = "r";

        static int red = 2;
        static int blue = 2;

        static Random random = new Random();

        static bool checkDir(string colour, int x, int y, int a, int b)
        {
            // opposite colour
            string o;
            if (colour == "r") o = "b";
            else o = "r";

            // try/catch so that the check doesn't run off the map
            try
            {
                // if the board already has a tile on it don't run the other checks just return false
                if (board[y, x] == "r" || board[y, x] == "b") return false;

                for (int i = 0; i < 8; i++)
                {
                    if (i == 0) continue;
                    else if (i == 1) // if the tile is the opposite colour then keep going
                    {
                        if (board[y + (b * i), x + (a * i)] == o) continue;
                        else return false; // otherwise it is your own colour and since it's right next to the last tile it's not a valid move
                    }
                    else
                    {
                        if (board[y + (b * i), x + (a * i)] == colour) return true; // if any tiles after that are your own colour then it's a valid move return true
                        else if (board[y + (b * i), x + (a * i)] == "" || board[y + (b * i), x + (a * i)] == "p") return false; // if there are none of your own colours then return FALSE
                    }
                }
            } catch { return false; }
            return false;
        }

        static void possibleMoves(string colour)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] == "")
                    {
                        // check every direction on every tile and if any one is true then the tile is possible
                        if (checkDir(colour, j, i, 1, 0) || checkDir(colour, j, i, 1, 1) || checkDir(colour, j, i, 1, -1) || checkDir(colour, j, i, 0, 1) || 
                            checkDir(colour, j, i, 0, -1) || checkDir(colour, j, i, -1, 1) || checkDir(colour, j, i, -1, -1) || checkDir(colour, j, i, -1, 0)) 
                        {
                            board[i, j] = "p";
                        }
                    }
                }
            }
        }

        static void flip(string colour, int x, int y, int a, int b)
        {
            // opposite colour
            string o;
            if (colour == "r") o = "b";
            else o = "r";
            
            for (int i = 1; i < 8; i++)
            {
                // check in given direction
                // check if the tile is the opposite colour, otherwise stop flipping
                if (board[y + (b * i), x + (a * i)] == o)
                {
                    // flip the tile
                    board[y + (b * i), x + (a * i)] = colour;
                    // change the score
                    if (colour == "r")
                    {
                        red++;
                        blue--;
                    }
                    else
                    {
                        blue++;
                        red--;
                    }
                }
                else break;
            }
        }

        static void placeTile(string colour, int x, int y)
        {
            // check if tiles can be flipped in every direction and then flip them
            if (checkDir(colour, x, y, 0, 1)) flip(colour, x, y, 0, 1);
            if (checkDir(colour, x, y, 0, -1)) flip(colour, x, y, 0, -1);
            if (checkDir(colour, x, y, 1, 1)) flip(colour, x, y, 1, 1);
            if (checkDir(colour, x, y, 1, 0)) flip(colour, x, y, 1, 0);
            if (checkDir(colour, x, y, 1, -1)) flip(colour, x, y, 1, -1);
            if (checkDir(colour, x, y, -1, 1)) flip(colour, x, y, -1, 1);
            if (checkDir(colour, x, y, -1, 0)) flip(colour, x, y, -1, 0);
            if (checkDir(colour, x, y, -1, -1)) flip(colour, x, y, -1, -1);
            // change the tile at position (X, Y) to the given colour
            board[y, x] = colour;
            // increase player's tile count score
            if (colour == "r") red++;
            else blue++;
        }

        static void clearPossible()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] == "p")
                        board[i, j] = "";
                }
            }
        }

        static int countPossible()
        {
            int c = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] == "p")
                        c++;
                }
            }
            return c;
        }

        static int countDir(int x, int y, int a, int b)
        {
            int c = 0;
            for (int i = 1; i < 8; i++)
            {
                if (board[y + (b * i), x + (a * i)] == "r")
                {
                    c++;
                }
                else break;
            }
            return c;
        }

        static int countTile(int x, int y)
        {
            int c = 1;

            // count tiles that would flip in all 8 directions if tiles would flip
            if (checkDir("b", x, y, 0, 1)) c = c + countDir(x, y, 0, 1);
            if (checkDir("b", x, y, 0, -1)) c = c + countDir(x, y, 0, -1);
            if (checkDir("b", x, y, 1, 1)) c = c + countDir(x, y, 1, 1);
            if (checkDir("b", x, y, 1, 0)) c = c + countDir(x, y, 1, 0);
            if (checkDir("b", x, y, 1, -1)) c = c + countDir(x, y, 1, -1);
            if (checkDir("b", x, y, -1, 1)) c = c + countDir(x, y, -1, 1);
            if (checkDir("b", x, y, -1, 0)) c = c + countDir(x, y, -1, 0);
            if (checkDir("b", x, y, -1, -1)) c = c + countDir(x, y, -1, -1);

            // additional score
            // corners
            if (x == 0 && y == 0) c = c + 100;
            else if (x == 7 && y == 0) c = c + 100;
            else if (x == 0 && y == 7) c = c + 100;
            else if (x == 7 && y == 7) c = c + 100;
            // edges
            else
            {
                if (x == 0 || x == 7) c = c + 10;
                else if (y == 0 || y == 7) c = c + 10;
            }
            return c;
        }
        
        static void displayMap()
        {
            // top row
            Console.Write("  ");
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("1 2 3 4 5 6 7 8 \n");

            for (int i = 0; i < 8; i++)
            {
                // first collumn
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{i + 1} ");

                // rows
                for (int j = 0; j < 8; j++)
                {
                    string piece = board[i, j];
                    if (piece == "")
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("X ");
                    }
                    else if (piece == "b")
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.Write("B ");
                    }
                    else if (piece == "r")
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("R ");
                    }
                    else if (piece == "p")
                    {
                        if (turn == "r")
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("O ");
                        }
                        if (turn == "b")
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("O ");
                        }
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("? ");
                    }
                }
                
                // game information
                Console.ResetColor();
                if (i == 0) // red points
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"  Red: {red}");
                }
                else if (i == 1) // blue points
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"  Blue: {blue}");
                }
                else if (i == 3) // whose move is it
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    if (turn == "b")
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write($"  Blue's Move");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"  Red's Move");
                    }
                }
                Console.Write("\n");
            }
        }

        static void clearMap()
        {
            // loop through and delete everything
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j] = "";
                }
            }
            // starting tiles
            board[3, 3] = "b";
            board[3, 4] = "r";
            board[4, 3] = "r";
            board[4, 4] = "b";
        }

        static string bestMove()
        {
            Dictionary<string, int> possible = 
                new Dictionary<string, int>();
            List<string> choices =
                new List<string>();

            int x = 0;
            int y = 0;
            
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] == "p")
                        possible.Add($"{j} {i}", countTile(j, i));
                }
            }
            Console.WriteLine(String.Join(",", possible.Keys));
            Console.WriteLine(String.Join(",", possible.Values));
            int max = possible.Values.Max();
            foreach (string i in possible.Keys)
                if (possible[i] == max) choices.Add(i);
            if (choices.Count() == 1) return choices[0];
            else return choices[random.Next(choices.Count())];
        }
        
        static void Main(string[] args)
        {
        Start:
            // initialize variables
            string input = "";
            bool computer = false;
            int w = 0;
            int x = 0;
            int y = 0;
            red = 2;
            blue = 2;

            Console.WriteLine("Welcome to Othello!");
            Console.WriteLine("How would you like to play?\n1 - With another human\n2 - With the computer");
            while (true)
            {
                try
                {
                    input = Console.ReadLine();
                    if (!"12".Contains(input) || input.Length != 1) throw new Exception();
                    if (input == "2") computer = true;
                    else computer = false;
                    break;
                }
                catch
                {
                    Console.WriteLine($"There was something wrong with your input! Please try again.");
                }
            }
            Console.Clear();

            //turns
            while (w != 2)
            {
                // opposite colour
                string o;
                if (turn == "r") o = "b";
                else o = "r";

                possibleMoves(turn);
                displayMap();
                // end game early if a player gets wiped off the board
                if (red == 0 || blue == 0)
                {
                    Console.WriteLine("No possible moves!");
                    break;
                }
                // end game early once neither player has any moves
                if (countPossible() == 0)
                {
                    Console.WriteLine("No possible moves!");
                    turn = o;
                    w++;
                    continue;
                }

                while (true)
                {
                    // ai turn
                    if (turn == "b" && computer == true)
                    {
                        string best = bestMove();
                        x = Convert.ToInt32(best[0].ToString());
                        y = Convert.ToInt32(best[2].ToString());
                        Console.WriteLine($"{x + 1} {y + 1}");
                        placeTile(turn, x, y);
                        clearPossible();
                        turn = o;
                        w = 0;
                        break;
                    }
                    // human turns
                    Console.Write("Where would you like to go? ");
                    input = Console.ReadLine().Trim();
                    try
                    {
                        // input
                        if (input[1] != ' ') throw new Exception();
                        if (input.Length > 3) throw new Exception();

                        x = Convert.ToInt32(input[0].ToString()) - 1;
                        y = Convert.ToInt32(input[2].ToString()) - 1;

                        if (x > 8 || x < 0) throw new Exception();
                        if (y > 8 || y < 0) throw new Exception();
                        if (board[y, x] != "p")
                        {
                            Console.WriteLine("That isn't a valid place to put a tile! Please try again.");
                            continue;
                        }
                        placeTile(turn, x, y);
                        clearPossible();
                        turn = o;
                        w = 0;
                        //Console.Clear();
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"There was something wrong with your input! Please try again."); //({e.GetType()})
                    }
                }
            }
            // win
            if (red > blue)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nRed wins!");
                Console.ResetColor();
                Console.WriteLine($"Red's score: {red}\nBlue's score: {blue}");
            }
            if (blue > red)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\nBlue wins!");
                Console.ResetColor();
                Console.WriteLine($"Red's score: {red}\nBlue's score: {blue}");
            }
            if (red == blue)
            {
                Console.WriteLine("\nIt's a tie!");
                Console.ResetColor();
                Console.WriteLine($"Red's score: {red}\nBlue's score: {blue}");
            }
            // restart
            while (true)
            {
                Console.WriteLine("Would you like to play again? ");
                input = Console.ReadLine().ToLower();
                if (input == "yes" || input == "y")
                {
                    clearMap();
                    Console.Clear();
                    turn = "r";
                    goto Start;
                }
                else break;
            }
        }
    }
}