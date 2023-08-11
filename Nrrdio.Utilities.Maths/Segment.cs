namespace Nrrdio.Utilities.Maths;

public class Segment {
	public Point Point1 { get; protected init; }
	public Point Point2 { get; protected init; }

	public Point Vector { get; protected init; }
    public Point Midpoint { get; protected init; }

	public double Slope { get; protected init; }
	public double InterceptY { get; protected init; }
	public double InterceptX { get ; protected init; }
    public bool IsPoint { get; protected init; }

	public Segment() { }
	public Segment(Point point1, Point point2) {
		Point1 = point1;
		Point2 = point2;
		
		Vector = point2 - point1;
		Midpoint = new Point(Math.Round((point1.X + point2.X) / 2, 15), Math.Round((point1.Y + point2.Y) / 2, 15));

        Slope = Math.Round((point2.Y - point1.Y) / (point2.X - point1.X), 15);
		InterceptY = point1.Y - (Slope * point1.X);
		InterceptX = Math.Round(-InterceptY / Slope, 15);
		IsPoint = point1 == point2;
    }

	/// <summary>
	/// Determines if and where another segment intersects with this segment.
	/// </summary>
	/// <returns>
	/// bool intersects
	/// Point intersection = null
	/// Point intersectionEnd = null
	/// </returns>
	public (bool Intersects, Point Intersection, Point IntersectionEnd) Intersects(Segment other) {
		var intersects = false;
		Point intersection = null;
		Point intersectionEnd = null;

		var differenceVector1 = Point1 - other.Point1;

        var cross = Math.Round(Vector.Cross(other.Vector), 10);

        // codirectional
        if (cross <= 1e-10 && cross >= 0 - 1e-10) {
			var diffCross1 = Math.Round(Vector.Cross(differenceVector1), 10);
			var diffCross2 = Math.Round(other.Vector.Cross(differenceVector1), 10);

            // not colinear
            if (diffCross1 > 1e-10 || diffCross1 < 0 - 1e-10 || diffCross2 > 1e-10 || diffCross2 < 0 - 1e-10) {
				intersects = false;
			}
			else {
				// both are single points
				if (IsPoint && other.IsPoint) {
					// both are the same point
					intersects = Point1 == other.Point1;
					intersection = new Point(Point1);
				}
				// only this segment is a point
				else if (IsPoint) {
					intersects = Point1.NearLine(other) == 0;
					intersection = new Point(Point1);
				}
				// only the other line is a point
				else if (other.IsPoint) {
					intersects = other.Point1.NearLine(this) == 0;
					intersection = new Point(other.Point1);
				}
				// they are both segments
				else {
					double overlapStart = 0;
					double overlapEnd = 0;
					var differenceVector2 = Point2 - other.Point1;

					// avoid divide by zero
					if (other.Vector.X != 0) {
						overlapStart = Math.Round(differenceVector1.X / other.Vector.X, 15);
						overlapEnd = Math.Round(differenceVector2.X / other.Vector.X, 15);
					}
					else if (other.Vector.Y != 0) {
						overlapStart = Math.Round(differenceVector1.Y / other.Vector.Y, 15);
						overlapEnd = Math.Round(differenceVector2.Y / other.Vector.Y, 15);
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
			var intersectWithThis = Math.Round(other.Vector.Cross(differenceVector1) / cross, 15);

			// confirmed intersection
			if (intersectWithThis >= 0 && intersectWithThis <= 1) {
				var lineIntersection = Point1 + intersectWithThis * Vector;

				if (Contains(lineIntersection) && other.Contains(lineIntersection)) {
					intersects = true;
					intersection = Point1 + intersectWithThis * Vector;
				}
			}
		}

		return (intersects, intersection, intersectionEnd);
	}

	public double Cross(Point point) => Vector.Cross(point - Point1);

    public float AngleTo(Segment other) {
		// Remove imprecision introduced in doubles math.
		var cleanedValue = Math.Round(Vector.Dot(other.Vector) / (Vector.Magnitude * other.Vector.Magnitude), 14);
        var radians = Math.Acos(cleanedValue);

        var degrees = Circle.FromRadians(radians);

		// Loss of precision due to doubles math.
		return Convert.ToSingle(degrees);
    }

    public bool Contains(Point point) => Math.Abs(Vector.Magnitude - ((point - Point1).Magnitude + (point - Point2).Magnitude)) < 1e-10;
	public bool Contains(IEnumerable<Point> points) => points.All(point => Contains(point));
	public bool Contains(params Point[] points) => points.All(point => Contains(point));

	public Point Lerp(double by) => Point1.Lerp(Point2, by);

	public static bool operator ==(Segment left, Segment right) => Equals(left, right);
	public static bool operator !=(Segment left, Segment right) => !Equals(left, right);

	public override bool Equals(object obj) => (obj is Segment other) && Equals(other);
    public bool Equals(Segment other) => (Point1 == other.Point1 && Point2 == other.Point2);
    public static bool Equals(Segment left, Segment right) => left.Equals(right);

	public override int GetHashCode() => ((int) Point1.X ^ (int) Point2.X ^ (int) Point1.Y ^ (int) Point2.Y).GetHashCode();
	public override string ToString() => $"{nameof(Segment)} [{Point1} {Point2}]";
}
