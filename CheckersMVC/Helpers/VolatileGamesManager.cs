using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Checkers;
using CheckersMVC.Models;
using System.Collections.Concurrent;

namespace CheckersMVC.Helpers
{
    public class VolatileGamesManager : IGamesManager
    {
        private ConcurrentDictionary<int, Game> _games;
        public VolatileGamesManager()
        {
            _games = new ConcurrentDictionary<int, Game>();
        }
        public Game CreateGame(int gameID)
        {
            Game newGame = GetGameById(gameID);
            if (newGame != null)
                return newGame;
            newGame = new Game();
            newGame.StartTime = DateTime.Now;
            newGame.GameID = gameID;
            _games.TryAdd(gameID, newGame);
            return newGame;
        }

        public Game GetGameById(int gameID)
        {
            Game game;
            if (!_games.TryGetValue(gameID, out game))
                game = null;
            return game;
        }

        public bool RemoveGame(int gameID)
        {
            Game gameToRemove;
            return _games.TryRemove(gameID, out gameToRemove);
        }

        //We're operating on reference to game object, so no need to save anything
        public bool SaveChanges(Game g)
        {
            return true;
        }
    }
}