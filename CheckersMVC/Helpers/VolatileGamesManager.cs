using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Checkers;
using CheckersMVC.Models;
using System.Collections.Concurrent;
using CheckersMVC.Services;

namespace CheckersMVC.Helpers
{
    public class VolatileGamesManager : IRoomManager
    {
        private ConcurrentDictionary<int, Room> _rooms;
        private int _limit = 50;
        public VolatileGamesManager()
        {
            _rooms = new ConcurrentDictionary<int, Room>();
        }

        public bool AddUserToRoom(User user, int roomID)
        {
            if (!CanUserJoinGame(user))
                return false;
            var room = GetRoomById(roomID);
            if (room == null)
                return false;
            return room.Game.AddUserToGame(user);
        }

        public Room CreateRoom(string name, User owner)
        {
            if (!CanUserCreateRoom(owner))
                return null;
            if (_rooms.Count == _limit)
                return null;
            int id = CalculateFreeRoomId();
            Game game = new Game()
            {
                GameID = id,
                GameState = Game.State.Nogame
            };
            Room room = new Room()
            {
                Game = game,
                Name = name,
                Owner = owner
            };
            _rooms.TryAdd(id, room);
            return room;

        }
        private bool CanUserJoinGame(User user)
        {
            var users = _rooms.Where(r => r.Value.IsUserPlayingInRoom(user));
            if (users.Count() == 0)
                return true;
            return false;
        }
        private bool CanUserCreateRoom(User user)
        {
            var users = _rooms.Where(r => r.Value.IsUserOwnerOfRoom(user) || r.Value.IsUserPlayingInRoom(user));
            if (users.Count() == 0)
                return true;
            return false;
        }
        private int CalculateFreeRoomId()
        {
            int currentId = 0;
            int[] keys = _rooms.Keys.OrderBy(k => k).ToArray();
            if (keys.Length == 0)
                return currentId;
            foreach(var id in keys)
            {
                if (id != currentId)
                    return currentId;
                currentId++;
            }
            return keys.Last() + 1;
        }
        public Room[] GetAllRooms()
        {
            return _rooms.Values.ToArray();
        }

        public Room GetRoomById(int roomID)
        {
            Room room;
            if (!_rooms.TryGetValue(roomID, out room))
                room = null;
            return room;
        }

        public bool RemoveRoom(int roomID)
        {
            Room roomToRemove;
            return _rooms.TryRemove(roomID, out roomToRemove);
        }

        public bool RemoveUserFromRoom(User user, int roomID)
        {
            var room = GetRoomById(roomID);
            if (room == null)
                return false;
            return room.Game.RemoveUserFromGame(user);
        }

        //We're operating on reference to game object, so no need to save anything
        public bool SaveChanges(Game g)
        {
            return true;
        }
    }
}