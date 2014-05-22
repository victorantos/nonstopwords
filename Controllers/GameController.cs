using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Wordament.Helpers;
using Wordament.Models;

namespace Wordament.Controllers
{
    
    public class GameController : Controller
    {
        //
        // GET: /Game/
        public ActionResult Data(int id, string language, int? level, bool? isDaily)
        {
            var dbContext = new Entities();
             
            IQueryable<GameGroup> res = null;
            int gameGroupId = -1;
            //string path = VirtualPathUtility.ToAbsolute("~/test/debug.txt");
            //System.IO.File.WriteAllText(path, widget == null ? "widget is null YES" : "NOt null");



            //we could have multiple puzzles for Today, so check the level and get the corespondent puzzle from Today
            if (isDaily.HasValue && isDaily.Value)
            {
                //ignore id and get today's puzzle
                var maxLimit = DateTime.UtcNow.AddDays(1);
                var firstSunday = new DateTime(1753, 1, 7);
                int x = (int)DateTime.UtcNow.DayOfWeek;
                
                var todayPuzzle = dbContext.DailyPuzzles.Where(p => p.DateWhenActive.HasValue && (EntityFunctions.DiffDays(firstSunday, p.DateWhenActive.Value) % 7 == x));
                DateTime now = DateTime.UtcNow.Date;
               
                if (todayPuzzle.Count() > 0)
                    res =
                        from p in todayPuzzle
                        join gg in dbContext.GameGroups on p.GameGroupId equals gg.GameGroupId
                        into tempGames
                        from puzzle in tempGames.DefaultIfEmpty()
                        //TODO uncomment this
                       // where EntityFunctions.DiffDays(p.DateWhenActive.Value,now) == 0
                        select puzzle;
                else
                {
                    //get some puzzle any one
                    if (!level.HasValue || level.Value == 0)
                        level = 1;

                    string levelStr = level.ToString();
                    res = from p in dbContext.GameGroups
                          where p.Level.Equals(levelStr) && p.Language == language
                          select p;
                }
            }
            else
            {
                //not sure about this, this case is a just to make sure we have a puzzle shown
                if (!level.HasValue || level.Value == 0)
                {
                    //load widget
                    res = from p in dbContext.GameGroups
                          where p.GameGroupId == id
                          select p;
                }
                else
                {
                    level = 1;
                    string levelStr = level.ToString();
                    res = from p in dbContext.GameGroups
                          where p.Level.Equals(levelStr) && p.Language == language
                          select p;
                }
            }

            if (res != null && res.FirstOrDefault() != null)
            {
                GameGroup gg = null;
                if (level.HasValue)
                {
                    string levelStr = level.ToString();
                    gg = res.Where(g => g.Level.Equals(levelStr)).FirstOrDefault();
                }
                if (gg == null)
                    gg = res.FirstOrDefault();
                gameGroupId = gg.GameGroupId;
            }
            var generatedLines = LoadGame(gameGroupId);

            try
            {
                dbContext.Entry(new GameHistory { GameGroupId = gameGroupId, DateStarted = DateTime.UtcNow, IpAddress = Request.UserHostAddress }).State = System.Data.Entity.EntityState.Added;
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                
              
            }

            return new JsonpResult
            {
                Data = new { gameId = gameGroupId, letters = generatedLines, nrLevels = res.Count() },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            
        }


        public JsonResult GetWords(int id)
        {
            var game = GameHelper.LoadGame(id, "Easy 5x5");

            return new JsonpResult
            {
                //TODO
                //Data = new { gameId = id, words = game.Words.Select(l => l.Letters) },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
 
        private List<GameLine> LoadGame(int gameGroupId)
        {
            if (gameGroupId < 0)
                return null;
            var lines = new List<GameLine>();
            var game = GameHelper.LoadGame(gameGroupId, "Easy 5x5");
               
            Tuple<int,int> max = GetMaxXandY(game.GameWords.Select(w=>w.Word).ToList());
            int nrLines = max.Item1;
            if (max.Item2 > max.Item1)
                nrLines = max.Item2;
            for (int i = 0; i < nrLines; i++)
            {
                var newLine = new GameLine();
                newLine.num = i+1;
                newLine.cells = new List<string>();
                for (int j = 0; j < nrLines; j++)
                {
                    newLine.cells.Add(RandLetter());
                }
                lines.Add(newLine);
            }
            foreach (var item in game.GameWords.Select(w => w.Word))
            {

                var x1 = item.GameVariants.First().X1;
                var y1 = item.GameVariants.First().Y1;
                var x2 = item.GameVariants.First().X2;
                var y2 = item.GameVariants.First().Y2;
 
                if (y1 == y2)
                {
                    if (x1 < x2)
                    {
                        for (short k = 0, j = x1; j <= x2; j++, k++)
                            SetValue(lines[y1 - 1], j, item.Word1[k].ToString().ToUpper());
                    }
                    else
                    {
                        for (short k = (short)(x1 - x2), j = x2; j <= x1; j++, k--)
                            SetValue(lines[y1 - 1], j, item.Word1[k].ToString().ToUpper());
                    }
                }
                else
                {
                    if (x1 == x2)
                    {
                        if (y1 < y2)
                        {
                            for (short k = 0, j = y1; j <= y2; j++, k++)
                                SetValue(lines[j - 1], x1, item.Word1[k].ToString().ToUpper());
                        }
                        else
                        {
                            for (short k = (short)(y1-y2), j = y2; j <= y1; j++, k--)
                                SetValue(lines[j-1], x1, item.Word1[k].ToString().ToUpper());
                        }
                    }
                    else
                    { 
                        //is rtl
                        if (x1 > x2)
                        {
                            if (y1 > y2)
                            {
                                for (short k = 0, j = y1; j >= y2; j--, k++)
                                    SetValue(lines[j-1], (short)(x1 - k), item.Word1[k].ToString().ToUpper());
                            }
                            else
                            {
                                for (short k = 0, j = y1; j <= y2; j++, k++)
                                    SetValue(lines[j - 1], (short)(x1 - k), item.Word1[k].ToString().ToUpper());
                            }
                        }
                            //is ltr
                        else
                        { 
                            if (y1 > y2)
                            {
                                for (short k = 0, j = y1; j >= y2; j--, k++)
                                    SetValue(lines[j - 1], (short)(x1 + k), item.Word1[k].ToString().ToUpper());
                            }
                            else
                            {
                                for (short k = 0, j = y1; j <= y2; j++, k++)
                                    SetValue(lines[j - 1], (short)(x1 + k), item.Word1[k].ToString().ToUpper());
                            }
                        }
                    }
                }
            }
          return lines;
        }

        private Tuple<int, int> GetMaxXandY(List<Models.Word> list)
        {
            int maxX = 0, maxY = 0;
            foreach (var item in list)
            {
                if (item.GameVariants.First().X1 >= item.GameVariants.First().X2 && item.GameVariants.First().X1 > maxX)
                    maxX = item.GameVariants.First().X1;
                else
                    if (item.GameVariants.First().X2 >= item.GameVariants.First().X1 && item.GameVariants.First().X2 > maxX)
                        maxX = item.GameVariants.First().X2;

                if (item.GameVariants.First().Y1 >= item.GameVariants.First().Y2 && item.GameVariants.First().Y1 > maxY)
                    maxY = item.GameVariants.First().Y1;
                else
                    if (item.GameVariants.First().Y2 >= item.GameVariants.First().Y1 && item.GameVariants.First().Y2 > maxY)
                        maxY = item.GameVariants.First().Y2;
            }

            Tuple<int, int> max = Tuple.Create<int, int>(maxX, maxY);
            return max;
        }
 
        private void SetValue(GameLine gameLine, short x, string letter)
        {
            gameLine.cells[x - 1] = letter;
           
        }

        readonly string[] Alphabet = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        Random r = new Random();
        private string RandLetter()
        {
            return Alphabet[r.Next(26)];

        }

        [System.Web.Mvc.HttpGet]
        public JsonResult GameOver(int id, string name, int s, string userId)
        {
            if (userId != "6899E741-2650-4154-8E85-23960C08AD87")
            {
                return new JsonpResult
                {
                    Data = new { result = "failed" },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            var dbContext = new Entities();
            GameHistory f = new GameHistory
            {
                GameGroupId = id,
                DateFinished = DateTime.UtcNow,
                IpAddress = Request.UserHostAddress,
                PlayerName = name == "undefined" ? null: name.Trim(),
                Score = s

            };
            dbContext.Entry(f).State = System.Data.Entity.EntityState.Added;
            dbContext.SaveChanges();

            return new JsonpResult
            {
                Data = new { result = "success" },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GameOver(GameOverModel gameOver)
        {
           
            try
            {

           
            var dbContext = new Entities();
            GameHistory f = new GameHistory
            {
                GameGroupId = gameOver.Id,
                DateFinished = DateTime.UtcNow,
                IpAddress = Request.UserHostAddress,
                PlayerName = gameOver.PlayerName,
                Score = gameOver.Score

            };
            dbContext.Entry(f).State =  System.Data.Entity.EntityState.Added;
            dbContext.SaveChanges();

            return new JsonpResult
            {
                Data = new { result = "success" },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            }
            catch (Exception ex)
            {

             
                return new JsonpResult
                {
                    Data = new { result = ex.InnerException.Message },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
           
        }
        public JsonResult IsCorrectWord(string x1, string y1, string x2, string y2, string gameId)
        {
            var game = GameHelper.LoadGame(int.Parse(gameId), "Easy 5x5");
            var isWord = false;
            var isReverse = false;
            //if (game != null)
            //{
            //    if (game.GameWords.Select(w => w.Word).ToList().Exists(w => w.GameVariants.StartWord.X.ToString() == x1 && w.Word.StartWord.Y.ToString() == y1 &&
            //        w.Position.EndWord.X.ToString() == x2 && w.Position.EndWord.Y.ToString() == y2))
            //    {
            //        isWord = true;

            //        return new JsonpResult
            //        {
            //            Data = new { result = isWord },
            //            JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //        };
 
            //    }
            //    else
            //        if (game.GameWords.Select(w => w.Word).Exists(w => w.Position.EndWord.X.ToString() == x1 && w.Position.EndWord.Y.ToString() == y1 &&
            //            w.Position.StartWord.X.ToString() == x2 && w.Position.StartWord.Y.ToString() == y2))
            //        {
            //            isWord = true;
            //            isReverse = true;
 
            //            return new JsonpResult
            //            {
            //                Data = new { result = isWord, r = isReverse },
            //                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //            };
            //        }
            //}
            return new JsonpResult
            {
                Data = new { result = isWord  },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult IsInline(string x1, string y1, string x2, string y2, string gameId)
        {
            var X1 = decimal.Parse(x1);
            var X2 = decimal.Parse(x2);
            var Y1 = decimal.Parse(y1);
            var Y2 = decimal.Parse(y2);

            return new JsonpResult
            {
                Data = new { result = Math.Abs(X1 - X2) == Math.Abs(Y1 - Y2) || X1 == X2 || Y1 == Y2 },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
         
     
        public ActionResult Index()
        {
            var model = new GameViewModel
            {
                GameId = 272,
                GameGroupId = Guid.NewGuid(),
                Language = "en"
            };
            return View(model);
        }


        public JsonResult TopUsers(int id)
        {
            List<TopUser> users = new List<TopUser>();
            var dbContext = new Entities();
            var games = dbContext.GameHistories.Where(h => h.GameGroupId == id && h.Score.HasValue).OrderBy(h => h.Score).Take(15).ToList();
         
            foreach (var item in games)
            {
                if (item.Score.HasValue)
                    users.Add(new TopUser { Name =  item.PlayerName ?? "Anonymous" , Time = item.Score.Value });
            }


            return new JsonpResult
            {
                Data = new { gameGroupId = id.ToString(), topUsers = users },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            
        }
         
        public ActionResult GetWordsearchHTML(int id)
        {
            //string path = VirtualPathUtility.ToAbsolute("~/test/debug.txt");
            //System.IO.File.WriteAllText(path, id.ToString());

            var dbContext = new Entities();
            bool isDaily = false;
            int? level = null;

            var widget = dbContext.Widgets.Where(w => w.WidgetId == id).FirstOrDefault();
            int gameGroupId = -1;
            if (widget != null)
            {
                gameGroupId = widget.GameGroupId;
            }
            else
            {
                level = 1;
                isDaily = true;
            }

            var game = new MyGameViewModel
            {

                DailyPuzzleViewModel = new GameViewModel
                {
                    GameId = gameGroupId,
                    Language = "en",
                    ShowLanguage = false,
                    ShowStopwatch = true,
                    IsDaily = isDaily,
                    Level = level ?? 0
                },
                HallOfFameViewModel = new HallOfFameViewModel { GameGroupId = gameGroupId }
                
            };
            return new JsonpResult
            {

                Data = new { html = this.RenderViewToString("View1", game) },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public JsonResult SavePuzzle(List<IEnumerable<Cell>> words)
        {
            var language = "en";
            int id = 1;
            var dbContext = new Entities();

            GameGroup game = new GameGroup();
            game.GameId = id;
            game.Language = language;
            dbContext.Entry(game).State = System.Data.Entity.EntityState.Added;
            dbContext.SaveChanges();

            string wordStr;
           
            foreach (var item in words)
            {
                var i = 0;
                var gv = new GameVariant();
                wordStr = string.Empty;
                foreach (var cell in item)
                {
                    wordStr += cell.letter;
                    if (i == 0)
                    {
                        gv.X1 = Convert.ToInt16(cell.x);
                        gv.Y1 = Convert.ToInt16(cell.y);
                    }
                    if (i == item.Count() - 1)
                    {
                        gv.X2 = Convert.ToInt16(cell.x);
                        gv.Y2 = Convert.ToInt16(cell.y);
                    }
                    i++;
                }
                if (!string.IsNullOrEmpty(wordStr))
                {
                    //add the word
                    var w = new Word();
                    w.Word1 = wordStr;
                    w.DateCreated = DateTime.UtcNow;
                    dbContext.Entry(w).State = System.Data.Entity.EntityState.Added;
                    dbContext.SaveChanges();

                    gv.WordId = w.WordId;
                
                    if (game.GameGroupId > 0)
                    {
                        gv.GameGroupId = game.GameGroupId;
                        dbContext.Entry(gv).State = System.Data.Entity.EntityState.Added;
                        dbContext.SaveChanges();
                    }
                }
            }
            int widgetId = -1;
            var title = "Created by {0}";
            var user = "victor";
            if (game.GameGroupId > 0)
            {
                Widget w = new Widget()
                {
                    IPAddress = Request.UserHostAddress,
                    Name = string.Format(title, user),
                    CreatedBy = user,
                    GameGroupId = game.GameGroupId,
                    DateCreated = DateTime.UtcNow
                };
                dbContext.Entry(w).State = System.Data.Entity.EntityState.Added;
                dbContext.SaveChanges();
                widgetId = w.WidgetId;
            }
            return Json(new { widgetId = widgetId });
        }

        public class MyGameViewModel
        {
            public GameViewModel DailyPuzzleViewModel { get; set; }
            public HallOfFameViewModel HallOfFameViewModel { get; set; }
        }
    
    }
}
