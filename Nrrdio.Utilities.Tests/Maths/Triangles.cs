using Nrrdio.Utilities.Maths;

namespace Nrrdio.Utilities.Tests;

[TestClass]
public class Triangles {
	[TestMethod]
	public void AreEqual() {
		var testTriangle1 = new Triangle(new Point(1, -1.732051), new Point(0, -1.732051), new Point(0.5, -0.8660254));
		var testTriangle2 = new Triangle(new Point(0, -1.732051), new Point(0.5, -0.8660254), new Point(1, -1.732051));

		Assert.AreEqual(testTriangle1, testTriangle2);
	}

	[TestMethod]
	public void AreEqualReversed() {
		var testTriangle1 = new Triangle(new Point(1, -1.732051), new Point(0, -1.732051), new Point(0.5, -0.8660254));
		var testTriangle2 = new Triangle(new Point(0.5, -0.8660254), new Point(0, -1.732051), new Point(1, -1.732051));

		Assert.AreEqual(testTriangle1, testTriangle2);
	}

	[TestMethod]
	public void EqualsPolygon() {
		var testTriangle = new Triangle(new Point(1, -1.732051), new Point(0, -1.732051), new Point(0.5, -0.8660254));
		var testPolygon = new Polygon(new Point(0, -1.732051), new Point(0.5, -0.8660254), new Point(1, -1.732051));

		Assert.AreEqual(testTriangle, testPolygon);
	}

	[TestMethod]
	public void EqualsPolygonReversed() {
		var testTriangle = new Triangle(new Point(1, -1.732051), new Point(0, -1.732051), new Point(0.5, -0.8660254));
		var testPolygon = new Polygon(new Point(0.5, -0.8660254), new Point(0, -1.732051), new Point(1, -1.732051));

		Assert.AreEqual(testTriangle, testPolygon);
	}

	[TestMethod]
	public void AreNotEqual() {
		var testTriangle1 = new Triangle(new Point(1, -1.732051), new Point(0, -1.732051), new Point(0.5, -0.8660254));
		var testTriangle2 = new Triangle(new Point(0, -1.732051), new Point(0.5, -2.5980762), new Point(1, -1.732051));

		Assert.AreNotEqual(testTriangle1, testTriangle2);
	}

	[TestMethod]
	public void Invalid() {
		try {
			new Triangle(new Point(1, -1.732051), new Point(1, -1.732051), new Point(0.5, -0.8660254));
		}
		catch (ArgumentException) {
			return;
		}

		Assert.Fail("Should not get this far");
	}

	[TestMethod]
	public void Edges() {
		var testTriangle = new Triangle(new Point(1, -1.732051), new Point(0, -1.732051), new Point(0.5, -0.8660254));

		Assert.AreEqual(3, testTriangle.Edges.Count());
	}
}
