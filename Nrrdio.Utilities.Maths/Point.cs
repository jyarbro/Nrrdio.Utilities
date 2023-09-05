namespace Nrrdio.Utilities.Maths;

public class Point : IComparable<Point> {
	public double X { get; protected init; }
	public double Y { get; protected init; }

	// https://en.wikipedia.org/wiki/Polar_coordinate_system
	public double PhiAngle { get; protected init; }
	public double Magnitude { get; protected init; }
	public double Magnitude2 { get; protected init; }
    public Point Normalized {
        get {
            if (Magnitude == 0) {
                return new Point(0, 0);
            }
            else {
                return new Point(X / Magnitude, Y / Magnitude);
            }
        }
    }

    public Point() { }
	public Point(Point other) : this(other.X, other.Y) { }
	public Point(double x, double y) {
		X = x;
		Y = y;

        PhiAngle = Math.Atan2(y, x);
        Magnitude2 = x * x + y * y;
        Magnitude = Math.Sqrt(Magnitude2);
    }

    public double Distance(Point other) => (this - other).Magnitude;

	/// <summary>
	/// > 0 : point is to the relative left of the line
	/// = 0 : point is on the line
	/// < 0 : point is to the relative right of the line
	/// </summary>
	public float NearLine(Segment segment) => Convert.ToSingle(Math.Round(segment.Vector.Cross(this - segment.Point1), 7, MidpointRounding.ToEven));
    public bool LeftSideOfLine(Segment segment) => NearLine(segment) > 0;
    public bool RightSideOfLine(Segment segment) => NearLine(segment) < 0;
    public bool OnLine(Segment segment) => NearLine(segment) == 0;

    public double Cross(Point other) => X * other.Y - Y * other.X;

	/// <summary>
	/// Dot product that treats a point as a vector from origin
	/// </summary>
	public double Dot(Point other) => X * other.X + Y * other.Y;

    /// <summary>
    /// Unclamped lerp
    /// </summary>
    public Point Lerp(Point other, double by) => this + (other - this) * by;

    public static Point operator +(Point left, Point right) => new (left.X + right.X, left.Y + right.Y);

	public static Point operator -(Point left, Point right) => new (left.X - right.X, left.Y - right.Y);
	public static Point operator -(Point point) => new Point(point.X, point.Y) * -1;

	public static Point operator *(Point point, double scalar) => new (point.X * scalar, point.Y * scalar);
	public static Point operator *(Point point, float scalar) => new (point.X * scalar, point.Y * scalar);
	public static Point operator *(Point point, int scalar) => new (point.X * scalar, point.Y * scalar);
	public static Point operator *(double scalar, Point point) => new (point.X * scalar, point.Y * scalar);
	public static Point operator *(float scalar, Point point) => new (point.X * scalar, point.Y * scalar);
	public static Point operator *(int scalar, Point point) => new (point.X * scalar, point.Y * scalar);

    public static Point operator /(Point point, double scalar) => new(point.X / scalar, point.Y / scalar);
    public static Point operator /(Point point, float scalar) => new(point.X / scalar, point.Y / scalar);
    public static Point operator /(Point point, int scalar) => new(point.X / scalar, point.Y / scalar);
    public static Point operator /(double scalar, Point point) => new(point.X / scalar, point.Y / scalar);
    public static Point operator /(float scalar, Point point) => new(point.X / scalar, point.Y / scalar);
    public static Point operator /(int scalar, Point point) => new(point.X / scalar, point.Y / scalar);

    public static bool operator ==(Point left, Point right) => Equals(left, right);
	public static bool operator !=(Point left, Point right) => !Equals(left, right);

	public override bool Equals(object obj) => (obj is Point other) && Equals(other);
	public static bool Equals(Point left, Point right) => left.Equals(right);
	public bool Equals(Point other) => Math.Abs(X - other.X) < 1e-10 && Math.Abs(Y - other.Y) < 1e-10;
	public int CompareTo(Point other) => PhiAngle.CompareTo(other.PhiAngle);

	public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode() << 2;
	public override string ToString() => $"({X:0.###}, {Y:0.###})";
}

public static class PointExtensions {
	public static List<Point> Sort(this List<Point> points, Point center) => points.OrderBy(p => (center - p).PhiAngle).ToList();
}
