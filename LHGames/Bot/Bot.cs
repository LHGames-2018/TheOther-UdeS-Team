using System;
using System.Collections.Generic;
using LHGames.Helper;
using LHGames.Interfaces;

namespace LHGames.Bot
{
    internal class Bot
    {
        internal IPlayer PlayerInfo { get; set; }
        private int _currentDirection = 1;

        IRessourcePlaner ressourcePlaner;
        IAStar astarService;

        internal Bot()
        {
        }

        /// <summary>
        /// Gets called before ExecuteTurn. This is where you get your bot's state.
        /// </summary>
        /// <param name="playerInfo">Your bot's current state.</param>
        internal void BeforeTurn(IPlayer playerInfo)
        {
            PlayerInfo = playerInfo;
        }

        /// <summary>
        /// Implement your bot here.
        /// </summary>
        /// <param name="map">The gamemap.</param>
        /// <param name="visiblePlayers">Players that are visible to your bot.</param>
        /// <returns>The action you wish to execute.</returns>
        internal string ExecuteTurn(Map map, IEnumerable<IPlayer> visiblePlayers)
        {
            this.astarService = new AStarAlgo(map);
            this.ressourcePlaner = new RessourcePlaner(map, PlayerInfo, astarService);


            if (PlayerInfo.CarriedResources < PlayerInfo.CarryingCapacity)
            {
                var best_ressource = ressourcePlaner.GetBestRessourcePath();
                if (best_ressource.Path.Count == 2)
                {
                    // On est adjacent à la meilleure ressource
                    var direction = GetDirectionToTile(best_ressource.Tile);
                    return AIHelper.CreateCollectAction(direction);
                }
                else
                {
                    // On est pas rendu
                    var direction = GetDirectionToTile(best_ressource.Path[1]);
                    return AIHelper.CreateMoveAction(direction);
                }
            }
            else
            {
                // on doit aller à la base
                var home_tile = map.GetTile(PlayerInfo.HouseLocation.X, PlayerInfo.HouseLocation.Y);
                var current_tile = map.GetTile(PlayerInfo.Position.X, PlayerInfo.Position.Y);
                var best_path_to_home = astarService.Run(current_tile, home_tile);
                
                // On est pas rendu
                var direction = GetDirectionToTile(best_path_to_home[1]);
                return AIHelper.CreateMoveAction(direction);
            }

            /*
             *             AStarAlgo astar = new AStarAlgo(map);
            var result = astar.Run(PlayerInfo.Position, new Point(-4, 21));
            

            var data = StorageHelper.Read<TestClass>("Test");
            Console.WriteLine(data?.Test);
            //return AIHelper.CreateMoveAction(new Point(_currentDirection, 0)); astar.DirectionToward(PlayerInfo.Position, result[0].Position);
            return AIHelper.CreateMoveAction(astar.DirectionToward(PlayerInfo.Position, result[0].Position));*/
        }

        /// <summary>
        /// Gets called after ExecuteTurn.
        /// </summary>
        internal void AfterTurn()
        {
        }

        public Point GetDirectionToTile(Tile tile)
        {
            var x_diff = tile.Position.X - PlayerInfo.Position.X;
            var y_diff = tile.Position.Y - PlayerInfo.Position.Y;

            // TODO maybe make sure this is 1 or -1 every time and no diagonal
            return new Point(x_diff, y_diff);
        }
    }
}

class TestClass
{
    public string Test { get; set; }
}
 