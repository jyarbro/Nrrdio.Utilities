﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Nrrdio.Utilities.Maths {
    public class Point : IComparable<Point> {
        public double X { get; set; }
        public double Y { get; set; }

        // https://en.wikipedia.org/wiki/Polar_coordinate_system
        public double PhiAngle => Math.Atan2(Y, X);
        public double RadialMagnitude => Math.Sqrt(X * X + Y * Y);

        public Point(double x, double y) {
            X = x;
            Y = y;
        }

        public double Distance(Point other) => (this - other).RadialMagnitude;

        /// <summary>
        /// > 0 : point is to the left of the line
        /// = 0 : point is on the line
        /// < 0 : point is to the right of the line
        /// </summary>
        public double NearLine(Segment segment) => segment.Vector.Cross(this - segment.Point1);

        /// <summary>
        /// > 0 : This is on right of the other.
        /// = 0 : Overlapping
        /// < 0 : This is on left of the other.
        /// </summary>
        public double Cross(Point other) => X * other.Y - Y * other.X;

        /// <summary>
        /// Dot product that treats a point as a vector from origin
        /// </summary>
        public double Dot(Point other) => X * other.X + Y * other.Y;

        public Point Lerp(Point other, double by) {
            var x = lerp(X, other.X);
            var y = lerp(Y, other.Y);

            return new Point(x, y);

            double lerp(double a, double b) => a * (1 - by) + b * by;
        }

        public static Point operator +(Point left, Point right) => new Point(left.X + right.X, left.Y + right.Y);

        public static Point operator -(Point left, Point right) => new Point(left.X - right.X, left.Y - right.Y);
        public static Point operator -(Point point) => new Point(point.X, point.Y) * -1;

        public static Point operator *(Point point, double scalar) => new Point(point.X * scalar, point.Y * scalar);
        public static Point operator *(double scalar, Point point) => new Point(point.X * scalar, point.Y * scalar);

        public static bool operator ==(Point left, Point right) => Equals(left, right);
        public static bool operator !=(Point left, Point right) => !Equals(left, right);

        public override bool Equals(object obj) => (obj is Point other) && Equals(other);
        public static bool Equals(Point left, Point right) => left.Equals(right);
        public bool Equals(Point other) => Math.Abs(X - other.X) < 0.000000000000001 && Math.Abs(Y - other.Y) < 0.000000000000001;

        public override string ToString() => $"{nameof(Point)} ({X:0.###}, {Y:0.###})";
        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode() << 2;

        public int CompareTo(Point other) => PhiAngle.CompareTo(other.PhiAngle);
    }

    public static class PointExtensions {
        public static List<Point> Sort(this List<Point> points, Point center) => points.OrderBy(p => (center - p).PhiAngle).ToList();
    }
}
