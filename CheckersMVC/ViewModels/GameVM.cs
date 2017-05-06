using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Checkers;

namespace CheckersMVC.ViewModels
{
    public class GameVM
    {
        public PieceVM[,] Board { get; set; }
        public bool IsPlayerTurn { get; set; }
        public static GameVM From(Game g)
        {
            GameVM vm = new GameVM();
            vm.Board = new PieceVM[g.Board.board.GetLength(0),g.Board.board.GetLength(1)];
            for (int i = 0; i < g.Board.board.GetLength(0); ++i)
                for (int j = 0; j < g.Board.board.GetLength(1); ++j)
                    vm.Board[i, j] = PieceVM.From(g.Board.board[i, j]);
            return vm;
        }
    }
}