using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class CheckerBoard
    {
        public Piece[,] board;
        public Piece this[Position pos]
        {
            get
            {
                if(!pos.IsPositionInRange())
                    throw new IndexOutOfRangeException();
                return board[pos.x, pos.y];
            }
            set
            {
                if (!pos.IsPositionInRange())
                    throw new IndexOutOfRangeException();
                board[pos.x, pos.y] = value;
            }
        }
        public Piece this[int x, int y]
        {
            get
            {
                Position pos = new Position(x, y);
                if (!pos.IsPositionInRange())
                    throw new IndexOutOfRangeException();
                return board[x, y];
            }
        }
        public CheckerBoard()
        {
            //ustawia pionki w Board
            board = new Piece[Config.Cfg.board_size, Config.Cfg.board_size];

            // dolna czesc z bialymi pionkami
            int row = Config.Cfg.board_size / 2 - 1;
            for (int y = 0; y < row; y++)
                for (int x = 0; x < Config.Cfg.board_size; x+=2)
                       board[x + y%2, y] = new Piece(Color.WHITE, new Position(x + y%2, y));
            
           // gorna czesc z czarnymi pionkami
            for (int y = Config.Cfg.board_size - row; y < Config.Cfg.board_size; y++)
                for (int x = 0; x < Config.Cfg.board_size; x+=2)
                        board[x + y%2, y] = new Piece(Color.BLACK, new Position(x + y%2, y));
        }

        public CheckerBoard(int[,] array)
        {
            board = new Piece[Config.Cfg.board_size, Config.Cfg.board_size];
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    int x = j;
                    int y = Config.Cfg.board_size - i - 1;
                    if (array[i, j] != 0)
                        board[x, y] = new Piece((Color)array[i, j], new Position(x, y));
                }
        }

        public void DrawBoard(Player player)
        {
            for (int y = board.GetLength(1) - 1; y >= 0; y--)
            {
                Console.Write("{0}   ", y);
                for (int x = 0; x < board.GetLength(0); x++)
                {
                    if (board[x, y] != null)
                        Console.Write(board[x, y] + "  ");
                    else
                        Console.Write("*  ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.Write("    ");
            for (int i = 0; i < board.GetLength(0); i++)
                Console.Write("{0}  ", i);
            Console.WriteLine();
        }


    }
}
