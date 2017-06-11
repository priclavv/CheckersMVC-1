using Checkers;
using CheckersMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckersMVC.Services
{
    public class GameHistoryService
    {
        private ApplicationDbContext _dbContext;
        public GameHistoryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void SaveGameToHistory(Game currentGame)
        {
            string winnerName = null;
            string loserName = null;
            bool isDraw = false;
            if(currentGame.Winner == null)
            {
                winnerName = currentGame.Player1.Name;
                loserName = currentGame.Player2.Name;
                isDraw = true;
            }
            else
            {
                winnerName = currentGame.Winner.Name;
                loserName = winnerName == currentGame.Player1.Name ? currentGame.Player2.Name : currentGame.Player1.Name;
            }
            var gameHistory = new GameHistory()
            {
                WinnerId = _dbContext.Users.Where(u => u.UserName == winnerName).First().Id,
                LoserId = _dbContext.Users.Where(u => u.UserName == loserName).First().Id,
                StartTime = currentGame.StartTime,
                EndTime = DateTime.Now,
                IsDraw = isDraw
            };
            _dbContext.GameHistoryList.Add(gameHistory);
            _dbContext.SaveChanges();
        }
    }
}