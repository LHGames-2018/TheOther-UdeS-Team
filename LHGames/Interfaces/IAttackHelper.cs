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

        int HowManyDamageToEnemy(IPlayer ennemy);

        int HowManyDamageFromEnemy(IPlayer ennemy);

        int HowManyTurnsToKillEnemy(IPlayer player);
    }
}
