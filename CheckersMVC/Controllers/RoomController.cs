using Checkers;
using CheckersMVC.Factories;
using CheckersMVC.Helpers;
using CheckersMVC.Models;
using CheckersMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheckersMVC.Controllers
{
    [Authorize]
    public class RoomController : Controller
    {
        // GET: Room
        private IRoomManager _roomManager = RoomManagerFactory.Instance.RoomManager;
        public ActionResult Index()
        {
            Room[] games =_roomManager.GetAllRooms();
            RoomVM[] gamesVM = VmFromRooms(games);
            return View(gamesVM);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create([Bind(Include = "Name")]Room room)
        {
            var name = User.Identity.Name;
            var newRoom = _roomManager.CreateRoom(room.Name, new Models.User { Name = name });
            if(newRoom != null)
            {
                return RedirectToAction("Index", "Game", new { id = newRoom.Game.GameID });
            }
            return View("Index");
            
        }
        private RoomVM[] VmFromRooms(Room[] rooms)
        {
            RoomVM[] roomVM = new RoomVM[rooms.Length];
            for(int i = 0; i < rooms.Length; i++)
            {
                roomVM[i] = RoomVM.From(rooms[i]);
            }
            return roomVM;
        }
    }
}