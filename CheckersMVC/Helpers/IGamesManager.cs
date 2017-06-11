using Checkers;
using CheckersMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersMVC.Helpers
{
    public interface IRoomManager
    {
        Room GetRoomById(int roomID);
        Room[] GetAllRooms();
        Room CreateRoom(string name, User owner);
        bool RemoveRoom(int roomID);
        bool AddUserToRoom(User user, int roomID);
        bool RemoveUserFromRoom(User user, int roomID);
        bool SaveChanges(Game g);
        
    }
}
