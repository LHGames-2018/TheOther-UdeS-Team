using LHGames.Helper;
using LHGames.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LHGames.Bot
{
    public class AttackHelper : IAttackHelper
    {

        public AttackHelper(IPlayer player)
        {
            this.player = player;
        }

        public IPlayer player;

        public int HowManyDamageToEnemy()
        {
            throw new NotImplementedException();
        }

        public int HowManyDamageToTrees()
        {
            bool hasSword = player.CarriedItems.Any(x => x == PurchasableItem.Sword);
            int swordValue = hasSword ? 2 : 0;
            return 3 + player.AttackPower + swordValue;
        }

        public int HowManyTurnsToKillEnemy(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public int HowManyTurnsToKillTree(Tile treeTile)
        {
            return 1; 
        }
    }
}
