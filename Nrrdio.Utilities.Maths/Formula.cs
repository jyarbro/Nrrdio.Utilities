namespace Nrrdio.Utilities.Maths;

public class Formula {
    // Unclamped lerp implementation
    public static double Lerp(double a, double b, double t) => a + (b - a) * t;
    public static float DegreesToRadians(double degrees) => Convert.ToSingle(Math.Round(Math.PI / 180 * degrees, 7, MidpointRounding.ToEven));
    public static float RadiansToDegrees(double radians) => Convert.ToSingle(Math.Round(radians * (180 / Math.PI), 7, MidpointRounding.ToEven));

}
