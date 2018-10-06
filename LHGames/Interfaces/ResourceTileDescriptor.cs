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
            return (resourceTile.AmountLeft * Path.Count).CompareTo(otherResourceTile.AmountLeft * other.Path.Count);
        }
    }
}
