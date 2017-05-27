﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckersMVC.Models
{
    public class GameHistory
    {
        public int Id { get; set; }
        public string WinnerId { get; set; }
        public string LoserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public virtual ApplicationUser Winner { get; set; }
        public virtual ApplicationUser Loser { get; set; }
    }
}