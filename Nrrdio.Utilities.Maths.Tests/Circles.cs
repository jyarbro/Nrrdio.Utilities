﻿namespace Nrrdio.Utilities.Maths.Tests;

[TestClass]
public class Circles {
    [TestMethod]
    public void FromPointsWhereThreeAreColinear() {
        var centroid = new Point(493.15355408035782, 479.83073386567105);

        var points = new List<Point> {
            new Point(500, 473.2050807568877),
            new Point(488.03871928527934, 493.9226266783777),
            new Point(489.5854990797751, 475.54530826018879),
            new Point(501.83644387599566, 470.02426665841449),
        };

        var circle = new Circle(points, centroid);

        // Radius is infinity if this is done incorrectly
        Assert.IsTrue(double.IsNormal(circle.Radius));
    }

    [TestMethod]
	public void RectangleCenter() {
		var points = new List<Point> {
				new Point(2, 0),
				new Point(4, 4),
				new Point(6, 3),
				new Point(4, -1),
			};

		var polygon = new Polygon(points);
		var circle = new Circle(points, polygon.Centroid);

		Assert.AreEqual(new Point(4, 1.5), circle.Center);
	}

	[TestMethod]
	public void PolygonCircumcircleCenter() {
		var points = new List<Point> {
				new Point(2, 0),
				new Point(2, 4),
				new Point(4, 4),
				new Point(4, 3),
				new Point(5, 3),
				new Point(5, 1),
				new Point(4, 1),
				new Point(4, 0),
			};

		var polygon = new Polygon(points);
		var circle = new Circle(points, polygon.Centroid);

		Console.WriteLine($"Centroid: {polygon.Centroid}, Radius: {circle.Radius}");
		Console.WriteLine($"Circle Center: {circle.Center}");

		Assert.AreEqual(new Point(3, 2), circle.Center);
	}

	[TestMethod]
	public void PolygonCircumcircleCenterReversed() {
		var points = new List<Point> {
				new Point(4, 4),
				new Point(2, 4),
				new Point(2, 0),
				new Point(4, 0),
				new Point(4, 1),
				new Point(5, 1),
				new Point(5, 3),
				new Point(4, 3),
			};

		var polygon = new Polygon(points);
		var circle = new Circle(points, polygon.Centroid);

		Assert.AreEqual(new Point(3, 2), circle.Center);
	}

	[TestMethod]
	public void ComplexPolygonCircumcircleCenter() {
		var points = new List<Point> {
				new Point(2, 4),
				new Point(3, 3.5),
				new Point(2.5, 2.5),
				new Point(4.5, 1.5),
				new Point(6, -1),
				new Point(2.5, 0),
				new Point(2, 2),
				new Point(0, 1.5),
			};

		var polygon = new Polygon(points);
		var circle = new Circle(points, polygon.Centroid);

		Assert.AreEqual(new Point(3.28125, 0.925), circle.Center);
	}

	[TestMethod]
	public void ToComplexPolygon() {
		var points = new List<Point> {
				new Point(2, 0),
				new Point(4, 4),
				new Point(6, 3),
				new Point(4, -1),
			};

		var polygon = new Polygon(points);
		var circle = new Circle(points, polygon.Centroid);

		Console.WriteLine($"Centroid: {polygon.Centroid}");

		foreach (var point in circle.ToPolygon(6).Vertices) {
			Console.WriteLine(point);
		}

		Assert.AreEqual(63, circle.ToPolygon(63).Vertices.Count());
	}

	[TestMethod]
	public void ToShapes() {
		for (var i = 3; i < 30; i++) {
            var circle = new Circle(new Point(300, 300), 300).ToPolygon(i);
            Assert.AreEqual(i, circle.Vertices.Count);
        }
    }

	[TestMethod]
	public void PolygonContainsPoint() {
		var points = new List<Point> {
				new Point(2, 0),
				new Point(4, 4),
				new Point(6, 3),
				new Point(4, -1),
			};

		var polygon = new Polygon(points);
		var circle = new Circle(points, polygon.Centroid);

		var test = polygon.Centroid - new Point(circle.Radius, 0);

		Console.WriteLine($"Centroid: {polygon.Centroid}, Radius: {circle.Radius}");
		Console.WriteLine($"Test: {test}");

		foreach (var point in circle.ToPolygon(6).Vertices) {
			Console.WriteLine(point);
		}

		Assert.IsTrue(circle.ToPolygon(6).Vertices.Contains(test));
	}
}
