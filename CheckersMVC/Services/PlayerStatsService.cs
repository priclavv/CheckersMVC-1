using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Checkers;
using CheckersMVC.Models;

namespace CheckersMVC.Services
{
    public class PlayerStatsService
    {
        private readonly ApplicationDbContext _dbContext;

        public PlayerStatsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private void UpdateWinnerAndLoser(Game currentGame)
        {
            var winner = _dbContext.PlayerStatsList.First(x => x.Name == currentGame.Winner.Name);
            string loserName = currentGame.Winner == currentGame.Player1
                ? currentGame.Player2.Name
                : currentGame.Player1.Name;
            var loser = _dbContext.PlayerStatsList.First(x => x.Name == loserName);
            winner.GamesWonCount++;
            winner.GamesPlayedCount++;
            loser.GamesLostCount++;
            loser.GamesPlayedCount++;
        }

        private void UpdateDraw(Game currentGame)
        {
            var playerStats1 = _dbContext.PlayerStatsList.First(x => x.Name == currentGame.Player1.Name);
            var playerStats2 = _dbContext.PlayerStatsList.First(x => x.Name == currentGame.Player2.Name);
            playerStats1.GamesPlayedCount++;
            playerStats1.GamesDrawnCount++;
            playerStats2.GamesPlayedCount++;
            playerStats2.GamesDrawnCount++;
        }

        public void UpdatePlayerStats(Game currentGame)
        {
            if(currentGame.Winner == null)
                UpdateDraw(currentGame);
            else
                UpdateWinnerAndLoser(currentGame);
            _dbContext.SaveChanges();
        }
    }
}