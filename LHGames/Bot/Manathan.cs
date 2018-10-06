using LHGames.Helper;
using LHGames.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LHGames.Bot
{
    public class Manathan : IManathan
    {
        public List<Point> GetManathanPath(Point start, Point end)
        {
            List<Point> path = new List<Point>();

            int distanceInX = end.X - start.X;
            int distanceInY = end.Y - start.Y;

            if (distanceInX != 0)
            {
                int directionInX = distanceInX / Math.Abs(distanceInX);
                for (int i = 1; i <= Math.Abs(distanceInX); ++i)
                {
                    path.Add(new Point(start.X + directionInX * i, start.Y));
                }
            }

            if (distanceInY != 0)
            {
                int directionInY = distanceInY / Math.Abs(distanceInY);

                for (int i = 1; i <= Math.Abs(distanceInY); ++i)
                {
                    path.Add(new Point(end.X, start.Y + directionInY * i));
                }
            }

            return path;
        }
    }
}
