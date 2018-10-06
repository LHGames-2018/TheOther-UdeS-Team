using LHGames.Helper;
using LHGames.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LHGames.Bot
{
    public class BadRessourcePlaner : IRessourcePlaner
    {

        private Map map;
        private IPlayer player;
        private IAStar astarService;

        internal BadRessourcePlaner(Map map, IPlayer player, IAStar astarService)
        {
            this.map = map;
            this.player = player;
            this.astarService = astarService;
        }

        public ResourceTileDescriptor GetBestRessourcePath()
        {
            var home_tile = map.GetTile(player.HouseLocation.X, player.HouseLocation.Y);
            var current_tile = map.GetTile(player.Position.X, player.Position.Y);

            var ressourceTile = map.GetTile(home_tile.Position.X - 6, home_tile.Position.Y - 6);

            var best_path_to_ressource = astarService.Run(current_tile, ressourceTile);

            return new ResourceTileDescriptor()
            {
                Path = best_path_to_ressource,
                Tile = ressourceTile
            };
        }

        public List<ResourceTileDescriptor> GetRessourcesPaths()
        {
            throw new NotImplementedException();
        }
    }
}
