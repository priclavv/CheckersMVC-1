using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Checkers;
using CheckersMVC.Helpers;

namespace CheckersMVC.Factories
{
    public class RoomManagerFactory
    {
        private static readonly Lazy<RoomManagerFactory> lazy =
            new Lazy<RoomManagerFactory>(() => new RoomManagerFactory());

        public static RoomManagerFactory Instance => lazy.Value;

        private RoomManagerFactory()
        {
            RoomManager = new VolatileGamesManager();
        }

        public IRoomManager RoomManager { get; set; }
    }
}