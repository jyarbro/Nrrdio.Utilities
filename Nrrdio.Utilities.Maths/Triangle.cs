namespace Nrrdio.Utilities.Maths;

public class Triangle : Polygon {
	public Triangle(Point v1, Point v2, Point v3) : base(v1, v2, v3) {
		if (v1 is null || v2 is null || v3 is null || v1 == v2 || v1 == v3 || v2 == v3) {
			throw new ArgumentException("Triangles require 3 unique points");
		}
	}
}
