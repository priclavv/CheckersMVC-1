using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckersMVC.DTOs
{
    public class MoveDTO
    {
        public int fromX { get; set; }
        public int fromY { get; set; }
        public int toX { get; set; }
        public int toY { get; set; }

    }
}