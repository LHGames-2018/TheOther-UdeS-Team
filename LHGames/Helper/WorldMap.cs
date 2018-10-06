using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LHGames.Helper
{
    internal class WorldMap
    {
        public Tile[,] Tiles { get; set; }
        public int XMin { get; set; }
        public int YMin { get; set; }
        public int XMax { get; set; }
        public int YMax { get; set; }

        /// <summary>
        /// If you can break walls (trees)
        /// </summary>
        public bool WallsAreBreakable { get; set; }

        internal WorldMap()
        {
            Tiles = null;
            XMin = Int32.MaxValue;
            XMax = -Int32.MaxValue;
            YMin = Int32.MaxValue;
            YMax = -Int32.MaxValue;
        }

        internal void UpdateWorldMap(Map currentVision)
        {
            bool needToExpand = false;
            int newXMin = XMin;
            if (XMin > currentVision.XMin)
            {
                // we have a new min
                needToExpand = true;
                newXMin = currentVision.XMin;
            }
            int newXMax = XMax + 1;
            if (newXMax < currentVision.XMax + 1)
            {
                needToExpand = true;
                newXMax = currentVision.XMax + 1;
            }
            int newYMin = YMin;
            if (YMin > currentVision.YMin)
            {
                newYMin = currentVision.YMin;
                needToExpand = true;
            }
            int newYMax = YMax + 1;
            if (newYMax < currentVision.YMax + 1)
            {
                needToExpand = true;
                newYMax = currentVision.YMax + 1;
            }
            if (needToExpand)
            {
                Tile[,] newTiles = new Tile[newXMax - newXMin, newYMax - newYMin];
                for (int i = newXMin; i < newXMax; ++i)
                {
                    for (int j = newYMin; j < newYMax; ++j)
                    {
                        if (currentVision.GetTile(i, j) != null)
                        {
                            newTiles[i - newXMin, j - newYMin] = currentVision.GetTile(i, j);
                        }
                        else if (Tiles != null && i > XMin && i < XMax && j > YMin && j < YMax)
                        {
                            newTiles[i - newXMin, j - newYMin] = Tiles[i - XMin, j - YMin];
                        }
                        else
                        {
                            newTiles[i - newXMin, j - newYMin] = new Tile() { Position = new Point(i, j), TileType = TileContent.Empty };
                        }
                    }
                }

                Tiles = newTiles;
                XMin = newXMin;
                XMax = newXMax - 1;
                YMin = newYMin;
                YMax = newYMax - 1;
            }
            else
            {
                for (int i = newXMin; i < newXMax; ++i)
                {
                    for (int j = newYMin; j < newYMax; ++j)
                    {
                        if (currentVision.GetTile(i, j) != null)
                        {
                            Tiles[i - newXMin, j - newYMin] = currentVision.GetTile(i, j);
                        }
                    }
                }
            }


            WallsAreBreakable = currentVision.WallsAreBreakable;

            InitMapSize();
        }

        public static WorldMap ReadMap()
        {
            return StorageHelper.Read<WorldMap>("worldMap");
        }

        public static void WriteMap(WorldMap mapToWrite)
        {
            StorageHelper.Write("worldMap", mapToWrite);
        }

        /// <summary>
        /// Returns the TileType at this location. If you try to look outside 
        /// of your visible region, it will always return TileType.Tile (Empty
        /// tile).
        /// 
        /// Negative values are valid since the map wraps around when you reach
        /// the end.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>The content of the tile.</returns>
        internal TileContent GetTileAt(int x, int y)
        {
            if (x < XMin || x > XMax || y < YMin || y > YMax)
            {
                return TileContent.Empty;
            }
            return Tiles[x - XMin, y - YMin].TileType;
        }

        internal Tile GetTile(int x, int y)
        {
            if (x < XMin || x > XMax || y < YMin || y > YMax)
            {
                return null;
            }
            return Tiles[x - XMin, y - YMin];
        }

        /// <summary>
        /// Returns an IEnumerable of all tiles that are visible to your bot.
        /// </summary>
        /// <returns>All visible tiles.</returns>
        internal IEnumerable<Tile> GetVisibleTiles()
        {
            return Tiles.Cast<Tile>();
        }

        /// <summary>
        /// Deserialize the map received from the game server. 
        /// DO NOT MODIFY THIS.
        /// </summary>
        /// <param name="customSerializedMap">The received map.</param>
        private void DeserializeMap(string customSerializedMap)
        {
            customSerializedMap = customSerializedMap.Substring(1, customSerializedMap.Length - 2);
            var rows = customSerializedMap.Split('[');
            var column = rows[1].Split('{');
            Tiles = new Tile[rows.Length - 1, column.Length - 1];
            for (int i = 0; i < rows.Length - 1; i++)
            {
                column = rows[i + 1].Split('{');
                for (int j = 0; j < column.Length - 1; j++)
                {
                    var tileType = (byte)TileContent.Empty;
                    if (column[j + 1][0] != '}')
                    {
                        var infos = column[j + 1].Split('}');
                        infos = infos[0].Split(',');
                        if (infos.Length > 1)
                        {
                            tileType = byte.Parse(infos[0]);
                            var amountLeft = int.Parse(infos[1]);
                            CultureInfo culture = new CultureInfo("en");
                            var density = double.Parse(infos[2], culture);
                            Tiles[i, j] = new ResourceTile(tileType, i + XMin, j + YMin, amountLeft, density);
                        }
                        else
                        {
                            tileType = byte.Parse(infos[0]);
                        }
                    }
                    if (tileType != (byte)TileContent.Resource)
                    {
                        Tiles[i, j] = new Tile(tileType, i + XMin, j + YMin);
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the XMax, YMax and VisibleDistance.
        /// </summary>
        private void InitMapSize()
        {
            //if (Tiles == null)
            //{
            //    throw new InvalidOperationException("Tiles cannot be null.");
            //}

            //XMax = XMin + Tiles.GetLength(0) - 1;
            //YMax = YMin + Tiles.GetLength(1) - 1;
        }
    }
}
