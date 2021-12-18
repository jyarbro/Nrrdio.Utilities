using System;
using System.Collections.Generic;
using System.Linq;

namespace Nrrdio.Utilities.Maths {
    public class Segment {
        public Point Vector { get; }
        public Point Point1 { get; }
        public Point Point2 { get; }
        public double Slope => (Point2.Y - Point1.Y) / (Point2.X - Point1.X);
        public double InterceptY => Point1.Y - (Slope * Point1.X);
        public double InterceptX => -InterceptY / Slope;

        public Segment(Point point1, Point point2) {
            Point1 = point1;
            Point2 = point2;
            Vector = point2 - point1;
        }

        /// <summary>
        /// Determines if and where another segment intersects with this segment.
        /// </summary>
        /// <returns>
        /// bool intersects
        /// Point intersection = null
        /// Point intersectionEnd = null
        /// </returns>
        public (bool, Point, Point) Intersects(Segment other) {
            var intersects = false;
            Point intersection = null;
            Point intersectionEnd = null;

            var offsetVector = Point1 - other.Point1;

            var cross = Vector.Cross(other.Vector);

            // parallel
            if (cross == 0) {
                // not collinear
                if (Vector.Cross(offsetVector) != 0 || other.Vector.Cross(offsetVector) != 0) {
                    intersects = false;
                }
                else {
                    var thisIsPoint = Point1 == Point2;
                    var otherIsPoint = other.Point1 == other.Point2;

                    // both are single points
                    if (thisIsPoint && otherIsPoint) {
                        // both are the same point
                        intersects = Point1 == other.Point1;
                        intersection = new Point(Point1.X, Point1.Y);
                    }
                    // only this segment is a point
                    else if (thisIsPoint) {
                        intersects = Point1.NearLine(other) == 0;
                        intersection = new Point(Point1.X, Point1.Y);
                    }
                    // only the other line is a point
                    else if (otherIsPoint) {
                        intersects = other.Point1.NearLine(this) == 0;
                        intersection = new Point(other.Point1.X, other.Point1.Y);
                    }
                    // they are both segments
                    else {
                        double overlapStart = 0;
                        double overlapEnd = 0;
                        var differenceVector2 = Point2 - other.Point1;

                        // avoid divide by zero
                        if (other.Vector.X != 0) {
                            overlapStart = offsetVector.X / other.Vector.X;
                            overlapEnd = differenceVector2.X / other.Vector.X;
                        }
                        else if (other.Vector.Y != 0) {
                            overlapStart = offsetVector.Y / other.Vector.Y;
                            overlapEnd = differenceVector2.Y / other.Vector.Y;
                        }

                        // ensure small before large
                        if (overlapStart > overlapEnd) {
                            var temp = overlapStart;
                            overlapStart = overlapEnd;
                            overlapEnd = temp;
                        }

                        // confirmed overlap
                        if (overlapEnd > 0 && overlapStart < 1) {
                            // clip to min 0, max 1
                            overlapStart = overlapStart < 0 ? 0 : overlapStart;
                            overlapEnd = overlapEnd > 1 ? 1 : overlapEnd;

                            // intersection is on a segment terminator
                            if (overlapStart == overlapEnd) {
                                intersection = other.Point1 + overlapStart * other.Vector;
                                intersects = true;
                            }
                            // they overlap in a valid subsegment
                            else {
                                intersection = other.Point1 + overlapStart * other.Vector;
                                intersectionEnd = other.Point1 + overlapEnd * other.Vector;
                                intersects = true;
                            }
                        }
                    }
                }
            }
            else {
                var intersectWithThis = other.Vector.Cross(offsetVector) / cross;

                // confirmed intersection
                if (intersectWithThis >= 0 && intersectWithThis <= 1) {
                    intersects = true;
                    intersection = Point1 + intersectWithThis * Vector;
                }
            }

            return (intersects, intersection, intersectionEnd);
        }

        public double Cross(Point point) => (point.Y - Point1.Y) * (Point2.X - Point1.X) - (point.X - Point1.X) * (Point2.Y - Point1.Y);

        public bool Contains(Point point) {
            var ma = (Point1 - Point2).Magnitude;
            var mb = (point - Point1).Magnitude;
            var mc = (point - Point2).Magnitude;

            return ma == mb + mc;
        }

        public bool Contains(IEnumerable<Point> points) => points.All(point => Contains(point));
        public bool Contains(params Point[] points) => points.All(point => Contains(point));

        public Point Lerp(double by) => Point1.Lerp(Point2, by);

        public static bool operator ==(Segment left, Segment right) => Equals(left, right);
        public static bool operator !=(Segment left, Segment right) => !Equals(left, right);

        public override bool Equals(object obj) => (obj is Segment other) && Equals(other);
        public bool Equals(Segment other) {
            return (Point1 == other.Point1 && Point2 == other.Point2)
                || (Point1 == other.Point2 && Point2 == other.Point1);
        }
        public static bool Equals(Segment left, Segment right) => left.Equals(right);

        public override int GetHashCode() => ((int)Point1.X ^ (int)Point2.X ^ (int)Point1.Y ^ (int)Point2.Y).GetHashCode();
    }
}
