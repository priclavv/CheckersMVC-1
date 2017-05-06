using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Checkers;

namespace CheckersMVC.Factories
{
    public class GameFactory
    {
        private static readonly Lazy<GameFactory> lazy =
            new Lazy<GameFactory>(() => new GameFactory());

        public static GameFactory Instance => lazy.Value;

        private GameFactory()
        {
            Game = new Game();
        }

        public Game Game { get; set; }
    }
}