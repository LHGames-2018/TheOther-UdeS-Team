using LHGames.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LHGames.Interfaces
{
    public interface IAStar
    {
        List<Tile> Run(int startX, int startY, int endX, int endY);

        List<Tile> Run(Point start, Point end);
        List<Tile> Run(Tile start, Tile end);


        Point DirectionToward(Point start, Point end);
    }
}
