using LHGames.Helper;
using LHGames.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LHGames.Bot
{
    internal class PlacePlaner
    {
        private Map map;
        private IPlayer player;
        private IAStar astarService;

        internal PlacePlaner(Map map, IPlayer player, IAStar astarService)
        {
            this.map = map;
            this.player = player;
            this.astarService = astarService;
        }

        public PlaceTileDescriptor GetBestPlacePath(TileContent tileContent)
        {
            return GetPlacesPaths(tileContent).Min();
        }

        public List<Tile> GetPlacesTiles(TileContent tileContent)
        {
            IEnumerable<Tile> mapTiles = map.GetVisibleTiles();
            List<Tile> ressourcesTiles = new List<Tile>();

            mapTiles.ToList().ForEach(i =>
            {
                if (i.TileType == tileContent)
                {
                    ressourcesTiles.Add(i);
                }
            });

            return ressourcesTiles;
        }

        public List<PlaceTileDescriptor> GetPlacesPaths(TileContent tileContent)
        {
            List<Tile> ressourceTiles = GetPlacesTiles(tileContent);
            Tile currentTile = map.GetTile(player.Position.X, player.Position.Y);
            List<PlaceTileDescriptor> placeTileDescriptors = new List<PlaceTileDescriptor>();
            ressourceTiles.ToList().ForEach(ressourceTile =>
            {
                List<Tile> calculatedPath = astarService.Run(currentTile, ressourceTile);
                if (calculatedPath != null && calculatedPath.Count != 0)
                {
                    PlaceTileDescriptor res = new PlaceTileDescriptor
                    {
                        Tile = ressourceTile,
                        Path = calculatedPath
                    };
                    placeTileDescriptors.Add(res);
                }
            });

            return placeTileDescriptors;
        }
    }
}
