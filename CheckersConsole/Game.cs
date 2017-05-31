using System;

namespace Checkers
{
    public enum Color { Undefined, White, Black };

    public class Game
    {
        public int GameID { get; set; }
        public enum State { Game, Gameover, Nogame };

        public State GameState { get; set;}
        public CheckerBoard Board { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; } 
        public Player CurrentPlayer { get; set; }
        protected Player winner;
        public DateTime StartTime { get; set; }

        public Game()
        {
            InitGame();
        }

        public Player Winner
        {
            get { return winner; }
            set
            {
                winner = value;
            }
        }

        public void InitGame()
        {
            Random gen = new Random(DateTime.Now.Millisecond);

            Color col1 = gen.Next(1, 100) % 2 == 0 ? Color.Black : Color.White;
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
            Player2 = new Player(col1 == Color.Black ? Color.White : Color.Black, Board);

            CurrentPlayer = col1 == Color.White ? Player1 : Player2;
            GameState = State.Nogame;
       

            // kolor dla gracza wylosowac
            // ustawic graczy, plansze stworzyc

        }

        public void Run()
        {
            while (GameState == State.Game)
            {
                if (IsGameOver())
                {
                    GameState = State.Gameover;
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

        public void SetGameState()
        {
            if(Player1.Name == null || Player2.Name == null)
                GameState = State.Nogame;
            else if (IsGameOver())
                GameState = State.Gameover;
            else
                GameState = State.Game;
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