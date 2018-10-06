using LHGames.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LHGames.Interfaces
{
    public interface IManathan
    {
        List<Point> GetManathanPath(Point start, Point end);
    }
}
