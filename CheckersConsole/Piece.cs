using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
   public class Piece
    {
        public Color pieceColor { get; } // Kacper: zmienilem pole na wlasciwosc tylko do odczytu, przydatne np w klasie player
        protected Position position;
        public Piece(Color color, Position defaultPosition)
        {
            pieceColor = color;
            position = defaultPosition;
        }

        public virtual bool IsPossibleSingleMove(CheckerBoard board)
        {
            // sprawdza czy pionek moze ruszyc sie o jedna pozycje
            Position right, left;
            if(pieceColor == Color.White)
            {
                left = position.NW();
                right = position.NE();
            }
            else
            {
                left = position.SW();
                right = position.SE();       
            }
           
            if (!IsCorrectDestination(false, left, board) && !IsCorrectDestination(false, right, board))
                return false;

            return true;
        }

        public virtual bool IsPieceBlocked(CheckerBoard board)
        {
            if (CanAttack(board))
                return false;

            return !IsPossibleSingleMove(board);
        }

        public virtual bool IsCorrectDestination(bool attackFlag, Position destination, CheckerBoard board)
        {
            if (!destination.IsPositionInRange())
            {
                return false;
            }
            if (attackFlag)
            {
                return CheckAttack(board, destination);
            }
            else
            {
                int diffX = destination.x - position.x;
                int diffY = destination.y - position.y;
                if ((pieceColor == Color.White && diffY == 1 && Math.Abs(diffX) == 1) 
                    || (pieceColor == Color.Black && diffY == -1 && Math.Abs(diffX) == 1))
                {
                    if (board[destination] == null)
                        return true;
                }
            }
            return false;
            //jesli attackFlag jest true to jesli jest bicie i destination jest oddalone o dwa zwraca true
            //jesli attackFlag jest false  to zwraca true jesli destination jest oddalone o jeden
        }

        public virtual bool CheckAttack(CheckerBoard board, Position destination)
        {
            // sprawdza czy wykonujac ruch na pozycje destination wystapi bicie
            if(!destination.IsPositionInRange())
            {
                return false;
            }
            int diffX = destination.x - position.x;
            int diffY = destination.y - position.y;
            Position attackedField = new Position(diffX < 0 ? position.x - 1 : position.x + 1, diffY < 0 ? position.y - 1 : position.y + 1);
            if((diffX == 2 || diffX == -2) && (diffY == 2 || diffY == -2)) // jesli destination jest o dwa pola na skos
            {
                if(board[destination] == null)
                {
                    if (board[attackedField] != null && board[attackedField].pieceColor != pieceColor)
                        return true;
                }
            }
            return false;
            
        }
        public virtual bool CanAttack(CheckerBoard board)
        {
            if (!position.IsPositionInRange())
            {
                throw new IndexOutOfRangeException();
            }
            return CheckAttack(board, position.NE().NE()) || CheckAttack(board, position.NW().NW()) || CheckAttack(board, position.SE().SE())
                    || CheckAttack(board, position.SW().SW());
           
            //sprawdza czy pionek ma bicie
        }

        public void Move(CheckerBoard board, Position to)
        {
            board[this.position] = null;
            board[to] = this;
            this.position = to;
        }

        public void RemovePiece(CheckerBoard board, List<Piece> pieces)
        {
            //usuwa pionek z listy i z planszy
            board[this.position] = null;
            pieces.Remove(this);
        }

        public bool IsBecomeQueen()
        {
            if (pieceColor == Color.Black && position.y == 0)
                return true;
            return pieceColor == Color.White && position.y == Config.Cfg.board_size - 1 ? true : false;
            //sprawdza czy pionek jest dama
        }

        public void ChangePieceToQueen(CheckerBoard board, List<Piece> pieces)
        {
            pieces[pieces.IndexOf(this)] = (board[this.position] = new Queen(this.pieceColor, this.position));
        }

        public Piece FunkcjaCudzika(CheckerBoard board, Position destination)
        {
            int xx = (destination.x - position.x) < 0 ? -1 : 1,
                yy = (destination.y - position.y) < 0 ? -1 : 1;

            for (int x = position.x+xx, y = position.y+yy; x != destination.x; x += xx, y += yy)
                if (board[x, y] != null) //raczej juz przed wejsciem mamy pewnosc ze -> && Board[x, y].pieceColor != this.pieceColor)
                    return board[x, y];
            return null;
            //zwraca wystepujacego pionka ktory zostal przeskoczony podczas bicia
        }

        //4x funkcja sprawdzajaca po przekatnej 1 pole od siebie
        public override string ToString() // pomocnicza do wypisywania
        {
            if (Color.Black == pieceColor)
                return "#";
            return "@";
        }
    }

    public class Queen : Piece
    {
        public Queen(Color color, Position defaultPosition):base(color,defaultPosition)
            {}

        public override bool CheckAttack(CheckerBoard board, Position destination)
        {
            List<Piece> piecesBetweenDestAndPos = new List<Piece>();
            Position iterPosition = new Position(position.x, position.y);
            int directionX = destination.x > position.x ? 1 : -1;
            int directionY = destination.y > position.y ? 1 : -1;
            if(!destination.IsPositionInRange())
            {
                return false;
            }
            if(board[destination] != null)
            {
                return false;
            }
            if(Math.Abs(position.x - destination.x) < 2)
            {
                return false;
            }
            if(position.IsPositionOnBias(destination))
            { 
                while (iterPosition.x != destination.x) // wiemy ze jestesmy na przekatnej, porownujemy tylko x
                {
                    iterPosition.x += directionX;
                    iterPosition.y += directionY;
                    if (board[iterPosition] != null)
                    { 
                        piecesBetweenDestAndPos.Add(board[iterPosition]);
                    }
                }
            }
            if(piecesBetweenDestAndPos.Count == 1)
            {
                if (piecesBetweenDestAndPos[0].pieceColor != pieceColor)
                    return true;
            }
            return false;
        }
        public bool CanAttackInDirection(CheckerBoard board, int directionX, int directionY)
        {
            // directionX i directionY sa rowne 1 lub -1, dla np. directionX = 1, directonY = -1 sprawdzamy czy mozna zaatakowac na poludniowy zachód
            Position iterPosition = new Position(position.x, position.y);
            if((directionX == 1 || directionX == -1) && (directionY == 1 || directionY == -1))
            {
                iterPosition.x += directionX;
                iterPosition.y += directionY;
                while(iterPosition.IsPositionInRange())
                {
                    if(board[iterPosition] != null)
                    {
                        if (board[iterPosition].pieceColor == pieceColor)
                            return false;
                        else
                        {
                            iterPosition.x += directionX;
                            iterPosition.y += directionY;
                            if(iterPosition.IsPositionInRange() && board[iterPosition] == null)
                            {
                                return true;
                            }
                            return false;
                        }
                    }
                    iterPosition.x += directionX;
                    iterPosition.y += directionY;
                }
            }
            return false;

        }
        public override bool CanAttack(CheckerBoard board)
        {
            return CanAttackInDirection(board, 1, 1) || CanAttackInDirection(board, 1, -1) || CanAttackInDirection(board, -1, -1) || CanAttackInDirection(board, -1, 1);
        }

        public override bool IsPossibleSingleMove(CheckerBoard board)
        {
            Position upperLeft = position.NW();
            Position upperRight = position.NE();
            Position bottomLeft = position.SW();
            Position bottomRight = position.SE();


            if (!IsCorrectDestination(false, upperLeft, board) && !IsCorrectDestination(false, upperRight, board) &&
                !IsCorrectDestination(false, bottomLeft, board) && !IsCorrectDestination(false, bottomRight, board))
                return false;

            return true;
        }

        public override bool IsPieceBlocked(CheckerBoard board)
        {
            if (CanAttack(board))
                return false;

            return !IsPossibleSingleMove(board);
        }

        public override bool IsCorrectDestination(bool attackFlag, Position destination, CheckerBoard board)
        {
            List<Piece> piecesBetweenDestAndPos = new List<Piece>();
            Position iterPosition = new Position(position.x, position.y);
            int directionX = destination.x > position.x ? 1 : -1;
            int directionY = destination.y > position.y ? 1 : -1;
            if (!destination.IsPositionInRange())
            {
                return false;
            }
            if (attackFlag)
            {
                return CheckAttack(board, destination);
            }
            if(!position.IsPositionOnBias(destination))
            {
                return false;
            }
            if(board[destination] != null)
            {
                return false;
            }
            while (iterPosition.x != destination.x) // wiemy ze jestesmy na przekatnej, porownujemy tylko x
            {
                        iterPosition.x += directionX;
                        iterPosition.y += directionY;
                        if (board[iterPosition] != null)
                        {
                            piecesBetweenDestAndPos.Add(board[iterPosition]);
                        }
                    
            }
            if (piecesBetweenDestAndPos.Count == 0)
            {
                    return true;
            }
            return false;
        }
    }
}

