namespace Nrrdio.Utilities.Maths {
    public class Hexagon : Polygon {
        const double BASE_APOTHEM = 0.8660254037844386467637231708;

        public double Radius {
            get => _Radius;
            private set {
                if (_Radius != value) {
                    _Radius = value;
                    Apothem = BASE_APOTHEM * _Radius;
                }
            }
        }
        double _Radius;

        public double Apothem { get; private set; }

        public Hexagon(Point center, int segmentsPerSide, double segmentLength) {
            Radius = segmentsPerSide * segmentLength;
            var lerpBy = 1d / segmentsPerSide;

            var vertex0 = new Point(center.X + Radius, center.Y);
            var vertex1 = new Point(center.X + Radius * 0.5d, center.Y + Apothem);
            var vertex2 = new Point(center.X - Radius * 0.5d, center.Y + Apothem);
            var vertex3 = new Point(center.X - Radius, center.Y);
            var vertex4 = new Point(center.X - Radius * 0.5d, center.Y - Apothem);
            var vertex5 = new Point(center.X + Radius * 0.5d, center.Y - Apothem);

            addSide(vertex0, vertex1);
            addSide(vertex1, vertex2);
            addSide(vertex2, vertex3);
            addSide(vertex3, vertex4);
            addSide(vertex4, vertex5);
            addSide(vertex5, vertex0);

            CalculateValuesFromVertices();

            void addSide(Point sideVertex1, Point sideVertex2) {
                for (var i = 0; i < segmentsPerSide; i++) {
                    _Vertices.Add(sideVertex1.Lerp(sideVertex2, lerpBy * i));
                }
            }
        }
    }
}
