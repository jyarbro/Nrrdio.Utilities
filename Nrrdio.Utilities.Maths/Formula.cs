namespace Nrrdio.Utilities.Maths {
    public class Formula {
        // Based on Unity Standard Assets
        // https://en.wikipedia.org/wiki/Centripetal_Catmull%E2%80%93Rom_spline
        public static Point CatmullRom(Point p0, Point p1, Point p2, Point p3, double i) {
            return 0.5 * ((2.0 * p1) + (-p0 + p2) * i + (2.0 * p0 - 5.0 * p1 + 4.0 * p2 - p3) * (i * i) + (-p0 + 3.0 * p1 - 3.0 * p2 + p3) * (i * i * i));
        }
    }
}
