namespace Nrrdio.Utilities.Maths;

/// <summary>
/// Creates a uniform catmull-rom spline.
/// If concerned about loops or self-intersections, use centripetal spline.
/// If performance is priority, use this.
/// </summary>
public class CatmullRomSpline {
    public Point P0 { get; }
    public Point P1 { get; }
    public Point P2 { get; }
    public Point P3 { get; }
    public double A { get; set; }


    public CatmullRomSpline(
        Point p0,
        Point p1,
        Point p2,
        Point p3,
        double tension = 0.5
    ) {
        P0 = p0;
        P1 = p1;
        P2 = p2;
        P3 = p3;
        A = tension;
    }

    public Point Interpolate(double t) {
        var tSqr = t * t;
        var tCub = tSqr * t;

        // Simplified the matrix terms up front for readability.
        // Variable initialization has a negligible cost here for the benefit.
        var m1 = (tSqr * 2) - t - tCub;
        var m2 = 2 - (tSqr * 5) + (tCub * 3);
        var m3 = t + (tSqr * 4) - (tCub * 3);
        var m4 = tCub - tSqr;

        //var m1 = A * -tCub + 2 * A * tSqr - A * t;
        //var m2 = (2 - A) * tCub + (A - 3) * tSqr + 1;
        //var m3 = (A - 2) * tCub + (3 - 2 * A) * tSqr + A * t;
        //var m4 = A * tCub - A * tSqr;

        return A * (P0 * m1 + P1 * m2 + P2 * m3 + P3 * m4);
    }

    public float AngleAt(double t) {
        var tangent = Interpolate(t + .01) - Interpolate(t - .01);
        return Formula.RadiansToDegrees(Math.Atan2(tangent.Y, tangent.X));
    }
}
