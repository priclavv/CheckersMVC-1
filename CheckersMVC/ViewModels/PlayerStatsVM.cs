using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CheckersMVC.Models;

namespace CheckersMVC.ViewModels
{
    public class PlayerStatsVM
    {
        public PlayerStats Stats { get; }
        public List<GameHistory> PlayedGames { get;}
        [Display(Name = "Active since")]
        public string CreationDate { get; }
        public PlayerStatsVM(ApplicationDbContext dbContext, int id)
        {
            Stats = dbContext.PlayerStatsList.First(stats => stats.Id == id);
            PlayedGames = dbContext.GameHistoryList.
                Where(gameHistory => gameHistory.Winner.Id == Stats.ApplicationUserId || gameHistory.Loser.Id == Stats.ApplicationUserId).ToList();
            CreationDate = Stats.CreationDateTime.ToShortDateString();
        }
    }
}