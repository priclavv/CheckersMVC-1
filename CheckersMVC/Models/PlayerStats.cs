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
        [Display(Name = "Played")]
        public int GamesPlayedCount { get; set; }
        [Display(Name = "Won")]
        public int GamesWonCount { get; set; }
        [Display(Name = "Drawn")]
        public int GamesDrawnCount { get; set; }
        [Display(Name = "Lost")]
        public int GamesLostCount { get; set; }
        public DateTime CreationDateTime { get; set; }
        public virtual ApplicationUser User { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
    }
}