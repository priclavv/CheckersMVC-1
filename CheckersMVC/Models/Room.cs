using Checkers;
using CheckersMVC.Helpers;
using CheckersMVC.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Timers;
using System.Web;

namespace CheckersMVC.Models
{
    public class Room
    {
        public Game Game { get; set; }
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }
        public User Owner { get; set; }
        private const int absenceLimitInSeconds = 60;
        private Timer User1AbsenceTimer { get; set; }
        private Timer User2AbsenceTimer { get; set; }
        public bool TryStartGame()
        {
            if (Game.GameState == Game.State.Game)
                return true;
            Game.SetGameState();
            //if(Game.GameState == Game.State.Game)
            //{
            //    StartUsersTimer(new User() { Name = Game.Player1.Name });
            //    StartUsersTimer(new User() { Name = Game.Player2.Name });
            //}
            if (Game.GameState == Game.State.Game)
                return true;
            return false;
        }
        public bool RemoveUserFromRoom(User user)
        {
            if (IsUserOwnerOfRoom(user))
                Owner.Name = null;
            StopUsersTimer(user);
            return Game.RemoveUserFromGame(user);
        }
        private void StopUsersTimer(User user)
        {
            if (user.Name == Game.Player1.Name)
            {
                User1AbsenceTimer.Stop();
                User1AbsenceTimer = null;
            }
            if(user.Name == Game.Player2.Name)
            {
                User2AbsenceTimer.Stop();
                User2AbsenceTimer = null;
            }
        }
        public void RestartGame()
        {
            var player1Name = Game.Player1.Name;
            var player2Name = Game.Player2.Name;
            Game.InitGame();
            if(player1Name != null)
                Game.AddUserToGame(new User() { Name = player1Name });
            StartUsersTimer(new User() { Name = player1Name });
            if(player2Name != null)
                Game.AddUserToGame(new User() { Name = player2Name });
            StartUsersTimer(new User() { Name = player2Name });
            TryStartGame();
        }
        public void StartUsersTimer(User user)
        {
            if (!IsUserPlayingInRoom(user))
                return;
            Timer timer = null;
            if (user.Name == Game.Player1.Name)
            {
                User1AbsenceTimer = new Timer();
                timer = User1AbsenceTimer;
            }
            if (user.Name == Game.Player2.Name)
            {
                User2AbsenceTimer = new Timer();
                timer = User2AbsenceTimer;
            }
            timer.Interval = absenceLimitInSeconds * 1000;
            TimerEventHelper helper = new TimerEventHelper(user, this);
            timer.Elapsed += new ElapsedEventHandler(helper.OnAbsenceTooLong);
            timer.Start();
        }
        public void MarkUserAsActive(User user)
        {
            Timer timer = GetUsersAbsenceTimer(user);
            if (timer != null)
                ResetTimer(timer);
        }
        private void ResetTimer(Timer timer)
        {
            timer.Stop();
            timer.Start();
            timer.Interval = absenceLimitInSeconds * 1000;
        }
        private Timer GetUsersAbsenceTimer(User user)
        {
            if (user.Name == Game.Player1.Name)
                return User1AbsenceTimer;
            if (user.Name == Game.Player2.Name)
                return User2AbsenceTimer;
            return null;
        }
        public bool IsUserPlayingInRoom(User user)
        {
            if (Game.Player1.Name == user.Name || Game.Player2.Name == user.Name)
                return true;
            return false;
        }
        public bool IsUserOwnerOfRoom(User user)
        {
            return Owner.Name == user.Name;
        }
    }
}