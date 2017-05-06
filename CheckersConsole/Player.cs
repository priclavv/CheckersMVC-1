using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Player
    {
        protected Color color;
        private int numberOfMovementsWithoutAttack = 0;
        protected List<Piece> pieces;
        private Piece attackPiece;
        public string Name { get; set; }
        
        // Konstruktor tworzy gracza o podanym kolorze i liscie pionkow z planszy
        public Player(Color c, CheckerBoard board)
        {
            color = c;
            numberOfMovementsWithoutAttack = 0;
            pieces = new List<Piece>();

            for (int y = Config.Cfg.board_size - 1; y >= 0; y--)
            {
                for (int x = 0; x < Config.Cfg.board_size; x++)
                {
                    if (board[x, y] != null && board[x, y].pieceColor == color)
                        pieces.Add(board[x, y]);
                }
            }
        }
        // zwraca liczbe pionkow
        public int NumberOfPieces
        {
            get
            {
                return pieces.Count;
            }
        }

        public int NumberOfMovementsWithoutAttack
        {
            get
            {
                return numberOfMovementsWithoutAttack;
            }
        }

        public Color GetColor
        {
            get
            {
                return color;
            }
        }

        public Piece SelectPiece(CheckerBoard board)
        {
            while(true)
            {
                Console.WriteLine("Write the x coordinate");
                string x = Console.ReadLine();
                Console.WriteLine("Write the y coordinate");
                string y = Console.ReadLine();
                int coordX = int.Parse(x);
                int coordY = int.Parse(y);
                if (coordX >= 0 && coordX < Config.Cfg.board_size && coordY >= 0 && coordY < Config.Cfg.board_size)
                {
                    if (board[coordX, coordY] == null)
                    {
                        Console.WriteLine("No piece on this position !!!");
                        continue;
                    }
                    if (!IsCorrectPiece(board[coordX, coordY]))
                    {
                        Console.WriteLine("Not your piece !!!");
                        continue;
                    }
                    return board[coordX, coordY];
                }
                Console.WriteLine("Bad coordinates");
            }
            //wprowadzenie pozycje pionkow
        }
        public Position SelectDestination(CheckerBoard board)
        {
            while (true)
            {
                Console.WriteLine("Write the x coordinate");
                string x = Console.ReadLine();
                Console.WriteLine("Write the y coordinate");
                string y = Console.ReadLine();
                int coordX = int.Parse(x);
                int coordY = int.Parse(y);
                if (coordX >= 0 && coordX < Config.Cfg.board_size && coordY >= 0 && coordY < Config.Cfg.board_size)
                {
                    return new Position(coordX, coordY);
                }
                Console.WriteLine("Bad coordinates");
            }
            //wprowadzenie pozycje pionkow
        }

        public bool IsPlayerBlocked(CheckerBoard board)
        {
            foreach (var piece in pieces)
                if (piece.IsPieceBlocked(board) == false)
                    return false;

            return true;
        }

        public bool IsCorrectPiece(Piece piece)
        {
            return piece.pieceColor == color;
        }

        public bool IsPossibleAttack(CheckerBoard board)
        {
            //przechodzi po liscie pionkow i jesli jest mozliwe bicie dla ktoregos pionka
            //zwraca true
            if (pieces == null)
                return false;
            foreach (Piece x in pieces)
                if (x.CanAttack(board))
                    return true;
            return false;
        }

        public void Input(out Piece piece, out Position destination, CheckerBoard board)
        {
            board.DrawBoard(null);
            piece = null;
            destination = new Position(-1,-1);
            bool attackPossible = IsPossibleAttack(board);
            if (pieces.Count == 0)
                return;
            while (true)
            {
                Console.WriteLine("Select piece !!!");
                piece = SelectPiece(board);
                Console.WriteLine("Select destination !!!");
                destination = SelectDestination(board);

                if (attackPossible)
                {
                    if (piece.IsCorrectDestination(true, destination, board))
                    {
                        break;
                    }
                    Console.WriteLine("You have to atack !!!");
                }
                else
                {
                    if (piece.IsCorrectDestination(false, destination, board))
                    {
                        break;
                    }
                    Console.WriteLine("Can't move your piece to this position !!!");
                }
            }
            //zwraca pionek
            //i poprawny cel w sensie ze jest puste pole
        }

        /// <summary>
        /// Function tries to make move selected piece to selected location.
        /// </summary>
        /// <param name="board">Current board</param>
        /// <param name="enemy"></param>
        /// <param name="piece">Selected piece</param>
        /// <param name="destination">Selected destination</param>
        /// <returns>Returns true if player finished his move</returns>
        public bool Turn(CheckerBoard board, Player enemy, Piece piece, Position destination)
        {

            bool attackFlag = IsPossibleAttack(board);
            if (!IsCorrectPiece(piece))
                return false;
            if (!piece.IsCorrectDestination(attackFlag, destination, board))
                return false;
            if (attackPiece != null && piece != attackPiece)
                return false;
            if(piece.CanAttack(board))
            {
                numberOfMovementsWithoutAttack = 0;
                Piece deletePiece = piece.FunkcjaCudzika(board, destination);
                deletePiece.RemovePiece(board, enemy.pieces);
            }
            else
            {
                if(piece.GetType() == typeof(Queen))
                    numberOfMovementsWithoutAttack++;
            }
            piece.Move(board, destination);
            if (attackFlag && piece.CanAttack(board))
            {
                attackPiece = piece;
                return false;
            }
            attackPiece = null;
            if (piece.IsBecomeQueen())
                piece.ChangePieceToQueen(board, this.pieces);
            return true;
        }


    }
}
