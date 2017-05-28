using Checkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckersMVC.Models
{
    public class Room
    {
        public Game Game { get; set; }
        public string Name { get; set; }
        public User Owner { get; set; }
    }
}