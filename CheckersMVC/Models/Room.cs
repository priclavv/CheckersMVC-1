using Checkers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CheckersMVC.Models
{
    public class Room
    {
        public Game Game { get; set; }
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }
        public User Owner { get; set; }
        public bool IsUserPlayingInRoom(User user)
        {
            if (Game.Player1.Name == user.Name || Game.Player2.Name == user.Name)
                return true;
            return false;
        }
        public bool IsUserOwnerOfRoom(User user)
        {
            return Owner.Name == user.Name;
        }
    }
}