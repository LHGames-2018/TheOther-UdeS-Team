using LHGames.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LHGames.Interfaces
{
    interface INavigationHelper
    {
        string NavigateToNextPosition(Tile adjacentTile);

        int HowManyTurnsToFollowPath(List<Tile> path);
    }
}
