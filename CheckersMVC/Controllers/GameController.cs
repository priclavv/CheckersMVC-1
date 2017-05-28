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
        private readonly IRoomManager _roomsManager = RoomManagerFactory.Instance.RoomManager;
        private static int index = 0;
        [Authorize]
        public ActionResult Index(int id = 0)
        {
            var name = User.Identity.Name;
            Room currentRoom = _roomsManager.GetRoomById(id);
            if (currentRoom == null)
                currentRoom = _roomsManager.CreateRoom("gra", new User() { Name = name });
            var currentGame = currentRoom.Game;
            lock (currentGame)
            {
                GameVM vm = GameVM.From(currentGame, currentRoom.Owner.Name);
                index++;
                if ( currentGame.GetPlayerByName(name) == null && !currentGame.AddUserToGame(new Models.User() {Name = name }))
                    name = "";
                if (currentGame.CurrentPlayer != null)
                {
                    vm.IsPlayerTurn = currentGame.CurrentPlayer.Name == name;
                }
                _roomsManager.SaveChanges(currentGame);
                return View(vm);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Refresh([Bind(Include = "GameID")]RefreshDTO dto)
        {
            var name = User.Identity.Name;
            int id = dto.GameID;
            Room currentRoom = _roomsManager.GetRoomById(id);
            if (currentRoom == null)
                currentRoom = _roomsManager.CreateRoom("gra", new User() { Name = name });
            var currentGame = currentRoom.Game;
            lock (currentGame)
            {
                GameVM vm = GameVM.From(currentGame, currentRoom.Owner.Name);
                if (currentGame.CurrentPlayer != null)
                    vm.IsPlayerTurn = currentGame.CurrentPlayer.Name == name;
                JsonResult result = new JsonResult { Data = JsonConvert.SerializeObject(vm) };
                return result;
            }
        }
        public ActionResult Move(MoveDTO moveCoords)
        {
            GameVM vm;
            var name = User.Identity.Name;
            Room currentRoom = _roomsManager.GetRoomById(moveCoords.GameID);
            if (currentRoom == null)
                currentRoom = _roomsManager.CreateRoom("gra", new User() { Name = name });
            var currentGame = currentRoom.Game;
            lock (currentGame)
            {
                vm = currentGame.MakeTurnAndUpdateGame(moveCoords, name, currentRoom.Owner.Name);
                _roomsManager.SaveChanges(currentGame);
                if (currentGame.GameState == Game.State.Gameover)
                {
                    var dbContext = new ApplicationDbContext();
                    var service = new PlayerStatsService(dbContext);
                    var gameHistoryService = new GameHistoryService(dbContext);
                    service.UpdatePlayerStats(currentGame);
                    gameHistoryService.SaveGameToHistory(currentGame);
                }
            }
            JsonResult result = new JsonResult { Data = JsonConvert.SerializeObject(vm) };
            return result;
        }


    }
}