using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wordament.Models;

namespace Wordament.Helpers
{
    public class GameHelper
    {
        public static Models.Game LoadGame(int gameGroupId, string level)
        {
            if (gameGroupId < 0)
                return null;
            var dbContext = new Entities();
           
           // var g = dbContext.Games.Where(ga => ga.Name.Equals(gameName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            var gameGroup = dbContext.GameGroups.Where(g => g.GameGroupId == gameGroupId).FirstOrDefault();
            Models.Game game = null;
            //if (g != null)
            //{
               // var gameGroup = g.GameGroups.Where(gg => gg.GameId == g.GameId && gg.Level.Equals(level, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                if (gameGroup != null)
                {
                    var gameVariants = dbContext.GameVariants.Where(gv => gv.GameGroupId == gameGroup.GameGroupId);
                    game = new Models.Game(default(int));
                    game.Words = new List<Models.Word>();

                    foreach (var item in gameVariants)
                    {
                        game.Words.Add(new Models.Word
                        {
                            Letters = item.Word.Word1,
                            Position = new Position2D
                            {
                                StartWord = new Coordinate { X = item.X1, Y = item.Y1 },
                                EndWord = new Coordinate { X = item.X2, Y = item.Y2 }
                            }
                        });
                    }
                }
           // }
            return game;
        }
    }
}