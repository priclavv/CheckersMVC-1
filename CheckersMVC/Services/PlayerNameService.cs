using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Checkers;

namespace CheckersMVC.Services
{
    public static class PlayerNameService
    {
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