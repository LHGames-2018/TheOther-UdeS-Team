using LHGames.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LHGames.Interfaces
{
    public interface IAStar
    {
        List<Tile> Run(Tile start, Tile end);
    }
}
