namespace Nrrdio.Utilities.Maths;

public class Polygon {
	public List<Point> Vertices { get; } = new List<Point>();
	public IList<Segment> Edges { get; } = new List<Segment>();

	public Circle Circumcircle { get; protected init; }
	public Point Centroid { get; protected init; }
	public double Area { get; protected init; }
	public double SignedArea { get; protected init; }
	public EWinding Winding { get; protected init; }
	public int VertexCount { get; protected init; }

	public Polygon() { }
	public Polygon(params Point[] vertices) : this(vertices.AsEnumerable()) { }
	public Polygon(IEnumerable<Point> vertices) {
		if (vertices is null || !vertices.Any()) {
            throw new ArgumentException("Polygons require vertices", nameof(vertices));
		}

		Vertices.AddRange(vertices);
		VertexCount = Vertices.Count;

		CreateEdges();

		SignedArea = CalculateSignedArea();
		Area = Math.Abs(SignedArea);
		Centroid = CalculateCentroid();
		Winding = CalculateWinding();
		Circumcircle = new Circle(Vertices, Centroid);
	}

	public bool Contains(Point point) {
        var contains = false;

		for (var i = 0; i < VertexCount; i++) {
			var j = (i + 1) % VertexCount;
			var point1 = Vertices[i];
			var point2 = Vertices[j];
			var vector1 = point1 - point2;
			var vector2 = point - point2;

			if (point1.Y <= point.Y && point.Y <= point2.Y
			 || point2.Y <= point.Y && point.Y <= point1.Y) {
				if (point2.X + vector2.Y / vector1.Y * vector1.X < point.X) {
					contains = !contains;
				}
			}
		}

		// The point isn't inside the border but maybe on an edge
		if (!contains) {
			contains = Edges.Any(edge => edge.Contains(point));
		}

		return contains;
	}
	public bool Contains(IEnumerable<Point> points) => points.All(point => Contains(point));
	public bool Contains(params Point[] points) => points.All(point => Contains(point));

	public bool SharesEdgeWith(Polygon other) => 2 == Vertices.Where(other.Vertices.Contains).Count();

	public bool IsConvex() {
		var totalEdges = Edges.Count;
		int j;
		var neg = false;
		var pos = false;

		for (var i = 0; i < totalEdges; i++) {
			j = (i + 1) % totalEdges;

			var cross = Edges[i].Vector.Cross(Edges[j].Vector);

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

	protected void CreateEdges() {
		for (var i = 0; i < VertexCount; i++) {
			var j = (i + 1) % VertexCount;
			Edges.Add(new Segment(Vertices[i], Vertices[j]));
		}
	}

	// https://en.wikipedia.org/wiki/Shoelace_formula
	protected double CalculateSignedArea() {
		int j;
		var signedArea = 0d;

		for (var i = 0; i < VertexCount; i++) {
			j = (i + 1) % VertexCount;
			signedArea += Vertices[i].Cross(Vertices[j]) * 0.5;
		}

		return signedArea;
	}

	// https://en.wikipedia.org/wiki/Centroid
	protected Point CalculateCentroid() {
		int j;
		Point current;
		Point next;
		double x = 0d;
		double y = 0d;
		double cross;

		for (var i = 0; i < VertexCount; i++) {
			j = (i + 1) % VertexCount;

			current = Vertices[i];
			next = Vertices[j];
			cross = current.Cross(next);

			x += (current.X + next.X) * cross;
			y += (current.Y + next.Y) * cross;
		}

		x /= 6 * SignedArea;
		y /= 6 * SignedArea;

		return new Point(x, y);
	}

	// Direction of vectors based on signed area.
	protected EWinding CalculateWinding() {
		var winding = EWinding.NONE;

		if (SignedArea < 0) {
			winding = EWinding.CLOCKWISE;
		}

		if (SignedArea > 0) {
			winding = EWinding.COUNTERCLOCKWISE;
		}

		return winding;
	}

	public override bool Equals(object obj) => (obj is Polygon other) && Equals(other);
	public bool Equals(Polygon other) {
		if (VertexCount != other.VertexCount) {
			return false;
		}

		var reversed = false;
		var areEqual = false;

		if (Winding != other.Winding) {
			reversed = true;
			Vertices.Reverse();
		}

		var start = Vertices.IndexOf(other.Vertices[0]);

		if (start >= 0) {
			areEqual = true;

			for (var i = 0; i < other.VertexCount; i++) {
				var j = (i + start) % other.VertexCount;

				if (other.Vertices[i] != Vertices[j]) {
					areEqual = false;
					break;
				}
			}
		}

		if (reversed) {
			Vertices.Reverse();
		}

		return areEqual;
	}
	public static bool Equals(Polygon left, Polygon right) => left.Equals(right);

	public override int GetHashCode() {
		var hashCode = Vertices[0].GetHashCode();

		for (var i = 1; i < VertexCount; i++) {
			hashCode ^= Vertices[i].GetHashCode();
		}

		return hashCode;
	}
	public override string ToString() => $"{Edges.Count} sides";

	public enum EWinding {
		CLOCKWISE, COUNTERCLOCKWISE, NONE
	}
}
