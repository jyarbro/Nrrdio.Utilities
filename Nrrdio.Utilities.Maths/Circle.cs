namespace Nrrdio.Utilities.Maths;

public class Circle {
	public Point Center { get; private set; }

	public double Radius {
		get => _Radius;
		private set {
			if (_Radius != value) {
				_Radius = value;
				RadiusSquared = Math.Pow(value, 2);
			}
		}
	}
	double _Radius;

	public double RadiusSquared { get; private set; }

	public Circle(Point center, double radius) {
		Radius = radius;
		Center = center;
	}

	public Circle(Point point1, Point point2) {
		Center = point1.Lerp(point2, .5);
		Radius = Math.Max(Center.Distance(point1), Center.Distance(point2));
	}

	public Circle(IEnumerable<Point> points, Point centroid) {
		if (points.Count() < 2 || centroid is null) {
			throw new Exception("A circle requires at least three points and a centroid.");
		}
		else {
			// Copy the list so we don't mess up the order.
			var circlePoints = new List<Point>(points);

			// Take the 3 points with the highest magnitude from a centroid and ignore all the internal points. This helps with generating circumcircles.
			circlePoints = circlePoints.OrderByDescending(p => (centroid - p).Magnitude).Take(3).ToList();

			FromCircumcircle(circlePoints[0], circlePoints[1], circlePoints[2]);
		}
	}

	public Polygon SuperTriangle() {
		// relative vectors
		var mp1x = Radius * Math.Cos(ToRadians(60d));
		var mp1y = Radius * Math.Sin(ToRadians(60d));
		var mp2x = Radius * Math.Cos(ToRadians(180d));
		var mp2y = Radius * Math.Sin(ToRadians(180d));
		var mp3x = Radius * Math.Cos(ToRadians(300d));
		var mp3y = Radius * Math.Sin(ToRadians(300d));

		// midpoints
		var mp1 = Center + new Point(mp1x, mp1y);
		var mp2 = Center + new Point(mp2x, mp2y);
		var mp3 = Center + new Point(mp3x, mp3y);

		// outer triangle vertices
		var p1x = mp1.X + mp3.X - mp2.X;
		var p1y = mp1.Y + mp3.Y - mp2.Y;
		var p2x = mp1.X + mp2.X - mp3.X;
		var p2y = mp1.Y + mp2.Y - mp3.Y;
		var p3x = mp2.X + mp3.X - mp1.X;
		var p3y = mp2.Y + mp3.Y - mp1.Y;

		var point1 = new Point(p1x, p1y);
		var point2 = new Point(p2x, p2y);
		var point3 = new Point(p3x, p3y);

		return new Polygon(point1, point2, point3);
	}

	public bool Contains(Point point) {
		return Center.Distance(point) <= Radius;
	}

	public Polygon ToPolygon(int sides = 3) {
		if (sides < 3) {
			throw new ArgumentException("Requires at least 3 sides");
		}

		var spacing = Math.PI * (2d / sides);

		double x;
		double y;
		Point point;
		Point offset;

		var points = new List<Point>();

		for (var theta = 0d; theta < Math.PI * 2; theta += spacing) {
			x = Radius * Math.Cos(theta);
			y = Radius * Math.Sin(theta);

			offset = new Point(x, y);
			point = Center + offset;
			points.Add(point);
		}

		return new Polygon(points);
	}

	void FromCircumcircle(Point p0, Point p1, Point p2) {
		// https://codefound.wordpress.com/2013/02/21/how-to-compute-a-circumcircle/#more-58
		// https://en.wikipedia.org/wiki/Circumscribed_circle

		var dA = p0.Dot(p0);
		var dB = p1.Dot(p1);
		var dC = p2.Dot(p2);

		var aux1 = (dA * (p2.Y - p1.Y) + dB * (p0.Y - p2.Y) + dC * (p1.Y - p0.Y));
		var aux2 = -(dA * (p2.X - p1.X) + dB * (p0.X - p2.X) + dC * (p1.X - p0.X));

		var div = (2 * (p0.X * (p2.Y - p1.Y) + p1.X * (p0.Y - p2.Y) + p2.X * (p1.Y - p0.Y)));

		Center = new Point(aux1 / div, aux2 / div);
		Radius = Math.Max(Math.Max(Center.Distance(p0), Center.Distance(p1)), Center.Distance(p2));
	}

	double ToRadians(double value) => Math.PI / 180 * value;
}