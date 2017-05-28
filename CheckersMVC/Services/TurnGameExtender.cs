using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Checkers;
using CheckersMVC.DTOs;
using CheckersMVC.ViewModels;

namespace CheckersMVC.Services
{
    public static class TurnGameExtender
    {
        public static GameVM MakeTurnAndUpdateGame(this Game currentGame, MoveDTO moveCoords, string name, string ownerName)
        {
            GameVM vm;
            Player enemyPlayer = currentGame.CurrentPlayer == currentGame.Player1
                    ? currentGame.Player2
                    : currentGame.Player1;
            if (currentGame.CurrentPlayer.Turn(currentGame.Board, enemyPlayer,
                currentGame.Board[moveCoords.fromX, moveCoords.fromY], new Position(moveCoords.toX, moveCoords.toY)))
            {
                currentGame.CurrentPlayer = currentGame.CurrentPlayer == currentGame.Player1 ? currentGame.Player2 : currentGame.Player1;
            }
            vm = GameVM.From(currentGame, ownerName);
            if (currentGame.CurrentPlayer != null)
                vm.IsPlayerTurn = currentGame.CurrentPlayer.Name == name;
            currentGame.SetGameState();
            vm.GameState = (int) currentGame.GameState;
            return vm;
        }
    }
}