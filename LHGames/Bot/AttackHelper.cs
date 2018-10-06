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

        public int HowManyDamageToEnemy(IPlayer ennemy)
        {
            //Attack function : Floor(3 + attacker's power + offensive items - 2 * (defender's defence + defensive items)^0.6 )
            bool hasSword = player.CarriedItems.Any(x => x == PurchasableItem.Sword);
            bool ennemyHasShield = ennemy.CarriedItems.Any(x => x == PurchasableItem.Shield);

            int swordValue = hasSword ? 2 : 0;
            int shieldValue = ennemyHasShield ? 2 : 0;

            return (int) Math.Floor(3 + player.AttackPower + swordValue - (2 * Math.Pow(ennemy.Defence + shieldValue, 0.6)));
        }

        public int HowManyDamageFromEnemy(IPlayer ennemy)
        {
            //Attack function : Floor(3 + attacker's power + offensive items - 2 * (defender's defence + defensive items)^0.6 )
            bool ennemyHasSword = ennemy.CarriedItems.Any(x => x == PurchasableItem.Sword);
            bool hasShield = player.CarriedItems.Any(x => x == PurchasableItem.Shield);

            int swordValue = ennemyHasSword ? 2 : 0;
            int shieldValue = hasShield ? 2 : 0;

            return (int)Math.Floor(3 + ennemy.AttackPower + swordValue - (2 * Math.Pow(player.Defence + shieldValue, 0.6)));
        }

        public int HowManyDamageToTrees()
        {
            bool hasSword = player.CarriedItems.Any(x => x == PurchasableItem.Sword);
            int swordValue = hasSword ? 2 : 0;
            return 3 + player.AttackPower + swordValue;
        }

        public int HowManyTurnsToKillEnemy(IPlayer ennemy)
        {
            int damagePerTurn = HowManyDamageToEnemy(ennemy);
            return (ennemy.Health + damagePerTurn) / damagePerTurn;
        }

        public int HowManyTurnsToKillTree(Tile treeTile)
        {
            return 1; 
        }
    }
}
