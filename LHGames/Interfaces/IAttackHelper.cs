using LHGames.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LHGames.Interfaces
{
    interface IAttackHelper
    {
        int HowManyDamageToTrees();

        int HowManyTurnsToKillTree(Tile treeTile);

        int HowManyDamageToEnemy();

        int HowManyTurnsToKillEnemy(IPlayer player);
    }
}
