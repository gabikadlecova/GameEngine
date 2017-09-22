using System;
using System.Collections.Generic;
using Casting.Environment.Interfaces;
using Casting.RayCasting.Interfaces;
using Microsoft.Xna.Framework;

namespace Casting.RayCasting
{
    public class RayCaster : IRayCaster
    {
        //ToDo Debug this (or write some nice unit tests)
        private const double Treshold = 2E-12;

        private IMap _map;
        private IContainer<IWall> _container;
        private List<Ray> _rayBuffer;

        public RayCaster(IMap map, IContainer<IWall> container)
        {
            _map = map;
            _container = container;
            _rayBuffer = new List<Ray>();
        }

        public IMap Map
        {
            get { return _map; }
            set { _map = value ?? _map; }
        }

        public IContainer<IWall> Walls
        {
            get { return _container; }
            set { _container = value ?? _container; }
        }

        public List<Ray> FieldOfView(int width, Vector2 position, Vector2 startDirection, Vector2 screenPlane, ICastCondition condition)
        {
            _rayBuffer.Clear();

            for (int x = 0; x < width; x++)
            {
                //todo check direction

                float planeMultiplier = 2F * x / width - 1;
                Vector2 planePart = planeMultiplier * screenPlane;
                Vector2 direction = Vector2.Add(startDirection, planePart);
                Ray ray = Cast(position, direction, condition);

                _rayBuffer.Add(ray);

            }

            return _rayBuffer;
        }

        public Ray Cast(Vector2 startPosition, Vector2 direction, ICastCondition stopCondition)
        {
            Ray resultRay = new Ray();

            //todo floats vs doubles
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

            //Todo try to make this more clear
            if (direction.X > 0)
            {
                sideX = 1;
                double nearestSideX = mapX - rayPositionX;

                rayDistX = (nearestSideX + 1) * deltaX;

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

                rayDistY = (nearestSideY + 1) * deltaY;

            }
            else
            {
                sideY = -1;
                rayDistY = (rayPositionY - mapY) * deltaY;
            }



            if (_map[mapX, mapY] > 0)
            {
                stopCondition.ObstacleCrossed(0);
            }

            while (!stopCondition.IsMet)
            {
                Side side;

                if (rayDistX < rayDistY)
                {
                    mapX += sideX;
                    rayDistX += deltaX;

                    //north or south side?
                    side = Side.SideX;
                }
                else
                {
                    mapY += sideY;
                    rayDistY += deltaY;

                    side = Side.SideY;
                }




                //todo check mapX and mapY



                int index = _map.IsInRange(mapX, mapY) ? _map[mapX, mapY] : -1;
                if (index > 0 || index == -1)
                {
                    IWall crossedWall = _container[index];

                    double wallDistance;
                    double xWallPoint;
                    double obliqueDist;

                    switch (side)
                    {
                        //crossed on x
                        case Side.SideX:
                            obliqueDist = rayDistX;

                            wallDistance = mapX - rayPositionX;
                            wallDistance = wallDistance + (1 - sideX) / 2;

                            wallDistance /= direction.X;
                            xWallPoint = rayPositionY + wallDistance * direction.Y;

                            break;
                        //crossed on y
                        case Side.SideY:
                            obliqueDist = rayPositionY;

                            wallDistance = mapY - rayPositionY;
                            wallDistance = wallDistance + (1 - sideY) / 2;

                            wallDistance /= direction.Y;

                            xWallPoint = rayPositionX + wallDistance * direction.X;
                            break;
                        default:
                            throw new ArgumentException("Wall hit on undefined wall side");
                    }

                    stopCondition.ObstacleCrossed(wallDistance);

                    xWallPoint -= Math.Floor(xWallPoint);

                    float wallPosX = 0;
                    float wallPosY = 0;

                    if (side == Side.SideX)
                    {
                        wallPosX = direction.X > 0 ? mapX : mapX + 1;
                        wallPosY = (float) (mapY + xWallPoint);
                    }

                    if (side == Side.SideY)
                    {
                        wallPosX = (float) (mapX + xWallPoint);
                        wallPosY = direction.Y > 0 ? mapY : mapY + 1;
                    }

                    Vector2 wallPos = new Vector2(wallPosX, wallPosY);
                    
                    resultRay.ObjectsCrossed.Add(new DistanceWrapper<ICrossable>(wallDistance, xWallPoint, side, crossedWall, wallPos, true));


                    if (index == -1)
                    {
                        stopCondition.Reset();
                        return resultRay;
                    }

                    


                }


            }

            stopCondition.Reset();
            return resultRay;

        }
    }
}
