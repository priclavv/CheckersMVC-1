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
using CheckersMVC.Helpers;
using CheckersMVC.Models;

namespace CheckersMVC.Controllers
{
    public class GameController : Controller
    {
        private readonly IGamesManager _gamesManager = GameManagerFactory.Instance.GamesManager;
        private static int index = 0;
        [Authorize]
        public ActionResult Index(int id = 0)
        {
            var name = User.Identity.Name;
            Game currentGame = _gamesManager.GetGameById(id);
            if (currentGame == null)
                currentGame = _gamesManager.CreateGame(id);
            lock (currentGame)
            {
                GameVM vm = GameVM.From(currentGame);
                index++;
                if ( currentGame.GetPlayerByName(name) == null && !currentGame.AddUserToGame(new Models.User() {Name = name }))
                    name = "";
                if (currentGame.CurrentPlayer != null)
                {
                    vm.IsPlayerTurn = currentGame.CurrentPlayer.Name == name;
                }
                _gamesManager.SaveChanges(currentGame);
                return View(vm);
            }
        }

        [HttpPost]
        public ActionResult Refresh([Bind(Include = "GameID")]RefreshDTO dto)
        {
            int id = dto.GameID;
            Game currentGame = _gamesManager.GetGameById(id);
            if (currentGame == null)
                currentGame = _gamesManager.CreateGame(id);
            var name = User.Identity.Name;
            lock (currentGame)
            {
                GameVM vm = GameVM.From(currentGame);
                if (currentGame.CurrentPlayer != null)
                    vm.IsPlayerTurn = currentGame.CurrentPlayer.Name == name;
                JsonResult result = new JsonResult { Data = JsonConvert.SerializeObject(vm) };
                return result;
            }
        }
        public ActionResult Move(MoveDTO moveCoords)
        {
            GameVM vm = null;
            var name = User.Identity.Name;
            Game currentGame = _gamesManager.GetGameById(moveCoords.GameID);
            if (currentGame == null)
                currentGame = _gamesManager.CreateGame(moveCoords.GameID);
            lock (currentGame)
            {
                Player enemyPlayer = currentGame.CurrentPlayer == currentGame.Player1
                 ? currentGame.Player2
                 : currentGame.Player1;
                if (currentGame.CurrentPlayer.Turn(currentGame.Board, enemyPlayer,
                    currentGame.Board[moveCoords.fromX, moveCoords.fromY], new Position(moveCoords.toX, moveCoords.toY)))
                {
                    currentGame.CurrentPlayer = currentGame.CurrentPlayer == currentGame.Player1 ? currentGame.Player2 : currentGame.Player1;
                }
                vm = GameVM.From(currentGame);
                if (currentGame.CurrentPlayer != null)
                    vm.IsPlayerTurn = currentGame.CurrentPlayer.Name == name;
                _gamesManager.SaveChanges(currentGame);
            }
            JsonResult result = new JsonResult { Data = JsonConvert.SerializeObject(vm) };
            return result;
        }
    }
}