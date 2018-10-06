using LHGames.Helper;
using LHGames.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LHGames.Bot
{
    public class NavigationHelper : INavigationHelper
    {

        public NavigationHelper(IPlayer player)
        {
            this.player = player;
        }

        public IPlayer player;
        public int HowManyTurnsToFollowPath(List<Tile> path)
        {
            int acc = 0;
            path.Skip(1).Take(path.Count - 2).ToList().ForEach((tile) =>
             {
                 if (tile.TileType == TileContent.Wall)
                 {
                     acc += 2;
                 } else
                 {
                     acc += 1;
                 }
             });

            return acc;
        }

        public string NavigateToNextPosition(Tile adjacentTile)
        {
            if (adjacentTile.TileType == TileContent.Wall)
            {
                return AIHelper.CreateMeleeAttackAction(GetDirectionToTile(adjacentTile));
            } else
            {
                return AIHelper.CreateMoveAction(GetDirectionToTile(adjacentTile));
            }
        }

        public Point GetDirectionToTile(Tile tile)
        {
            var x_diff = tile.Position.X - player.Position.X;
            var y_diff = tile.Position.Y - player.Position.Y;

            // TODO maybe make sure this is 1 or -1 every time and no diagonal
            return new Point(x_diff, y_diff);
        }
    }
}
