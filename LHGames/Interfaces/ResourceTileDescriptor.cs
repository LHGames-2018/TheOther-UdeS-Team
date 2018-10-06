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
        public int CompareTo(ResourceTileDescriptor other) { return Path.Count.CompareTo(other.Path.Count); }
    }
}
