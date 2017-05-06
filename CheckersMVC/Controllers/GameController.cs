using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Checkers;
using CheckersMVC.DTOs;
using CheckersMVC.Factories;
using CheckersMVC.Services;
using CheckersMVC.ViewModels;
using Newtonsoft.Json;

namespace CheckersMVC.Controllers
{
    public class GameController : Controller
    {
        private readonly Game _currentGame = GameFactory.Instance.Game;
        private static int index = 0;
        [Authorize]
        public ActionResult Index()
        {
            var name = User.Identity.Name;
            lock (_currentGame)
            {
                GameVM vm = GameVM.From(_currentGame);
                index++;
                if ( _currentGame.GetPlayerByName(name) == null && !_currentGame.SetPlayerName(name))
                    name = "";
                if (_currentGame.CurrentPlayer != null)
                    vm.IsPlayerTurn = _currentGame.CurrentPlayer.Name == name;
                return View(vm);
            }
        }

        public ActionResult Refresh()
        {
            var name = User.Identity.Name;
            lock (_currentGame)
            {
                GameVM vm = GameVM.From(_currentGame);
                if (_currentGame.CurrentPlayer != null)
                    vm.IsPlayerTurn = _currentGame.CurrentPlayer.Name == name;
                JsonResult result = new JsonResult { Data = JsonConvert.SerializeObject(vm) };
                return result;
            }
        }
        public ActionResult Move(MoveDTO moveCoords)
        {
            GameVM vm = null;
            var name = User.Identity.Name;
            lock (_currentGame)
            {
                Player enemyPlayer = _currentGame.CurrentPlayer == _currentGame.Player1
                 ? _currentGame.Player2
                 : _currentGame.Player1;
                if (_currentGame.CurrentPlayer.Turn(_currentGame.Board, enemyPlayer,
                    _currentGame.Board[moveCoords.fromX, moveCoords.fromY], new Position(moveCoords.toX, moveCoords.toY)))
                {
                    _currentGame.CurrentPlayer = _currentGame.CurrentPlayer == _currentGame.Player1 ? _currentGame.Player2 : _currentGame.Player1;
                }
                vm = GameVM.From(_currentGame);
                if (_currentGame.CurrentPlayer != null)
                    vm.IsPlayerTurn = _currentGame.CurrentPlayer.Name == name;
            }
            JsonResult result = new JsonResult { Data = JsonConvert.SerializeObject(vm) };
            return result;
        }
    }
}