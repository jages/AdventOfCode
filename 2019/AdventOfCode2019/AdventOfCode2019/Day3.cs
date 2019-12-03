using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    public class Day3
    {
        private List<Distance> DistancesA;
        private List<Distance> DistancesB;

        public class Point
        {
            int X;
            int Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int getManhattanDistance()
            {
                int dist1 = X < 0 ? X * (-1) : X;
                dist1 += Y < 0 ? Y * (-1) : Y;
                
                return dist1;
            }
        }

        public class Distance
        {
            int X1;
            int Y1;
            int X2;
            int Y2;
            char Line;
            bool Horizontal;
            long totalPreviousTravelDistance;

            public Distance(int x1, int y1, int x2, int y2, char line, bool horizontal, long traveledBefore)
            {
                if (x1 <= x2 && y1 <= y2)
                {
                    X1 = x1;
                    Y1 = y1;
                    X2 = x2;
                    Y2 = y2;
                }
                else
                {
                    X1 = x2;
                    Y1 = y2;
                    X2 = x1;
                    Y2 = y1;
                }

                Line = line;
                Horizontal = horizontal;
                totalPreviousTravelDistance = traveledBefore;
            }

            public List<Point> getCrossingPoints(Distance obj)
            {
                List<Point> points = new List<Point>();

                if (this.Horizontal && !obj.Horizontal)
                {
                    points.Add(new Point(obj.X1, this.Y1));
                }
                else if (!this.Horizontal && obj.Horizontal)
                {
                    points.Add(new Point(this.X1, obj.Y1));
                }
                else
                {
                    for (int i = this.X1; i <= this.X2; i++)
                    {
                        for (int j = this.Y1; j <= this.Y2; j++)
                        {
                            if (j >= obj.Y1 && j <= obj.Y2 && i >= obj.X1 && i <= obj.X2)
                                points.Add(new Point(i, j));
                        }
                    }
                }

                return points;
            }

            public bool Crosses(Distance obj)
            {
                if (this.Horizontal && !obj.Horizontal)
                {
                    bool inY = this.Y1 <= obj.Y2 && this.Y1 >= obj.Y1;
                    bool inX = obj.X1 <= this.X2 && obj.X1 >= this.X1;
                    return inY && inX;
                }else if (!this.Horizontal && obj.Horizontal)
                {
                    bool inY = obj.Y1 <= this.Y2 && obj.Y1 >= this.Y1;
                    bool inX = this.X1 <= obj.X2 && this.X1 >= obj.X1;
                    return inY && inX;
                } else if(this.Horizontal && obj.Horizontal)
                {
                    bool inY = this.Y1 == obj.Y1;
                    bool inX = (this.X1 <= obj.X2 && this.X1 >= obj.X1) || (obj.X1 <= this.X2 && obj.X1 >= this.X1);
                    return inX && inY;
                }
                else
                {
                    bool inX = this.X1 == obj.X1;
                    bool inY = (this.Y1 <= obj.Y2 && this.Y1 >= obj.Y1) || (obj.Y1 <= this.Y2 && obj.Y1 >= this.Y1);
                    return inX && inY;
                }
            }

            public int GetManhattanOfClosestPoint()
            {
                int dist1 = 0;
                if (X1 < 0)
                    dist1 += X1 * (-1);
                else
                    dist1 += X1;

                if (Y1 < 0)
                    dist1 += Y1 * (-1);
                else
                    dist1 += Y1;

                int dist2 = 0;
                if (X2 < 0)
                    dist2 += X2 * (-1);
                else
                    dist2 += X2;

                if (Y2 < 0)
                    dist2 += Y2 * (-1);
                else
                    dist2 += Y2;

                return dist1 < dist2 ? dist1 : dist2;
            }
        }


        public int GetSmallestManhattandistance(string path1, string path2)
        {
            DistancesA = new List<Distance>();
            DistancesB = new List<Distance>();

            PaintPath(path1, 'A');
            PaintPath(path2, 'B');

            List<Point> crossings = new List<Point>();
            foreach (var distance in DistancesA)
            {
                foreach (var otherDistance in DistancesB)
                {
                    if(distance.Crosses(otherDistance))
                        crossings.AddRange(distance.getCrossingPoints(otherDistance));
                }
                
            }

            crossings = crossings.OrderBy(x => x.getManhattanDistance()).ToList();


            //PrintField();
            return crossings.Count > 2 ? crossings.ElementAt(1).getManhattanDistance() : 0;
        }

        private void AddDistance(int x1, int y1, int x2, int y2, char line, bool horizontal, long previousTravelDistance)
        {
            if (line == 'A')
                DistancesA.Add(new Distance(x1, y1, x2, y2, line, horizontal, previousTravelDistance));
            if (line == 'B')
                DistancesB.Add(new Distance(x1, y1, x2, y2, line, horizontal, previousTravelDistance));
        }

        private void PaintPath(string path, char FiberNum)
        {
            int currentXPosition = 0;
            int currentYPosition = 0;
            string[] strokes = path.Split(',');
            long distance = 0;
            for (int i = 0; i < strokes.Length; i++)
            {
                int len = int.Parse(strokes[i].Substring(1));
                int target = 0;
                bool horizontal = true;
                switch (strokes[i][0])
                {
                    case 'R':
                        target = currentXPosition + len;
                        horizontal = true;
                        break;
                    case 'L':
                        target = currentXPosition - len;
                        horizontal = true;
                        break;
                    case 'U':
                        target = currentYPosition + len;
                        horizontal = false;
                        break;
                    case 'D':
                        target = currentYPosition - len;
                        horizontal = false;
                        break;
                }

                if (horizontal)
                {
                    AddDistance(currentXPosition, currentYPosition, target, currentYPosition, FiberNum, true, distance);
                    currentXPosition = target;
                }
                else
                {
                    AddDistance(currentXPosition, currentYPosition, currentXPosition, target, FiberNum, false, distance);
                    currentYPosition = target;
                }

                distance += len;
            }
        }
    }
}