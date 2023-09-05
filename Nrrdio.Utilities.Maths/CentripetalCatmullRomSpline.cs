using System.ComponentModel.DataAnnotations;

namespace Nrrdio.Utilities.Maths;

/// <summary>
/// Creates a catmull-rom spline without loops or self-intersections.
/// If concerned about performance, use the other CatmullRomSpline class.
/// 
/// Curve types:
/// Uniform: alpha is 0
/// Centripetal: alpha is 0.5
/// Chordal: alpha is 1
/// </summary>
public class CentripetalCatmullRomSpline {
    public Point P0 { get; set; }
    public Point P1 { get; set; }
    public Point P2 { get; set; }
    public Point P3 { get; set; }

    double K1 { get; set; }
    double K2 { get; set; }
    double K3 { get; set; }

    public CentripetalCatmullRomSpline(
        Point p0,
        Point p1,
        Point p2,
        Point p3,
        double alpha = 0.5
    ) {
        P0 = p0;
        P1 = p1;
        P2 = p2;
        P3 = p3;

        alpha *= 0.5;

        // Knot intervals
        K1 = Math.Pow((P0 - P1).Magnitude2, alpha);
        K2 = Math.Pow((P1 - P2).Magnitude2, alpha) + K1;
        K3 = Math.Pow((P2 - P3).Magnitude2, alpha) + K2;
    }

    /// <summary>
    /// Barry-Goldman Pyramidal Interpolation
    /// Inspired by https://en.wikipedia.org/wiki/Centripetal_Catmull%E2%80%93Rom_spline
    /// </summary>
    public Point Interpolate(double t) {
        var lerp = Formula.Lerp(K1, K2, t);

        var a1 = Remap(0, K1, P0, P1, lerp);
        var a2 = Remap(K1, K2, P1, P2, lerp);
        var a3 = Remap(K2, K3, P2, P3, lerp);

        var b1 = Remap(0, K2, a1, a2, lerp);
        var b2 = Remap(K1, K3, a2, a3, lerp);

        return Remap(K1, K2, b1, b2, lerp);
    }

    Point Remap(double k1, double k2, Point p1, Point p2, double t) => p1.Lerp(p2, (t - k1) / (k2 - k1));
}
