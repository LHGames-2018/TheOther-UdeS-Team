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
        private WorldMap map;
        private IPlayer player;
        private IAStar astarService;

        internal RessourcePlaner(WorldMap map, IPlayer player, IAStar astarService)
        {
            this.map = map;
            this.player = player;
            this.astarService = astarService;
        }

        public ResourceTileDescriptor GetBestRessourcePath()
        {
            return GetRessourcesPaths().Min();
        }

        public List<Tile> GetRessourcesTiles() {
            IEnumerable<Tile> mapTiles = map.GetVisibleTiles();
            List<Tile> ressourcesTiles = new List<Tile>();

            mapTiles.ToList().ForEach(i =>
            {
                if (i.TileType == TileContent.Resource)
                {
                    ressourcesTiles.Add(i);
                }
            });

            return ressourcesTiles;
        }

        public List<ResourceTileDescriptor> GetRessourcesPaths()
        {
            List <Tile> ressourceTiles = GetRessourcesTiles();
            Tile currentTile = map.GetTile(player.Position.X, player.Position.Y);
            List <ResourceTileDescriptor> resourceTileDescriptors = new List<ResourceTileDescriptor>();
            ressourceTiles.ToList().ForEach(ressourceTile => 
            {
                List<Tile> calculatedPath = astarService.Run(currentTile, ressourceTile);
                if (calculatedPath != null && calculatedPath.Count != 0)
                {
                    ResourceTileDescriptor res = new ResourceTileDescriptor
                    {
                        Tile = ressourceTile,
                        Path = calculatedPath
                    };
                    resourceTileDescriptors.Add(res);
                }
            });

            return resourceTileDescriptors;
        }
    }
}
