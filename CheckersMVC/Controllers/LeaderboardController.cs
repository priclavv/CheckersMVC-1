using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CheckersMVC.Models;

namespace CheckersMVC.Controllers
{
    public class LeaderboardController : Controller
    {
        // GET: Leaderboard
        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var playerStatsList = db.PlayerStatsList.ToList();
            playerStatsList.Sort((x, y) => x.GamesWonCount > y.GamesWonCount ? -1 : 1);
            return View(playerStatsList);
        }

        // GET: Leaderboard/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var vm = new ViewModels.PlayerStatsVM(new ApplicationDbContext(), id);
                return View(vm);
            }
            catch (Exception e)
            {
                return HttpNotFound(e.Message);
            }
        }
    }
}
