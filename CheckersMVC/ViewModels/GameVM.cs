using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Checkers;
using System.ComponentModel.DataAnnotations;

namespace CheckersMVC.ViewModels
{
    public class GameVM
    {
        public int GameID { get; set; }
        public PieceVM[,] Board { get; set; }
        public bool IsPlayerTurn { get; set; }
        public int GameState { get; set; }
        [Display(Name="Player 1")]
        public string PlayerName1 { get; set; }
        [Display(Name = "Player 2")]
        public string PlayerName2 { get; set; }
        public static GameVM From(Game g)
        {
            GameVM vm = new GameVM();
            vm.Board = new PieceVM[g.Board.board.GetLength(0),g.Board.board.GetLength(1)];
            for (int i = 0; i < g.Board.board.GetLength(0); ++i)
                for (int j = 0; j < g.Board.board.GetLength(1); ++j)
                    vm.Board[i, j] = PieceVM.From(g.Board.board[i, j]);
            vm.GameID = g.GameID;
            vm.GameState = (int)g.GameState;
            vm.PlayerName1 = g.Player1.Name;
            vm.PlayerName2 = g.Player2.Name;
            return vm;
        }
    }
}