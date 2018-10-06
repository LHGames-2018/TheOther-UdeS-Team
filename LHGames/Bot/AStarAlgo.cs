using LHGames.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LHGames.Bot
{
    public class AStarAlgo
    {   
        public class ATuple
        {
            public Tile tile;
            public int heuristique_remaining;
            public int real_cost_to_get_here;
            public ATuple parent;

            public int f()
            {
                return real_cost_to_get_here + heuristique_remaining;
            }
        }

        public Tile[,] allTiles;
        public int MapSizeX = 10;
        public int MapSizeY = 10;

        public List<Tile> ConstructPath(ATuple final)
        {
            List<Tile> tiles = new List<Tile>();
            ATuple current = final;
            while (current.parent != null)
            {
                tiles.Add(current.tile);
                current = current.parent;
            }
            tiles.Add(current.tile);
            return tiles;
        }

        public List<Tile> Run(Tile start, Tile end)
        {
            List<ATuple> open = new List<ATuple>();
            open.Add(new ATuple()
            {
                tile = start,
                parent = null,
                heuristique_remaining = heuristique(start.Position, end.Position),
                real_cost_to_get_here = 0
            });
            //List<ATuple> closed = new List<ATuple>();

            while (true)
            {
                open.Sort((atuple1, atuple2) => atuple1.f().CompareTo(atuple2.f()));
                if (open.Count == 0)
                {
                    // TODO
                    throw new Exception();
                }

                var current_tuple = open[0];
                open.Remove(current_tuple);

                if (current_tuple.tile.Position.Equals(end.Position))
                {
                    return ConstructPath(current_tuple);
                }

                foreach (var neighbor in GetNeighbors(current_tuple.tile))
                {
                    ATuple next = new ATuple()
                    {
                        tile = neighbor,
                        parent = current_tuple,
                        real_cost_to_get_here = current_tuple.real_cost_to_get_here + 1,
                        heuristique_remaining = heuristique(neighbor.Position, end.Position)
                    };

                    var neighbor_value_in_open = open.Find(atuple => atuple.tile.Position.Equals(neighbor.Position));
                    if (neighbor_value_in_open == null)
                    {
                        open.Add(next);
                    }
                    else if (neighbor_value_in_open.f() > next.f())
                    {
                        open.Remove(neighbor_value_in_open);
                        open.Add(next);
                    }
                }
            }
        }

        public int GetCostToWalkUponTile(Tile tile)
        {
            switch (tile.TileType)
            {
                case TileContent.Empty:
                case TileContent.House:
                    return 1;
                case TileContent.Wall:
                    return 1 + CalculateCostOfCuttingTree();
                default:
                    // TODO account for lava
                    return 10000;
            }
        }

        public int CalculateCostOfCuttingTree()
        {
            // TODO Calculate cost of cutting tree, depending on its health
            return 2;
        }

        public int heuristique(Point point1, Point point2)
        {
            return 1;
        }

        public Tile GetTileByPosition(int x, int y)
        {
            return allTiles[x, y];
        }

        public List<Tile> GetNeighbors(Tile current)
        {
            List<Tile> neighbors = new List<Tile>();

            var rightTile = GetTileByPosition((current.Position.X + 1 + MapSizeX) % MapSizeX, current.Position.Y);
            var leftTile = GetTileByPosition((current.Position.X - 1 + MapSizeX) % MapSizeX, current.Position.Y);
            var upTile = GetTileByPosition(current.Position.X, (current.Position.Y - 1 + MapSizeY) % MapSizeY);
            var downTile = GetTileByPosition(current.Position.X, (current.Position.Y + 1 + MapSizeY) % MapSizeY);

            if (IsTileWalkable(rightTile.TileType))
            {
                neighbors.Add(rightTile);
            }
            if (IsTileWalkable(leftTile.TileType))
            {
                neighbors.Add(leftTile);
            }
            if (IsTileWalkable(upTile.TileType))
            {
                neighbors.Add(upTile);
            }
            if (IsTileWalkable(downTile.TileType))
            {
                neighbors.Add(downTile);
            }

            return neighbors;
        }

        public bool IsTileWalkable(TileContent tileContent)
        {
            // TODO handle lava
            return tileContent == TileContent.Empty || tileContent == TileContent.House;
        }
    }
}
