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
        IAStar astar;
        public NavigationHelper(IPlayer player, IAStar astar)
        {
            this.player = player;
            this.astar = astar;
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
            return astar.DirectionToward(player.Position, tile.Position);
        }
    }
}
