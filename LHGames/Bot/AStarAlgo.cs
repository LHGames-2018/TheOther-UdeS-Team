using LHGames.Helper;
using LHGames.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LHGames.Bot
{
    public class AStarAlgo : IAStar
    {
        internal AStarAlgo(Map map)
        {
            this.map = map;
        }

        public class ATuple
        {
            public Tile tile;
            public  double heuristique_remaining;
            public  double real_cost_to_get_here;
            public ATuple parent;

            public  double f()
            {
                return real_cost_to_get_here + heuristique_remaining;
            }
        }

        internal Map map;
        public int MapSizeX => map.XMax - map.XMin;
        public int MapSizeY => map.YMax - map.YMin;

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
            tiles.Reverse();
            return tiles;
        }

        public Point DirectionToward(Point start, Point end)
        {
            int difX = end.X - start.X;
            if (difX > 1)
            {
                difX = end.X + MapSizeX - start.X;
            }
            else if (difX < -1)
            {
                difX = end.X - MapSizeX - start.X;
            }

            int difY = end.Y - start.Y;
            if (difY > 1)
            {
                difY = end.Y + MapSizeY - start.Y;
            }
            else if (difY < -1)
            {
                difY = end.Y - MapSizeY - start.Y;
            }
            return new Point(difX, difY);
        }

        public List<Tile> Run(int startX, int startY, int endX, int endY)
        {
            return Run(map.GetTile(startX, startY), map.GetTile(endX, endY));
        }

        public List<Tile> Run(Point start, Point end)
        {
            return Run(start.X, start.Y, end.X, end.Y);
        }

        public List<Tile> Run(Tile start, Tile end)
        {
            List<ATuple> open = new List<ATuple>();
            Dictionary<Point, double> costSoFar = new Dictionary<Point, double>();
            open.Add(new ATuple()
            {
                tile = start,
                parent = null,
                heuristique_remaining = heuristique(start.Position, end.Position),
                real_cost_to_get_here = 0
            });

            while (true)
            {
                open.Sort((atuple1, atuple2) => atuple1.f().CompareTo(atuple2.f()));
                if (open.Count == 0)
                {
                    // TODO
                    return null;
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

                    //if (neighbor_value_in_open == null)
                    if (!costSoFar.ContainsKey(neighbor.Position))
                    {
                        open.Add(next);
                        costSoFar[neighbor.Position] = next.f();
                    }
                    else if (costSoFar[neighbor.Position] > next.f())
                    {
                        var neighbor_value_in_open = open.Find(atuple => atuple.tile.Position.Equals(neighbor.Position));
                        open.Remove(neighbor_value_in_open);
                        open.Add(next);
                    }
                }
            }
        }

        public double GetCostToWalkUponTile(Tile tile)
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

        public double heuristique(Point point1, Point point2)
        {
            //return 1;
            return Point.Distance(point1, point2);
        }

        public Tile GetTileByPosition(int x, int y)
        {
            return map.GetTile(x, y);
        }

        public List<Tile> GetNeighbors(Tile current)
        {
            List<Tile> neighbors = new List<Tile>();

            //var rightTile = GetTileByPosition((current.Position.X + 1 + MapSizeX) % MapSizeX, current.Position.Y);
            Tile rightTile = null;
            if (current.Position.X == map.XMax)
            {
                //rightTile = GetTileByPosition(map.XMin, current.Position.Y);
            }
            else
            {
                rightTile = GetTileByPosition(current.Position.X + 1, current.Position.Y);
            }

            Tile leftTile = null;
            if (current.Position.X == map.XMin)
            {
                //leftTile = GetTileByPosition(map.XMax - 1, current.Position.Y);
            }
            else
            {
                leftTile = GetTileByPosition(current.Position.X - 1, current.Position.Y);
            }

            //var upTile = GetTileByPosition(current.Position.X, (current.Position.Y - 1 + MapSizeY) % MapSizeY);

            Tile upTile = null;
            if (current.Position.Y == map.YMin)
            {
                //upTile = GetTileByPosition(current.Position.X, map.YMax);
            }
            else
            {
                upTile = GetTileByPosition(current.Position.X, current.Position.Y - 1);
            }
            //var downTile = GetTileByPosition(current.Position.X, (current.Position.Y + 1 + MapSizeY) % MapSizeY);
            Tile downTile = null;
            if (current.Position.Y == map.YMax)
            {
                //downTile = GetTileByPosition(current.Position.Y, map.YMin);
            }
            else
            {
                downTile = GetTileByPosition(current.Position.X, current.Position.Y + 1);
            }

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
