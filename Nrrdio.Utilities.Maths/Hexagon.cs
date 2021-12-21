using System;
using System.Collections.Generic;

namespace Nrrdio.Utilities.Maths {
    public class Hexagon : Polygon {
        const double BASE_APOTHEM = 0.8660254037844386467637231708;

        public double Radius { get; protected init; }
        public double Apothem { get; protected init; }

        public Hexagon(Point center, int segmentsPerSide, double segmentLength) {
            Radius = segmentsPerSide * segmentLength;
            Apothem = BASE_APOTHEM * Radius;

            var lerpBy = 1d / segmentsPerSide;

            var vertex0 = new Point(center.X + Radius, center.Y);
            var vertex1 = new Point(center.X + Radius * 0.5d, center.Y + Apothem);
            var vertex2 = new Point(center.X - Radius * 0.5d, center.Y + Apothem);
            var vertex3 = new Point(center.X - Radius, center.Y);
            var vertex4 = new Point(center.X - Radius * 0.5d, center.Y - Apothem);
            var vertex5 = new Point(center.X + Radius * 0.5d, center.Y - Apothem);

            var points = new List<Point>();

            addSegmentedSide(vertex0, vertex1);
            addSegmentedSide(vertex1, vertex2);
            addSegmentedSide(vertex2, vertex3);
            addSegmentedSide(vertex3, vertex4);
            addSegmentedSide(vertex4, vertex5);
            addSegmentedSide(vertex5, vertex0);

            Vertices.AddRange(points);
            VertexCount = Vertices.Count;

            CreateEdges();

            SignedArea = CalculateSignedArea();
            Area = Math.Abs(SignedArea);
            Centroid = CalculateCentroid();
            Winding = CalculateWinding();
            Circumcircle = new Circle(Vertices, Centroid);

            void addSegmentedSide(Point sideVertex1, Point sideVertex2) {
                for (var i = 0; i < segmentsPerSide; i++) {
                    points.Add(sideVertex1.Lerp(sideVertex2, lerpBy * i));
                }
            }
        }
    }
}
