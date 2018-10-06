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
        IPlacePlaner placePlaner;
        IAStar astarService;
        INavigationHelper navigationHelper;
        WorldMap worldMap;
        IManathan manathan;

        internal Bot()
        {
            worldMap = new WorldMap();
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
            worldMap = WorldMap.ReadMap();
            if (worldMap == null || worldMap.HomePosition != PlayerInfo.HouseLocation)
            {
                worldMap = new WorldMap();
            }
            worldMap.UpdateWorldMap(map);
            worldMap.HomePosition = PlayerInfo.HouseLocation;
            WorldMap.WriteMap(worldMap);
            this.astarService = new AStarAlgo(worldMap);
            this.ressourcePlaner = new RessourcePlaner(worldMap, PlayerInfo, astarService);
            this.navigationHelper = new NavigationHelper(PlayerInfo, astarService);
            this.manathan = new Manathan();
            this.placePlaner = new PlacePlaner(map, PlayerInfo, astarService);

            try
            {
                var best_ressource = ressourcePlaner.GetBestRessourcePath();
                //var best_place_for_shop = placePlaner.GetBestPlacePath(TileContent.Shop);

                if (PlayerInfo.CarriedResources < PlayerInfo.CarryingCapacity && best_ressource != null)
                {
                    if (best_ressource.Path.Count == 2)
                    {
                        // On est adjacent à la meilleure ressource
                        var direction = astarService.DirectionToward(PlayerInfo.Position, best_ressource.Tile.Position);
                        return AIHelper.CreateCollectAction(direction);
                    }
                    else if (best_ressource.Path.Count == 1)
                    {
                        // on est dessus
                        var tileToGo = map.GetTile(PlayerInfo.Position.X - 1, PlayerInfo.Position.Y);
                        var action = navigationHelper.NavigateToNextPosition(tileToGo);
                        return action;
                    }
                    else
                    {
                        // On est pas rendu
                        return navigationHelper.NavigateToNextPosition(best_ressource.Path[1]);
                    }
                }
                else
                {
                    // on doit aller à la base
                    var home_tile = worldMap.GetTile(PlayerInfo.HouseLocation.X, PlayerInfo.HouseLocation.Y);
                    var current_tile = worldMap.GetTile(PlayerInfo.Position.X, PlayerInfo.Position.Y);
                    var best_path_to_home = astarService.Run(current_tile, home_tile);

                    if (best_path_to_home == null)
                    {
                        var path = manathan.GetManathanPath(current_tile.Position, PlayerInfo.HouseLocation);
                        return navigationHelper.NavigateToNextPosition(worldMap.GetTile(path[0].X, path[0].Y));
                    }

                    // On est pas rendu
                    return navigationHelper.NavigateToNextPosition(best_path_to_home[1]);
                }
            }
            catch
            {
                Console.WriteLine("*** Reset the map! ***");

                worldMap = new WorldMap();
                worldMap.UpdateWorldMap(map);
                worldMap.HomePosition = PlayerInfo.HouseLocation;
                WorldMap.WriteMap(worldMap);

                return "";
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
    }
}

class TestClass
{
    public string Test { get; set; }
}
 