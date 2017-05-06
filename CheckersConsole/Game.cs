using System;

namespace Checkers
{
    public enum Color { UNDEFINED, WHITE, BLACK };

    public class Game
    {
        protected enum State { GAME, GAMEOVER, NOGAME };

        protected State gameState;
        public CheckerBoard Board { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; } 
        public Player CurrentPlayer { get; set; }
        protected Player winner;

        public Game()
        {
            InitGame();
        }

        public Player Winner
        {
            set
            {
                winner = value;
            }
        }

        public void InitGame()
        {
            Random gen = new Random(DateTime.Now.Millisecond);

            Color col1 = gen.Next(1, 100) % 2 == 0 ? Color.BLACK : Color.WHITE;
            int[,] array = { {0, 0, 0, 0, 0, 0, 0, 0},
                             {0, 0, 0, 1, 0, 0, 0, 0},
                             {0, 0, 0, 0, 0, 0, 0, 0},
                             {0, 0, 0, 0, 0, 0, 0, 0},
                             {0, 0, 0, 0, 0, 1, 0, 0},
                             {0, 0, 0, 0, 1, 0, 1, 0},
                             {0, 0, 0, 1, 0, 1, 0, 2},
                             {0, 0, 0, 0, 0, 0, 0, 0},

            
                                };
            Board = new CheckerBoard();

            Player1 = new Player(col1, Board);
            Player2 = new Player(col1 == Color.BLACK ? Color.WHITE : Color.BLACK, Board);

            CurrentPlayer = col1 == Color.WHITE ? Player1 : Player2;
       

            // kolor dla gracza wylosowac
            // ustawic graczy, plansze stworzyc

        }

        public void Run()
        {
            while (gameState == State.GAME)
            {
                if (IsGameOver())
                {
                    gameState = State.GAMEOVER;
                    break;
                }
                Player enemy = CurrentPlayer == Player1 ? Player2 : Player1;
                //CurrentPlayer.Turn(Board, enemy);
                if (CurrentPlayer == Player1)
                    CurrentPlayer = Player2;
                else
                    CurrentPlayer = Player1;
            }

            AnnounceWinning();
        }

        private bool IsGameOver()
        {
           if (Player1.NumberOfMovementsWithoutAttack == Config.Cfg.maxNumberOfMovements && Player2.NumberOfMovementsWithoutAttack == Config.Cfg.maxNumberOfMovements)
                return true;
            if (Player1.NumberOfPieces == 0 || Player1.IsPlayerBlocked(Board))
            {
                Winner = Player2;
                return true;
            }
            if(Player2.NumberOfPieces == 0 || Player2.IsPlayerBlocked(Board))
            {
                Winner = Player1;
                return true;
            }
               
            return false;
        }

        private void AnnounceWinning()
        {
            Console.WriteLine("!!! GAME OVER !!!");
            if (winner == null)
                Console.WriteLine("DRAW!");
            else
            Console.WriteLine("Player {0} won the game !", winner.GetColor);
        }
    }

    class Program
    {
        public static void Main()
        {
            // mozna sobie cos posprawdzac
            //Console.WriteLine("Checkers");
            //int[,] array = { {2, 0, 2, 0, 2, 0, 2, 0},
            //                 {0, 2, 0, 2, 0, 2, 0, 2},
            //                 {2, 0, 2, 0, 2, 0, 2, 0},
            //                 {0, 0, 0, 0, 0, 0, 0, 0},
            //                 {0, 0, 0, 0, 0, 0, 0, 0},
            //                 {1, 0, 1, 0, 1, 0, 1, 0},
            //                 {0, 1, 0, 1, 0, 1, 0, 1},
            //                 {1, 0, 1, 0, 1, 0, 1, 0},


            //                    };
            //var myBoard = new CheckerBoard();
            //var myPlayer = new Player(Color.BLACK, myBoard);
            //if (myBoard[2, 2] == null)
            //    Console.WriteLine("NULL REFERENCE");
            //else Console.WriteLine("IsCorrectPiece: " + myPlayer.IsCorrectPiece(myBoard[2, 2]));
            //myBoard.DrawBoard(myPlayer);

            //Console.WriteLine("CanAttack: " + myBoard[4, 2].CanAttack(myBoard));
            //Position dest;
            //Piece myPiece;
            //myPlayer.Input(out myPiece, out dest, myBoard);
            //myPiece.Move(myBoard, dest);
            //myBoard.DrawBoard(myPlayer);
            //Console.ReadKey();
            //Game g = new Game();
            //g.Run();

        }
    }
}