﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Nrrdio.Utilities.Maths {
    public class Polygon {
        public IEnumerable<Point> Vertices => _Vertices;
        protected readonly List<Point> _Vertices = new List<Point>();

        public IEnumerable<Segment> Edges => _Edges;
        protected readonly List<Segment> _Edges = new List<Segment>();

        public Circle Circumcircle {
            get {
                if (_Circumcircle is default(Circle)) {
                    _Circumcircle = new Circle(Vertices, Centroid);
                }

                return _Circumcircle;
            }
        }
        Circle _Circumcircle;

        public Point Centroid { get; private set; }
        public double Area { get; private set; }
        public double SignedArea { get; private set; }
        public EWinding Winding { get; private set; }

        public Polygon(params Point[] points) : this(points.ToList()) { }
        public Polygon(IEnumerable<Point> points) {
            _Vertices.AddRange(points);
            CalculateValuesFromVertices();
        }

        public bool Contains(Point point) {
            var vertices = _Vertices.Count;
            var winding = 0;
            Segment currentLine;
            Point currentVertex;
            Point nextVertex;
            int j;

            for (var i = 0; i < vertices; i++) {
                j = (i + 1) % vertices;

                currentLine = _Edges[i];
                currentVertex = _Vertices[i];
                nextVertex = _Vertices[j];

                if (currentVertex.Y <= point.Y) {
                    if (nextVertex.Y > point.Y) {
                        if (point.NearLine(currentLine) > 0) {
                            winding++;
                        }
                    }
                }

                else {
                    if (nextVertex.Y <= point.Y) {
                        if (point.NearLine(currentLine) < 0) {
                            winding--;
                        }
                    }
                }
            }

            return winding != 0;
        }

        public bool SharesEdgeWith(Polygon other) => 2 == Vertices.Where(other.Vertices.Contains).Count();

        public bool IsConvex() {
            var totalEdges = _Edges.Count;
            int j;
            var neg = false;
            var pos = false;

            for (var i = 0; i < totalEdges; i++) {
                j = (i + 1) % totalEdges;

                var cross = _Edges[i].Vector.Cross(_Edges[j].Vector);

                if (cross < 0) {
                    neg = true;
                }

                if (cross > 0) {
                    pos = true;
                }

                if (neg & pos) {
                    return false;
                }
            }

            return true;
        }

        protected void CalculateValuesFromVertices() {
            GenerateEdges();
            CalculateArea();
            CalculateCentroid();
            CalculateWinding();
        }

        void GenerateEdges() {
            var vertexCount = Vertices.Count();

            for (var i = 0; i < vertexCount; i++) {
                var j = (i + 1) % vertexCount;
                _Edges.Add(new Segment(_Vertices[i], _Vertices[j]));
            }
        }

        // https://en.wikipedia.org/wiki/Shoelace_formula
        void CalculateArea() {
            var vertices = _Vertices.Count;
            int j;

            for (var i = 0; i < vertices; i++) {
                j = (i + 1) % vertices;
                SignedArea += _Vertices[i].Cross(_Vertices[j]) * 0.5;
            }

            Area = Math.Abs(SignedArea);
        }

        // https://en.wikipedia.org/wiki/Centroid
        void CalculateCentroid() {
            var totalVertices = _Vertices.Count;
            int j;
            Point current;
            Point next;
            double x = 0d;
            double y = 0d;
            double cross;

            for (var i = 0; i < totalVertices; i++) {
                j = (i + 1) % totalVertices;

                current = _Vertices[i];
                next = _Vertices[j];
                cross = current.Cross(next);

                x += (current.X + next.X) * cross;
                y += (current.Y + next.Y) * cross;
            }

            x /= 6 * SignedArea;
            y /= 6 * SignedArea;

            Centroid = new Point(x, y);
        }

        // Direction of vectors based on signed area.
        void CalculateWinding() {
            Winding = EWinding.NONE;

            if (SignedArea < 0) {
                Winding = EWinding.CLOCKWISE;
            }

            if (SignedArea > 0) {
                Winding = EWinding.COUNTERCLOCKWISE;
            }
        }

        public override bool Equals(object obj) => (obj is Polygon other) && Equals(other);
        public bool Equals(Polygon other) {
            if (_Vertices.Count != other._Vertices.Count) {
                return false;
            }

            var reversed = false;
            var areEqual = false;

            if (Winding != other.Winding) {
                reversed = true;
                _Vertices.Reverse();
            }

            var start = _Vertices.IndexOf(other._Vertices[0]);

            if (start >= 0) {
                areEqual = true;

                for (var i = 0; i < other._Vertices.Count; i++) {
                    var j = (i + start) % other._Vertices.Count;

                    if (other._Vertices[i] != _Vertices[j]) {
                        areEqual = false;
                        break;
                    }
                }
            }

            if (reversed) {
                _Vertices.Reverse();
            }

            return areEqual;
        }
        public static bool Equals(Polygon left, Polygon right) => left.Equals(right);

        public override string ToString() => $"{nameof(Polygon)} ({string.Join("), (", Vertices)})";
        public override int GetHashCode() {
            var hashCode = _Vertices[0].GetHashCode();

            for (var i = 1; i < _Vertices.Count; i++) {
                hashCode ^= _Vertices[i].GetHashCode();
            }

            return hashCode;
        }

        public enum EWinding {
            CLOCKWISE, COUNTERCLOCKWISE, NONE
        }
    }
}
