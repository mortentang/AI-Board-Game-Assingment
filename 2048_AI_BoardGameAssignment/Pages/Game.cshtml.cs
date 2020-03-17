using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace _2048_AI_BoardGameAssignment.Pages
{
    public class GameModel : PageModel
    {
        [BindProperty]
        public string Direction { get; set; }

        [BindProperty]
        public int[,] Board { get; set; } = new int[4, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };

        private readonly ILogger<GameModel> _logger;

        public GameModel(ILogger<GameModel> logger)
        {
            _logger = logger;
        }

        public void MoveTiles(string direction)
        {
            System.Diagnostics.Debug.WriteLine(direction + " pressed");
            switch (direction)
            {
                case "left":
                    //for (int y = 0; y <= 3; y++)
                    //    for (int x = 1; x <= 3; x++)
                    //        for (int i = x; i > 0; i--)
                    //            if (board[x - i, y] == 0)
                    //            {
                    //                board[x - i, y] = board[x, y];
                    //                board[x, y] = 0;
                    //                break;
                    //            };
                    UpdateBoard();
                    break;
                case "right":
                    UpdateBoard();
                    break;
                case "up":
                    UpdateBoard();
                    break;
                case "down":
                    UpdateBoard();
                    break;
            }
        }
        public void OnGet()
        {
            UpdateBoard();
            UpdateBoard();
            StartGame();

        }

        public void StartGame()
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 3000;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            UpdateBoard();
            int rowCheck = 1;
            System.Diagnostics.Debug.WriteLine("Next board");
            foreach (int tile in Board)
            {
                System.Diagnostics.Debug.Write(tile + " ");
                if (rowCheck % 4 == 0)
                {
                    System.Diagnostics.Debug.WriteLine(" ");
                }
                rowCheck += 1;
            }
        }

        public struct Square
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Square(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        public void AddNumber()
        {
            var possibleSquares = new List<Square>();
            for (int x = 0; x <= 3; x++)
            {
                for (int y = 0; y <= 3; y++)
                    if (Board[x, y] == 0)
                    {
                        possibleSquares.Add(new Square(x, y));
                    };
            };
            var squaresToAdd = possibleSquares.Count;
            if (squaresToAdd > 0)
            {
                Random rnd = new Random();
                int squareToFill = rnd.Next(0, squaresToAdd);
                Board[possibleSquares[squareToFill].X, possibleSquares[squareToFill].Y] = rnd.NextDouble() < 0.9 ? 2 : 4;
            }
        }

        public void UpdateBoard()
        {
            AddNumber();
            ViewData["1"] = Board[0, 0];
            ViewData["2"] = Board[0, 1];
            ViewData["3"] = Board[0, 2];
            ViewData["4"] = Board[0, 3];
            ViewData["5"] = Board[1, 0];
            ViewData["6"] = Board[1, 1];
            ViewData["7"] = Board[1, 2];
            ViewData["8"] = Board[1, 3];
            ViewData["9"] = Board[2, 0];
            ViewData["10"] = Board[2, 1];
            ViewData["11"] = Board[2, 2];
            ViewData["12"] = Board[2, 3];
            ViewData["13"] = Board[3, 0];
            ViewData["14"] = Board[3, 1];
            ViewData["15"] = Board[3, 2];
            ViewData["16"] = Board[3, 3];
            ViewData["Board"] = Board;
        }
    }
}
