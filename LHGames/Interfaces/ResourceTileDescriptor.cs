using LHGames.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LHGames.Interfaces
{
    public class ResourceTileDescriptor : IComparable<ResourceTileDescriptor>
    {
        public Tile Tile;
        public List<Tile> Path;

        
        public int CompareTo(ResourceTileDescriptor other) {
            if (Tile is ResourceTile && other.Tile is ResourceTile)
            {
                ResourceTile resourceTile = (ResourceTile)Tile;
                ResourceTile otherResourceTile = (ResourceTile)other.Tile;
                double resourceScore = 1 / resourceTile.AmountLeft * 1 / resourceTile.Density * Path.Count;
                double otherResourceScore = 1 / otherResourceTile.AmountLeft * 1 / otherResourceTile.Density * other.Path.Count;

                return resourceScore.CompareTo(otherResourceScore);
            }
            return 0;
        }
    }
}
