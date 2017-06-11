using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web;
using CheckersMVC.Models;

namespace CheckersMVC.ViewModels
{
    public class PlayerStatsVM
    {
        public PlayerStats Stats { get; }
        public List<GameHistoryVM> PlayedGames { get; private set; }
        [Display(Name = "Active since")]
        public string CreationDate { get; }
        public PlayerStatsVM(ApplicationDbContext dbContext, int id)
        {
            Stats = dbContext.PlayerStatsList.First(stats => stats.Id == id);
            CreateListOfPlayedGames(dbContext);
            CreationDate = Stats.CreationDateTime.ToShortDateString();
        }

        private void CreateListOfPlayedGames(ApplicationDbContext dbContext)
        {
            var games = dbContext.GameHistoryList.
                Where(gameHistory => gameHistory.Winner.Id == Stats.ApplicationUserId || gameHistory.Loser.Id == Stats.ApplicationUserId).ToList();
            PlayedGames = new List<GameHistoryVM>();
            foreach (var gameHistory in games)
            {
                PlayedGames.Add(new GameHistoryVM(dbContext, gameHistory, Stats));
            }
        }
    }

    public class GameHistoryVM
    {
        private readonly ApplicationDbContext _dbContext;
        public string CurrentPlayerName { get; }
        public string OpponentName { get; private set; }
        public int OpponentStatsId { get; private set; }
        public int GameDurationInMinutes { get; }
        public DateTime StartDateTime { get; }
        public string Result { get; private set; }

        public GameHistoryVM(ApplicationDbContext db, GameHistory gameHistory, PlayerStats stats)
        {
            _dbContext = db;
            CurrentPlayerName = stats.Name;
            if (CurrentPlayerName == gameHistory.Winner.UserName)
                SetOpponent(gameHistory.Loser);
            else
                SetOpponent(gameHistory.Winner);
            GameDurationInMinutes = gameHistory.EndTime.Subtract(gameHistory.StartTime).Minutes;
            StartDateTime = gameHistory.StartTime;
            SetResult(gameHistory);

        }

        private void SetResult(GameHistory gameHistory)
        {
            if (gameHistory.IsDraw)
                Result = "Draw";
            else if (CurrentPlayerName == gameHistory.Winner.UserName)
                Result = "Win";
            else
                Result = "Lost";
        }

        private void SetOpponent(ApplicationUser opponent)
        {
            OpponentName = opponent.UserName;
            OpponentStatsId = _dbContext.PlayerStatsList.First(stats => stats.ApplicationUserId == opponent.Id).Id;
        }
    }
}