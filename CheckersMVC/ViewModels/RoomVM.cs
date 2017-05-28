using CheckersMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckersMVC.ViewModels
{
    public class RoomVM
    {
        public int GameID { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }

        public static RoomVM From(Room room)
        {
            RoomVM vm = new RoomVM()
            {
                GameID = room.Game.GameID,
                Name = room.Name,
                Owner = room.Owner.Name,
                Player1 = room.Game.Player1.Name,
                Player2 = room.Game.Player2.Name
            };
            return vm;
        }
    }
}