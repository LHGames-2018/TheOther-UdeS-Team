using LHGames.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LHGames.Interfaces
{
    public class ResourceTileDescriptor
    {
        public Tile Tile;
        public List<Tile> Path;
        public int CompareTo(ResourceTileDescriptor other) {
            ResourceTile resourceTile = (ResourceTile)Tile;
            ResourceTile otherResourceTile = (ResourceTile)other.Tile;
            double resourceScore = resourceTile.AmountLeft * resourceTile.Density * Path.Count;
            double otherResourceScore = otherResourceTile.AmountLeft * otherResourceTile.Density * other.Path.Count;

            return resourceScore.CompareTo(otherResourceScore);
        }
    }
}
