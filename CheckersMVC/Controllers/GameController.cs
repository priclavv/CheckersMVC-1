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
using System.Net;

namespace CheckersMVC.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly IRoomManager _roomsManager = RoomManagerFactory.Instance.RoomManager;
        public ActionResult Index(int id = 0)
        {
            var name = User.Identity.Name;
            Room currentRoom = _roomsManager.GetRoomById(id);
            if (currentRoom == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            var currentGame = currentRoom.Game;
            if (name == currentRoom.Owner.Name)
                _roomsManager.AddUserToRoom(new User() {Name = name}, id);
            lock (currentGame)
            {
                GameVM vm = GameVM.From(currentGame, currentRoom.Owner.Name);
                if (currentGame.CurrentPlayer != null)
                {
                    vm.IsPlayerTurn = currentGame.CurrentPlayer.Name == name;
                }
                _roomsManager.SaveChanges(currentGame);
                return View(vm);
            }
        }
        [HttpPost]
        public ActionResult Join([Bind(Include = "GameID")]RefreshDTO dto)
        {
            var name = User.Identity.Name;
            int id = dto.GameID;
            Room currentRoom = _roomsManager.GetRoomById(id);
            if (currentRoom == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            var currentGame = currentRoom.Game;
            lock (currentGame)
            {
                GameVM vm = GameVM.From(currentGame, currentRoom.Owner.Name);
                _roomsManager.AddUserToRoom(new User() {Name = name}, id);
                return Refresh(dto);
            }

        }
        [HttpPost]
        public ActionResult Restart([Bind(Include = "GameID")]RefreshDTO dto)
        {
            Room currentRoom = _roomsManager.GetRoomById(dto.GameID);
            var name = User.Identity.Name;
            var playerName1 = currentRoom.Game.Player1.Name;
            var playerName2 = currentRoom.Game.Player2.Name;
            if (playerName1 != name && playerName2 != name)
                return HttpNotFound();
            if (currentRoom.Game.GameState == Game.State.Game)
                return HttpNotFound();
            currentRoom.Game.InitGame();
            currentRoom.Game.AddUserToGame(new User(){ Name = playerName1 });
            currentRoom.Game.AddUserToGame(new User(){ Name = playerName2 });
            return Refresh(dto);
        }
        [HttpPost]
        public ActionResult Refresh([Bind(Include = "GameID")]RefreshDTO dto)
        {
            var name = User.Identity.Name;
            int id = dto.GameID;
            Room currentRoom = _roomsManager.GetRoomById(id);
            if (currentRoom == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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
        [HttpPost]
        public ActionResult Move(MoveDTO moveCoords)
        {
            GameVM vm;
            var name = User.Identity.Name;
            Room currentRoom = _roomsManager.GetRoomById(moveCoords.GameID);
            if (currentRoom == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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

        public ActionResult Quit([Bind(Include = "GameID")]RefreshDTO dto)
        {
            if (_roomsManager.RemoveUserFromRoom(new User() {Name = User.Identity.Name}, dto.GameID))
            {
                var currentRoom = _roomsManager.GetRoomById(dto.GameID);
                var playerName = currentRoom.Game.Player1.Name ?? currentRoom.Game.Player2.Name;
                currentRoom.Game.InitGame();
                currentRoom.Game.AddUserToGame(new User() { Name = playerName });
            }
            return Refresh(dto);
        } 


    }
}