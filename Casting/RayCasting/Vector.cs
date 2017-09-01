using System;
using Casting.RayCasting.Interfaces;

namespace Casting.RayCasting
{
    public class Vector : IVector
    {
        public static IVector NullVector = new Vector(0, 0);

        public double X { get; private set; }
        public double Y { get; private set; }

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }
        public static IVector Multiply(IVector left, IVector right)
        {
            return new Vector(left.X * right.X, left.Y * right.Y);
        }

        public double Norm
        {
            get { return Math.Sqrt(X * X + Y * Y); }
        }

        public static IVector Multiply(double alpha, IVector vector)
        {
            if (Math.Abs(alpha) < 2E-12)
                return NullVector;
                //throw new ArgumentException("Vector cannot be multiplied by zero.");
            return new Vector(vector.X * alpha, vector.Y * alpha);
        }

        public void Resize(double multiplier)
        {
            if (Math.Abs(multiplier) < 2E-12)
                throw new ArgumentException("Vector cannot be multiplied by zero.");
            X *= multiplier;
            Y *= multiplier;
        }

        public void Rotate(double alpha)
        {
            double cos = Math.Cos(alpha);
            double sin = Math.Sin(alpha);

            X = X * cos + Y * sin;
            Y = -X * sin + Y * cos;
        }

        public static IVector Add(IVector left, IVector right)
        {
            return new Vector(left.X + right.X, left.Y + right.Y);
        }
    }
}
