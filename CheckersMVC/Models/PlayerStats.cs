using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CheckersMVC.Models
{
    public class PlayerStats
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int GamesWonCount { get; set; }

        public virtual ApplicationUser User { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
    }
}