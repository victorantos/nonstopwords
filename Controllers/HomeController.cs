using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Mvc;
using Wordament;
using Wordament.Models;

namespace wwwroot.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Test()
        {

            return View();

        }

        public ActionResult Index()
        {
            ViewBag.Message = "My game. Wordsearch maker.";
            var defaultGame = new DailyPuzzleViewModel
            {
                GameId = 1,
                Language = "en",
                IsDaily = true,
                ShowStopwatch = true
            };
            var dbContext = new Entities();
            var maxLimit = DateTime.UtcNow.AddDays(1);
            var firstSunday = new DateTime(1753, 1, 7);
            int x = (int)DateTime.UtcNow.DayOfWeek;
            var now = DateTime.Now;
            var todayPuzzle = dbContext.DailyPuzzles.Where(p => p.DateWhenActive.HasValue && (EntityFunctions.DiffDays(firstSunday, p.DateWhenActive.Value) % 7 == x) && (EntityFunctions.DiffDays(p.DateWhenActive.Value, now) == 0)).FirstOrDefault();

            if (todayPuzzle != null)
            {
                var gameGroup = dbContext.GameGroups.Where(gg => gg.GameGroupId == todayPuzzle.GameGroupId).FirstOrDefault();
                if (gameGroup != null)
                {
                    defaultGame.GameId = gameGroup.GameGroupId;
                    defaultGame.Language = gameGroup.Language;
                }
            }

            var myGameViewModel = new MyGameViewModel
            {
                DailyPuzzleViewModel = defaultGame,
                HallOfFameViewModel = new HallOfFameViewModel { GameGroupId = todayPuzzle != null ? todayPuzzle.GameGroupId : -1 }
            };
            return View(myGameViewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult PuzzleMaker()
        {
            ViewBag.Message = "Puzzle Maker";

            return View();
        }

        public ActionResult Multiplayer()
        {
            ViewBag.Message = "Multiplayer";

            return View();
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult Feedback(Wordament.Models.Feedback feedback)
        {
            var dbContext = new Entities();
            Wordament.Feedback f = new Wordament.Feedback
            {
                Email = feedback.Email,
                Message = feedback.Message,
                DateCreated = DateTime.UtcNow,
                IPAddress = Request.UserHostAddress
            };
            dbContext.Entry(f).State = System.Data.Entity.EntityState.Added;
            dbContext.SaveChanges();
            return Json(new { result = "success" });
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Thank you for your feedback!";

            return View();
        }
        public ActionResult HowtoPlay()
        {
            ViewBag.Message = "Video demo.";

            return View();
        }
        public ActionResult MyGame()
        {
            return View();
        }

        public class MyGameViewModel
        {
            public GameViewModel DailyPuzzleViewModel { get; set; }
            public HallOfFameViewModel HallOfFameViewModel { get; set; }
        }
    }
}
