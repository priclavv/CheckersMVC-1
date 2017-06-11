using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Checkers;
using CheckersMVC.Models;

namespace CheckersMVC.Services
{
    public static class PlayerNameService
    {
        public static bool AddUserToGame(this Game g, User user)
        {
            bool isSuccess = g.SetPlayerName(user.Name);
            g.TryStartGame();
            return isSuccess;
        }
        public static bool RemoveUserFromGame(this Game g, User user)
        {
            if (g.Player1.Name == user.Name)
                g.Player1.Name = null;
            if (g.Player2.Name == user.Name)
                g.Player2.Name = null;
            g.SetGameState();
            return true;
        }
        public static bool SetPlayerName(this Game g, string name)
        {
            if (g.Player1.Name == null)
            {
                g.Player1.Name = name;
                return true;
            }
            if (g.Player2.Name == null)
            {
                g.Player2.Name = name;
                return true;
            }
            return false;
        }

        private static bool TryStartGame(this Game g)
        {
            if(g.Player1.Name != null && g.Player2.Name != null)
            {
                g.StartTime = DateTime.Now;
                return true;
            }
            return false;
        }

        public static Player GetPlayerByName(this Game g, string name)
        {
            if (name == g.Player1.Name)
                return g.Player1;
            if (name == g.Player2.Name)
                return g.Player2;
            return null;

        }
    }
}