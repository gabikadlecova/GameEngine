﻿using System;
using Casting.Environment.Interfaces;
using Casting.RayCasting.Interfaces;

namespace Casting.RayCasting
{
    public partial class RayCaster : IRayCaster
    {
        //ToDo Debug this (or write some nice unit tests)
        private const double Treshold = 2E-12;

        private IMap _map;
        private IWallContainer _wallContainer;

        public RayCaster(IMap map, IWallContainer container)
        {
            _map = map;
            _wallContainer = container;
        }

        public IMap Map
        {
            get { return _map; }
            set { _map = value ?? _map; }
        }

        public IWallContainer Walls
        {
            get { return _wallContainer; }
            set { _wallContainer = value ?? _wallContainer; }
        }

        public IRay Cast(IVector startPosition, IVector direction, ICastCondition stopCondition)
        {
            Ray resultRay = new Ray();


            double rayPositionX = startPosition.X;
            double rayPositionY = startPosition.Y;
            int mapX = (int)Math.Floor(rayPositionX);
            int mapY = (int)Math.Floor(rayPositionY);

            double deltaX = Math.Sqrt(1 + (direction.Y * direction.Y) / (direction.X * direction.X));
            double deltaY = Math.Sqrt(1 + (direction.X * direction.X) / (direction.Y * direction.Y));

            int sideX;
            int sideY;
            double rayDistX;
            double rayDistY;
            bool gridPoint = false;

            //Todo try to make this more clear
            if (direction.X > 0)
            {
                sideX = 1;
                double nearestSideX = mapX - rayPositionX;
                if (Math.Abs(nearestSideX) < Treshold)
                {
                    rayDistX = 0;
                    gridPoint = true;
                }
                else
                {
                    rayDistX = (nearestSideX + 1) * deltaX;
                }
            }
            else
            {
                sideX = -1;
                rayDistX = (rayPositionX - mapX) * deltaX;
            }

            if (direction.Y > 0)
            {
                sideY = 1;
                double nearestSideY = mapY - rayPositionY;
                if (Math.Abs(nearestSideY) < Treshold)
                {
                    rayDistY = 0;
                    gridPoint = true;
                }
                else
                {
                    rayDistY = (nearestSideY + 1) * deltaY;
                }
            }
            else
            {
                sideY = -1;
                rayDistY = (rayPositionY - mapY) * deltaY;
            }
            


            if(gridPoint && _map[mapX, mapY] > 0)
            {
                stopCondition.WallCrossed(0);
            }
            
            while (!stopCondition.IsMet)
            {
                Side side;
                double smallVal = Math.Abs(rayDistX - rayDistY);
                if (smallVal < Treshold)
                {
                    //ToDo check diagonal movement
                    mapX += sideX;
                    mapY += sideY;

                    rayDistX += deltaX;
                    rayDistY += deltaY;

                    side = Side.Corner;
                }
                else
                {
                    if (rayDistX < rayDistY)
                    {
                        mapX += sideX;
                        rayDistX += deltaX;

                        side = Side.SideX;
                    }
                    else
                    {
                        mapY += sideY;
                        rayDistY += deltaY;

                        side = Side.SideY;
                    }
                }

                //todo check mapX and mapY
                if (!_map.IsInRange(mapX, mapY))
                    return resultRay;

                int index = _map[mapX, mapY];
                if (index > 0)
                {

                    
                    IWall crossedWall = _wallContainer[index];
                    
                    double wallDistance;
                    double xWallPoint;

                    switch(side)
                    {
                        //crossed on x
                        case Side.SideX:
                            wallDistance = mapX - rayPositionX;
                            wallDistance = !gridPoint ? wallDistance + (1 - sideX) / 2 : wallDistance;
                            
                            wallDistance /= direction.X;
                            xWallPoint = rayPositionY + wallDistance * direction.Y;

                            break;
                        //crossed on y or diagonal
                        case Side.Corner:
                        case Side.SideY:
                            wallDistance = mapY - rayPositionY;     
                            wallDistance = !gridPoint ? wallDistance + (1 - sideY) / 2 : wallDistance;
                            
                            wallDistance /= direction.Y;

                            xWallPoint = rayPositionX + wallDistance * direction.X;
                            xWallPoint = side == Side.Corner ? 0 : xWallPoint;
                            break;
                        default:
                            throw new ArgumentException("Wall hit on undefined wall side");
                    }

                    stopCondition.WallCrossed(wallDistance);

                    xWallPoint -= Math.Floor(xWallPoint);
                    resultRay.WallsCrossed.Add(new DistanceWrapper<IWall>(wallDistance, xWallPoint, side, crossedWall));
                }
                

            }

            stopCondition.Reset();
            return resultRay;

        }
    }
}
