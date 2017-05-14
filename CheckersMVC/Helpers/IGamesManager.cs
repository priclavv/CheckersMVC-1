using Checkers;
using CheckersMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersMVC.Helpers
{
    public interface IGamesManager
    {
        Game GetGameById(int gameID);
        Game CreateGame(int gameID);
        bool RemoveGame(int gameID);
        bool SaveChanges(Game g);
        
    }
}
