using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Checkers;
using CheckersMVC.Helpers;

namespace CheckersMVC.Factories
{
    public class GameManagerFactory
    {
        private static readonly Lazy<GameManagerFactory> lazy =
            new Lazy<GameManagerFactory>(() => new GameManagerFactory());

        public static GameManagerFactory Instance => lazy.Value;

        private GameManagerFactory()
        {
            GamesManager = new VolatileGamesManager();
        }

        public IGamesManager GamesManager { get; set; }
    }
}