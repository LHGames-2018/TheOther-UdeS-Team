using LHGames.Helper;
using LHGames.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LHGames.Bot
{
    internal class RessourcePlaner : IRessourcePlaner
    {
        private Map map;
        private IPlayer player;
        private IAStar astarService;

        internal RessourcePlaner(Map map, IPlayer player, IAStar astarService)
        {
            this.map = map;
            this.player = player;
            this.astarService = astarService;
        }

        public ResourceTileDescriptor GetBestRessourcePath()
        {
            throw new NotImplementedException();
        }

        public List<ResourceTileDescriptor> GetRessourcesPaths()
        {
            throw new NotImplementedException();
        }
    }
}
